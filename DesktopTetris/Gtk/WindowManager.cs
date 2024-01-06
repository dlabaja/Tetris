using Gtk;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DesktopTetris.Gtk;

public static class WindowManager
{
    public static Dictionary<(int, int), BlockWindow> windows = new Dictionary<(int, int), BlockWindow>();
    private static ObservableCollection<BlockWindow> hiddenWindows = new ObservableCollection<BlockWindow>();

    static WindowManager()
    {
        hiddenWindows.CollectionChanged += (sender, args) =>
        {
            if (hiddenWindows.Count <= 6)
                return;
            
            hiddenWindows[0].Close();
            hiddenWindows.RemoveAt(0);
        };
    }

    public static void HideWindow((int x, int y) pos)
    {
        try
        {
            var window = windows[(pos.y, pos.x)];
            if (!window.IsVisible)
            {
                return;
            }

            windows.Remove((pos.y, pos.x));
            hiddenWindows.Add(window);
            window.Hide();
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

        windows[(pos.y, pos.x)] = window;

        return window;
    }
}
