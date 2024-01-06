using Gtk;

namespace DesktopTetris.Gtk;

public class BlockWindow : Window
{
    public BlockWindow(int length, (int x, int y) position) : base("Desktop Tetris")
    {
        SetDefaultSize(length, length);
        Move(position.x, position.y);

        Decorated = false;
    }
}

