using DesktopTetris.Gtk;
using Gdk;
using Gtk;
using Timer = System.Timers.Timer;
#pragma warning disable CS0612

namespace DesktopTetris;

public static class Renderer
{
    private static MainWindow mainWindow;
    private static readonly int blockSize;
    private static readonly (int width, int height) desktopSize;
    private static readonly int anchor;
    private static bool[,] renderedMap = new bool[16, 10];

    public static Dictionary<(int, int), BlockWindow> windows = new Dictionary<(int, int), BlockWindow>();

    static Renderer()
    {
        mainWindow = new MainWindow();
        desktopSize = (mainWindow.Screen.Width, mainWindow.Screen.Height);
        blockSize = (int)Math.Round((double)desktopSize.height / 16);
        anchor = (int)Math.Round(((double)desktopSize.width - 10 * blockSize) / 2);

        var frameTimer = new Timer(200);
        frameTimer.Elapsed += (_, _) => PrintNewFrame();
        frameTimer.Start();

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                windows.Add((x, y), new BlockWindow(blockSize, RelativeToAbsoluteCoords(x, y)));
            }
        }
    }

    private static (int x, int y) RelativeToAbsoluteCoords(int x, int y) => (anchor + x * blockSize, y * blockSize);

    private static void PrintNewFrame()
    {
        renderedMap = new bool[16, 10];
        foreach (var block in Program.currentGame.Blocks)
        {
            PrintBlock(block);
        }
        HideUnusedWindows();
    }

    private static void PrintBlock(Block block)
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

                var r = GetRectSize(matrice, x, y, ref counted);
                ShowWindow(block, r, x, y);
            }
        }
    }

    private static void HideUnusedWindows()
    {
        var map = Program.currentGame.map;
        for (int y = 0; y < map.GetLength(0) - 1; y++)
        {
            for (int x = 0; x < map.GetLength(1) - 1; x++)
            {
                if (renderedMap[y, x])
                    continue;
                
                var window = windows[(x, y)];
                if (!window.IsVisible)
                    continue;
                
                window.Hide();
                window.SetDefaultSize(blockSize, blockSize);
                return;
            }
        }
    }

    private static void ShowWindow(Block block, Rectangle r, int x , int y)
    {
        var pos = block.GetMapRelativePosition(x, y);
        if (pos.x is < 0 or >= 10 || pos.y is < 0 or >= 16)
            return;

        renderedMap[pos.y, pos.x] = true;
            
        var window = windows[pos];

        window.ModifyBg(StateType.Normal, block.Color);
        window.DefaultSize = new Size(r.sizeX, r.sizeY);
        window.Show();
    }

    private static Rectangle GetRectSize(bool[,] matrice, int x, int y, ref List<(int, int)> counted)
    {
        var r = new Rectangle(new[]{x, y});
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
    
    private struct Rectangle
    {
        public int sizeX = 0;
        public int sizeY = blockSize;
        public int[] anchor;

        public Rectangle(int[] anchor)
        {
            this.anchor = anchor;
        }
    }
}
