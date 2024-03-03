using Gtk;
using System.ComponentModel;
using System.Diagnostics;

namespace DesktopTetris.GtkWindows;

public class MainWindow : Window
{
    private readonly Label scoreLabel;

    public MainWindow() : base(WindowType.Toplevel)
    {
        KeyPressEvent += KeyPress;
        DeleteEvent += OnDeleteEventHandler;
        FocusOutEvent += OnFocusOutEventHandler;
        Game.currentGame.ScoreChanged += OnScoreChanged;

        ActivateFocus();

        SetDefaultSize(400, 200);
        Move(0,0);
        
        KeepAbove = true;
        GrabFocus();
        
        scoreLabel = new Label("Score: 0");
        
        Add(scoreLabel);

        ShowAll();
    }

    private void OnScoreChanged(object? sender, EventArgs eventArgs)
    {
        scoreLabel.Text = $"Score: {Game.currentGame.Score}";
    }

    private void OnFocusOutEventHandler(object o, FocusOutEventArgs focusOutEventArgs)
    {
        ActivateFocus();
    }

    private void OnDeleteEventHandler(object o, DeleteEventArgs deleteEventArgs)
    {
        KeyPressEvent -= KeyPress;
        Game.currentGame.ScoreChanged -= OnScoreChanged;
        Application.Quit();
        Environment.Exit(0);
    }

    [GLib.ConnectBefore]
    private static void KeyPress(object sender, KeyPressEventArgs args)
    {
        Controls.CallAction(args.Event.Key);
    }
}
