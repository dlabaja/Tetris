﻿using Gtk;
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
            currentGame = new Game((20, 20));
            RuntimeHelpers.RunClassConstructor(typeof(Renderer).TypeHandle);
            Thread.Sleep(-1);
        }).Start();
        Application.Run();
    }
}
