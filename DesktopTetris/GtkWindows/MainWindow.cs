using Gtk;
using System.ComponentModel;
using System.Diagnostics;

namespace DesktopTetris.GtkWindows;

public class MainWindow : Window
{
    private readonly Label scoreLabel;
    private readonly Label levelLabel;

    public void ChangeScore(int score) => scoreLabel.Text = $"Score: {score}";
    public void ChangeLevel(int level) => levelLabel.Text = $"Level: {level}";

    public MainWindow() : base(WindowType.Toplevel)
    {
        KeyPressEvent += KeyPress;
        DeleteEvent += OnDeleteEventHandler;
        FocusOutEvent += OnFocusOutEventHandler;

        ActivateFocus();

        SetDefaultSize(400, 200);
        Move(0,0);
        
        KeepAbove = true;
        GrabFocus();
        
        scoreLabel = new Label("Score: 0");
        levelLabel = new Label("Level: 1");
        
        Add(scoreLabel);
        //Add(levelLabel);

        ShowAll();
    }

    private void OnFocusOutEventHandler(object o, FocusOutEventArgs focusOutEventArgs)
    {
        ActivateFocus();
    }

    private void OnDeleteEventHandler(object o, DeleteEventArgs deleteEventArgs)
    {
        Application.Quit();
        Environment.Exit(0);
        KeyPressEvent -= KeyPress;
    }

    [GLib.ConnectBefore]
    private static void KeyPress(object sender, KeyPressEventArgs args)
    {
        Controls.CallAction(args.Event.Key);
    }
}
