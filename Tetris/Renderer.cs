using Mindmagma.Curses;
using Timer = System.Timers.Timer;

namespace Tetris;

public class Renderer
{
    public List<Block> blocks = new List<Block>();
    private nint window;
    
    public Renderer(int sizeX, int sizeY)
    {
        for (short i = 1; i < 8; i++)
            NCurses.InitPair(i, i, 0);

        window = NCurses.NewWindow(sizeX, sizeY, 0, 0);
        
        var frameTimer = new Timer(20);
        frameTimer.Elapsed += (_, _) => PrintNewFrame();
        frameTimer.Start();
    }

    private void PrintNewFrame()
    {
        NCurses.Clear();
        foreach (var block in blocks)
        {
            PrintBlock(block);
        }
    }

    private static void PrintBlock(Block block)
    {
        NCurses.AttributeOn(NCurses.ColorPair(block.color));
        var anchor = block.anchorPosition;
        var matrice = block.GetMatrice();
        for (int x = 0; x < matrice.GetLength(0); x++)
            for (int y = 0; y < matrice.GetLength(1); y++)
                if (matrice[x, y])
                    NCurses.MoveAddChar(anchor[1] + y, anchor[1] + y, '█');
        NCurses.AttributeOff(NCurses.ColorPair(block.color));
        NCurses.Refresh();
    }
    
    /*
        COLOR_BLACK   0
        COLOR_RED     1
        COLOR_GREEN   2
        COLOR_YELLOW  3
        COLOR_BLUE    4
        COLOR_MAGENTA 5
        COLOR_CYAN    6
        COLOR_WHITE   7
     */
}
