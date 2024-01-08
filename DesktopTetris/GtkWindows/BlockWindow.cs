using Gdk;
using Gtk;
using Window = Gtk.Window;

namespace DesktopTetris.GtkWindows;

public class BlockWindow : Window
{
    public BlockWindow((int x, int y) position, (int x, int y) size, Color color) : base(string.Empty)
    {
        Application.Invoke((_, _) => 
        {
            SetDefaultSize(size.x, size.y);
            Move(position.x, position.y);
            ModifyBg(StateType.Normal, color);
            AcceptFocus = false;

            KeepAbove = true;
            Decorated = false;
            Sensitive = false;
            
            Show();
        });
    }
}

