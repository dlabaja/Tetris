using Gtk;

namespace DesktopTetris.Gtk;

public class BlockWindow : Window
{
    public BlockWindow((int x, int y) position, (int x, int y) size) : base(string.Empty)
    {
        SetDefaultSize(size.x, size.y);
        Move(position.x, position.y);

        Decorated = false;
        
        Show();
    }
}

