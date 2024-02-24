using DesktopTetris.GtkWindows;
using Gdk;
using Application = Gtk.Application;
using Timer = System.Timers.Timer;

#pragma warning disable CS0612

namespace DesktopTetris;

public static class Renderer
{
    private const int framePeriod = 20;
    private static readonly int blockSize;
    private static readonly (int x, int y) anchor;

    private static Rectangle?[,] rectanglesOld = new Rectangle?[Game.mapHeight, Game.mapWidth];

    static Renderer()
    {
        (int width, int height) desktopSize = (WindowManager.mainWindow.Screen.Width, WindowManager.mainWindow.Screen.Height);
        blockSize = (int)Math.Round(((double)desktopSize.height - 100) / Game.mapHeight);
        anchor = ((int)Math.Round(((double)desktopSize.width - Game.mapWidth * blockSize) / 2), blockSize);

        var frameTimer = new Timer(framePeriod);
        frameTimer.Elapsed += (_, _) => PrintNewFrame();
        frameTimer.Start();
    }

    // converts relative grid coords to the coords on the screen
    public static (int x, int y) RelativeToAbsoluteCoords((int x, int y) pos) => (anchor.x + pos.x * blockSize, pos.y * blockSize);

    private static void PrintNewFrame()
    {
        var rectanglesNew = new Rectangle?[Game.mapHeight, Game.mapWidth];

        var blocks = Game.currentGame.Blocks;

        // create rectangles for all blocks in the game
        foreach (var block in blocks)
        {
            CalculateRectangles(block, ref rectanglesNew);
        }

        rectanglesNew = RemoveDuplicates(rectanglesNew); // passnutím listu se z nějakýho důvodu vytvoří reference

        // dispose unused windows by both new and old list
        foreach (var (key, _) in WindowManager.windows)
        {
            if (rectanglesOld[key.Item2, key.Item1] == null && rectanglesNew[key.Item2, key.Item1] == null)
                WindowManager.DisposeWindow(key);
        }
        WindowManager.CleanRemnants();

        // render windows left in new list
        for (int y = 0; y < Game.mapHeight; y++)
        {
            for (int x = 0; x < Game.mapWidth; x++)
            {
                if (rectanglesNew[y, x] == null)
                    continue;
                
                var r = rectanglesNew[y, x]!.Value;
                WindowManager.GetNewWindow((r.posX, r.posY), (r.sizeX, r.sizeY), r.color, r.debugText);
                rectanglesOld[y, x] = r;
            }
        }
    }

    private static void CalculateRectangles(Block? block, ref Rectangle?[,] rectanglesNew)
    {
        if (block == null)
        {
            return;
        }
        
        var matrice = block.Matrice;
        var counted = new List<(int, int)>();

        for (int y = 0; y < matrice.GetLength(0); y++)
        {
            for (int x = 0; x < matrice.GetLength(1); x++)
            {
                if (!matrice[y, x] || counted.Contains((x, y)) || block.GetMapRelativePosition(x, y).y < 0)
                {
                    continue;
                }

                var pos = block.GetMapRelativePosition(x, y);
                var r = GetRectSize(matrice, x, y, ref counted);
                r.posX = pos.x;
                r.posY = pos.y;
                r.color = block.Color;

                if (pos.y > 15)
                {
                    continue;
                }
                rectanglesNew[r.posY, r.posX] = r;
            }
        }
    }

    // finds lowest number of rectangles in a tetris block 
    private static Rectangle GetRectSize(bool[,] matrice, int x, int y, ref List<(int, int)> counted)
    {
        var r = new Rectangle();
        for (int i = x; i < matrice.GetLength(1); i++)
        {
            if (matrice[y, i] && !counted.Contains((i, y)))
            {
                counted.Add((i, y));
                r.sizeX += blockSize;
                continue;
            }

            break;
        }

        for (int j = y + 1; j < matrice.GetLength(0); j++)
        {
            for (int i = 0; i < r.sizeX / blockSize; i++)
                if (!matrice[j, i + x])
                    return r;

            for (int i = 0; i < r.sizeX / blockSize; i++)
                if (matrice[j, i + x])
                    counted.Add((i + x, j));

            r.sizeY += blockSize;
        }

        return r;
    }

    // removes duplicates in the new and old list
    private static Rectangle?[,] RemoveDuplicates(Rectangle?[,] rectanglesNew)
    {
        for (int y = 0; y < Game.mapHeight; y++)
        {
            for (int x = 0; x < Game.mapWidth; x++)
            {
                if (rectanglesNew[y, x] == null && rectanglesOld[y, x] == null)
                    continue;

                if (rectanglesNew[y, x] != null && rectanglesOld[y, x] != null)
                {
                    if (rectanglesNew[y, x]!.Value.Equals(rectanglesOld[y, x]!.Value))
                    {
                        rectanglesNew[y, x] = null;
                        continue;
                    }
                }

                if (rectanglesNew[y, x] == null && rectanglesOld[y, x] != null)
                    rectanglesOld[y, x] = null;
            }
        }

        return rectanglesNew;
    }

    private struct Rectangle
    {
        public int posX = 0;
        public int posY = 0;

        public int sizeX = 0;
        public int sizeY = blockSize;
        public string debugText;

        public Color color;

        public Rectangle()
        {
        }
    }
}
