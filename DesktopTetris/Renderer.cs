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
        var rectanglesNew = new List<Rectangle>();
        foreach (var block in Program.currentGame.Blocks)
        {
            rectanglesNew = CalculateRectangles(block, rectanglesNew);
        }
        
        Debug.WriteLine($"-{rectanglesNew.Count}x{rectanglesOld.Count}");
        var _rectanglesNew = RemoveDuplicates(rectanglesNew);
        Debug.WriteLine($"I{rectanglesNew.Count}x{rectanglesOld.Count}x{_rectanglesNew.Count}");

        foreach (var r in rectanglesOld)
        {
            WindowManager.HideWindow((r.posX, r.posY));
        }
        
        foreach (var r in _rectanglesNew)
        {
            WindowManager.GetNewWindow((r.posX, r.posY), (r.sizeX, r.sizeY));
        }
        
        rectanglesOld = rectanglesNew;
    }
    
    private static List<Rectangle> CalculateRectangles(Block block, List<Rectangle> rectanglesNew)
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
                
                rectanglesNew.Add(r);
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
    private static List<Rectangle> RemoveDuplicates(List<Rectangle> rectanglesNew)
    {
        /*var _new = new List<Rectangle>();
        var _old = new List<Rectangle>();
        for (int i = 0; i < rectanglesNew.Count; i++)
        {
            for (int j = 0; j < rectanglesOld.Count; j++)
            {
                if (rectanglesNew[i].posX == rectanglesOld[j].posX
                    && rectanglesNew[i].posY == rectanglesOld[j].posY
                    && rectanglesNew[i].sizeX == rectanglesOld[j].sizeX
                    && rectanglesNew[i].sizeY == rectanglesOld[j].sizeY)
                {
                    _new.Add(rectanglesNew[i]);
                    _old.Add(rectanglesOld[j]);
                }
            }
        }
        
        rectanglesOld.AddRange(_old);
        rectanglesOld = rectanglesOld.Distinct().ToList();

        _new.AddRange(rectanglesNew);

        return _new.Distinct().ToList();*/

        for (int i = 0; i < rectanglesNew.Count; i++)
        {
            for (int j = 0; j < rectanglesOld.Count; j++)
            {
                if (rectanglesNew[i].posX == rectanglesOld[j].posX
                    && rectanglesNew[i].posY == rectanglesOld[j].posY
                    && rectanglesNew[i].sizeX == rectanglesOld[j].sizeX
                    && rectanglesNew[i].sizeY == rectanglesOld[j].sizeY)
                {
                    try
                    {
                        rectanglesOld.RemoveAt(j);
                        _rectanglesNew.RemoveAt(i);
                    }
                    catch{}
                }
            }
        }

        return _rectanglesNew;
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
