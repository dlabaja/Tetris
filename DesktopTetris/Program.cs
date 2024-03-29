﻿using DesktopTetris.GtkWindows;
using Gtk;
using System.Runtime.CompilerServices;

namespace DesktopTetris;

public static class Program
{
    private static void Main()
    {
        Application.Init();
        new Thread(o =>
        {
            var g = new Game();
            g.AddBlock(new Block());
            
            RuntimeHelpers.RunClassConstructor(typeof(Renderer).TypeHandle);
            RuntimeHelpers.RunClassConstructor(typeof(WindowManager).TypeHandle);
            Thread.Sleep(-1);
        }).Start();

        Application.Run();
    }
}
