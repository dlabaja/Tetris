using Gdk;
using Gtk;

namespace DesktopTetris.GtkWindows;

public static class WindowManager
{
    public static MainWindow mainWindow;
    public static Dictionary<(int, int), BlockWindow> windows = new Dictionary<(int, int), BlockWindow>();

    static WindowManager()
    {
        mainWindow = new MainWindow();
    }

    public static void DisposeWindow((int, int) key)
    {
        Application.Invoke((_, _) => 
        {
            windows[key].Dispose();
            windows.Remove(key);
        });
    }

    public static void GetNewWindow((int x, int y) pos, (int x, int y) size, Color color)
    {
        Application.Invoke((sender, args) =>
        {
            windows[(pos.x, pos.y)] = new BlockWindow(Renderer.RelativeToAbsoluteCoords(pos), size, color);;
        });
    }
}
