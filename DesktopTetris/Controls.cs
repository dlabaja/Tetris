using Gdk;
using Action = System.Action;
using Key = Gtk.Key;

namespace DesktopTetris;

public static class Controls
{
    private static Dictionary<Gdk.Key, Action> controls = new Dictionary<Gdk.Key, Action>{
        {Gdk.Key.W, () => OnUpPress(null, EventArgs.Empty)},
        {Gdk.Key.Up, () => OnUpPress(null, EventArgs.Empty)},
        {Gdk.Key.S, () => OnDownPress(null, EventArgs.Empty)},
        {Gdk.Key.Down, () => OnDownPress(null, EventArgs.Empty)},
        {Gdk.Key.A, () => OnLeftPress(null, EventArgs.Empty)},
        {Gdk.Key.Left, () => OnLeftPress(null, EventArgs.Empty)},
        {Gdk.Key.D, () => OnRightPress(null, EventArgs.Empty)},
        {Gdk.Key.Right, () => OnRightPress(null, EventArgs.Empty)},
        {Gdk.Key.R, () => OnRotatePress(null, EventArgs.Empty)}
    };

    public static event EventHandler RightPress = OnRightPress;
    public static event EventHandler LeftPress = OnLeftPress;
    public static event EventHandler UpPress = OnUpPress;
    public static event EventHandler DownPress = OnDownPress;
    public static event EventHandler RotatePress = OnRotatePress;

    public static void CallAction(Gdk.Key key)
    {
        if (!controls.ContainsKey(key))
            return;

        controls[key]();
    }
    
    private static void OnRotatePress(object? sender, EventArgs e)
    {
        RotatePress.Invoke(sender, e);
    }

    private static void OnDownPress(object? sender, EventArgs e)
    {
        DownPress.Invoke(sender, e);
    }

    private static void OnUpPress(object? sender, EventArgs e)
    {
        UpPress.Invoke(sender, e);
    }

    private static void OnLeftPress(object? sender, EventArgs e)
    {
        LeftPress.Invoke(sender, e);
    }

    private static void OnRightPress(object? sender, EventArgs e)
    {
        RightPress.Invoke(sender, e);
    }
}
