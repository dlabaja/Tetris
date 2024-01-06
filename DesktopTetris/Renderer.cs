using DesktopTetris.Gtk;
using System.Diagnostics;
using Timer = System.Timers.Timer;

#pragma warning disable CS0612

namespace DesktopTetris;

public static class Renderer
{
    private static MainWindow mainWindow;
    private static readonly int blockSize;
    private static readonly int anchor;

    private static Rectangle?[,] rectanglesOld = new Rectangle?[16, 10];

    static Renderer()
    {
        mainWindow = new MainWindow();
        (int width, int height) desktopSize = (mainWindow.Screen.Width, mainWindow.Screen.Height);
        blockSize = (int)Math.Round((double)desktopSize.height / 16);
        anchor = (int)Math.Round(((double)desktopSize.width - 10 * blockSize) / 2);

        var frameTimer = new Timer(200);
        frameTimer.Elapsed += (_, _) => PrintNewFrame();
        //frameTimer.Start();

        for (int i = 0; i < 100; i++)
        {
            PrintNewFrame();
            Thread.Sleep(500);
        }
    }

    // converts relative grid coords to the coords on the screen
    public static (int x, int y) RelativeToAbsoluteCoords((int x, int y) pos) => (anchor + pos.x * blockSize, pos.y * blockSize);

    private static void PrintNewFrame()
    {
        var rectanglesNew = new Rectangle?[16, 10];

        foreach (var block in Program.currentGame.Blocks)
        {
            rectanglesNew = CalculateRectangles(block, rectanglesNew);
        }

        var _old = rectanglesOld;
        rectanglesNew = RemoveDuplicates(rectanglesNew); // passnutím listu se z nějakýho důvodu vytvoří reference

        foreach (var window in WindowManager.windows)
        {
            var key = window.Key;
            if (rectanglesOld[key.Item2, key.Item1] == null && rectanglesNew[key.Item2, key.Item1] == null)
            {
                try
                {
                    //window.Value.Dispose();
                }
                catch {}
            }
        }

        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (rectanglesNew[y, x] != null)
                {
                    var r = rectanglesNew[y, x]!.Value;
                    WindowManager.GetNewWindow((r.posX, r.posY), (r.sizeX, r.sizeY));
                    rectanglesOld[y, x] = r;
                }
            }
        }
    }

    private static Rectangle?[,] CalculateRectangles(Block block, Rectangle?[,] rectanglesNew)
    {
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

                rectanglesNew[r.posY, r.posX] = r;
                //Debug.WriteLine($"{r.posX};{r.posY}, {r.sizeX}x{r.sizeY}");
            }
        }

        return rectanglesNew;
    }

    // finds lowest number of rectangles in a tetris block 
    private static Rectangle GetRectSize(bool[,] matrice, int x, int y, ref List<(int, int)> counted)
    {
        var r = new Rectangle();
        for (int i = x; i < matrice.GetLength(1); i++)
        {
            if (matrice[y, i])
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
        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (rectanglesNew[y, x] == null && rectanglesOld[y, x] == null)
                    continue;

                if (rectanglesNew[y, x] != null && rectanglesOld[y, x] != null)
                {
                    if (rectanglesNew[y, x].Value.Equals(rectanglesOld[y, x].Value))
                    {
                        rectanglesNew[y, x] = null;
                        continue;
                    }
                }

                if (rectanglesNew[y, x] == null && rectanglesOld[y, x] != null)
                {
                    rectanglesOld[y, x] = null;
                }
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

        public Rectangle()
        {
        }
    }
}
