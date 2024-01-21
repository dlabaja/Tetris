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
            try
            {
                windows[key].Dispose();
                windows.Remove(key);
            }
            catch{}
        });
    }

    public static void CleanRemnants()
    {
        var values = windows.Values;
        var gtkWindows = GetAllBlockWindows();
        foreach (var window in gtkWindows.Where(window => !values.Contains(window)))
        {
            window.Dispose();
        }
    }

    public static void GetNewWindow((int x, int y) pos, (int x, int y) size, Color color)
    {
        Application.Invoke((_, _) =>
        {
            windows[(pos.x, pos.y)] = new BlockWindow(Renderer.RelativeToAbsoluteCoords(pos), size, color);
        });
    }

    public static List<Gtk.Window> GetAllBlockWindows() => Gtk.Window.ListToplevels().Where(x => x.GetType() != typeof(MainWindow)).ToList();
}
