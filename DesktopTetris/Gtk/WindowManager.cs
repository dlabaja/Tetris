using Gtk;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DesktopTetris.Gtk;

public static class WindowManager
{
    public static MainWindow mainWindow;
    public static Dictionary<(int, int), BlockWindow> windows = new Dictionary<(int, int), BlockWindow>();
    private static ObservableCollection<BlockWindow> hiddenWindows = new ObservableCollection<BlockWindow>();

    static WindowManager()
    {
        mainWindow = new MainWindow();
        /*hiddenWindows.CollectionChanged += (sender, args) =>
        {
            if (hiddenWindows.Count <= 6)
                return;
            
            windows.Remove(windows.Values.Where(x => x == hiddenWindows[0]).First())
            hiddenWindows[0].Close();
            hiddenWindows.RemoveAt(0);
        };*/
    }

    public static void HideWindow((int x, int y) pos)
    {
        try
        {
            var window = windows[(pos.x, pos.y)];
            if (!window.IsVisible)
            {
                return;
            }
            
            //hiddenWindows.Add(window);
            window.Dispose();
        }
        catch{}
    }

    public static BlockWindow GetNewWindow((int x, int y) pos, (int x, int y) size)
    {
        BlockWindow window;
        if (hiddenWindows.Any())
        {
            window = hiddenWindows[0];
            hiddenWindows.RemoveAt(0);
        }
        else
        {
            window = new BlockWindow(Renderer.RelativeToAbsoluteCoords(pos), size);
        }

        windows[(pos.x, pos.y)] = window;

        return window;
    }
}
