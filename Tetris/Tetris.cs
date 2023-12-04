using Mindmagma.Curses;
using System.Timers;

namespace Tetris;

internal abstract class Tetris
{
    private static void Main()
    {
        NCurses.InitScreen();
        NCurses.StartColor();
        NCurses.NoEcho();

        var renderer = new Renderer(20, 20);
        renderer.blocks.Add(new Block());
        
        NCurses.GetChar();
        NCurses.EndWin();
    }

    private static void TOnElapsed(object? sender, ElapsedEventArgs e)
    {
        throw new NotImplementedException();
    }
}
