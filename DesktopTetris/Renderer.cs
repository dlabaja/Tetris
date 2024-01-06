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
    
    private static List<Rectangle> rectanglesOld = new List<Rectangle>();
    private static List<Rectangle> rectanglesNew = new List<Rectangle>();

    static Renderer()
    {
        mainWindow = new MainWindow();
        (int width, int height) desktopSize = (mainWindow.Screen.Width, mainWindow.Screen.Height);
        blockSize = (int)Math.Round((double)desktopSize.height / 16);
        anchor = (int)Math.Round(((double)desktopSize.width - 10 * blockSize) / 2);

        var frameTimer = new Timer(200);
        frameTimer.Elapsed += (_, _) => PrintNewFrame();
        frameTimer.Start();
    }

    // converts relative grid coords to the coords on the screen
    public static (int x, int y) RelativeToAbsoluteCoords((int x, int y) pos) => (anchor + pos.x * blockSize, pos.y * blockSize);

    private static void PrintNewFrame()
    {
        rectanglesOld = rectanglesNew;
        rectanglesNew.Clear();
        
        foreach (var block in Program.currentGame.Blocks)
        {
            CalculateRectangles(block);
        }
        
        var _rectanglesNew = rectanglesNew;
        Debug.WriteLine($"{rectanglesNew.Count}");
        RemoveDuplicates(ref _rectanglesNew);
        Debug.WriteLine($"{rectanglesNew.Count}");

        foreach (var r in _rectanglesNew)
        {
            WindowManager.GetNewWindow((r.posX, r.posY), (r.sizeX, r.sizeY));
        }
        
        /*foreach (var r in rectanglesOld)
        {
            WindowManager.HideWindow((r.posX, r.posY));
        }*/
    }
    
    private static void CalculateRectangles(Block block)
    {
        var matrice = block.Matrice;
        var counted = new List<(int, int)>();
        
        for (int y = 0; y < matrice.GetLength(0); y++)
        {
            for (int x = 0; x < matrice.GetLength(1); x++)
            {
                if (!matrice[y, x] || counted.Contains((x, y)))
                {
                    continue;
                }

                var pos = block.GetMapRelativePosition(x, y);
                var r = GetRectSize(matrice, x, y, ref counted);
                r.posX = pos.x;
                r.posY = pos.y;
                
                rectanglesNew.Add(r);
                //Debug.WriteLine($"{r.posX};{r.posY}, {r.sizeX}x{r.sizeY}");
            }
        }
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
    private static void RemoveDuplicates(ref List<Rectangle> _rectanglesNew)
    {
        for (int i = 0; i < _rectanglesNew.Count - 1; i++)
        {
            for (int j = 0; j < rectanglesOld.Count - 1; j++)
            {
                if (_rectanglesNew[i].posX == rectanglesOld[j].posX
                    && _rectanglesNew[i].posY == rectanglesOld[j].posY
                    && _rectanglesNew[i].sizeX == rectanglesOld[j].sizeX
                    && _rectanglesNew[i].sizeY == rectanglesOld[j].sizeY)
                {
                    _rectanglesNew.RemoveAt(i);
                    rectanglesOld.RemoveAt(j);
                }
            }
        }
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
