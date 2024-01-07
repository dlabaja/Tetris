using Gdk;

namespace DesktopTetris.GtkWindows;

public static class WindowManager
{
    public static MainWindow mainWindow;
    public static Dictionary<(int, int), BlockWindow> windows = new Dictionary<(int, int), BlockWindow>();

    static WindowManager()
    {
        mainWindow = new MainWindow();
    }

    public static BlockWindow GetNewWindow((int x, int y) pos, (int x, int y) size, Color color)
    {
        var window = new BlockWindow(Renderer.RelativeToAbsoluteCoords(pos), size, color);

        windows[(pos.x, pos.y)] = window;

        return window;
    }
}
