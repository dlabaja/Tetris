using DesktopTetris.GtkWindows;
using Gtk;
using System.Runtime.CompilerServices;

namespace DesktopTetris;

public static class Program
{
    public static Game currentGame;

    private static void Main()
    {
        Application.Init();
        new Thread(o =>
        {
            RuntimeHelpers.RunClassConstructor(typeof(Renderer).TypeHandle);
            RuntimeHelpers.RunClassConstructor(typeof(WindowManager).TypeHandle);
            currentGame = new Game((20, 20));
            Thread.Sleep(-1);
        }).Start();

        Application.Run();
    }
}
