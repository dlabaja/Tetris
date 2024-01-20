using Gdk;

namespace DesktopTetris;

public static class Controls
{
    private static Dictionary<Key, Action> controls = new Dictionary<Key, Action>
    {
        { Key.s, () => OnDownPress(null, EventArgs.Empty) },
        { Key.Down, () => OnDownPress(null, EventArgs.Empty) },
        { Key.a, () => OnLeftPress(null, EventArgs.Empty) },
        { Key.Left, () => OnLeftPress(null, EventArgs.Empty) },
        { Key.d, () => OnRightPress(null, EventArgs.Empty) },
        { Key.Right, () => OnRightPress(null, EventArgs.Empty) },
        { Key.r, () => OnRotatePress(null, EventArgs.Empty) }
    };

    public static event EventHandler RightPress;
    public static event EventHandler LeftPress;
    public static event EventHandler DownPress;
    public static event EventHandler RotatePress;

    public static void CallAction(Key key)
    {
        if (!controls.ContainsKey(key))
            return;

        controls[key]();
    }

    private static void OnRotatePress(object? sender, EventArgs e)
    {
        RotatePress?.Invoke(sender, e);
    }

    private static void OnDownPress(object? sender, EventArgs e)
    {
        DownPress?.Invoke(sender, e);
    }

    private static void OnLeftPress(object? sender, EventArgs e)
    {
        LeftPress?.Invoke(sender, e);
    }

    private static void OnRightPress(object? sender, EventArgs e)
    {
        RightPress?.Invoke(sender, e);
    }
}
