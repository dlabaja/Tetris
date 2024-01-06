using Gtk;

namespace DesktopTetris.Gtk;

public class MainWindow : Window
{
    public Label scoreLabel;
    public Label levelLabel;

    public void ChangeScore(int score) => scoreLabel.Text = $"Score: {score}";
    public void ChangeLevel(int level) => levelLabel.Text = $"Level: {level}";

    public MainWindow() : base("Desktop Tetris")
    {
        SetDefaultSize(400, 200);
        Move(0,0);
        DeleteEvent += (o, args) =>
        {
            foreach (var window in Screen.ToplevelWindows)
            {
                window.Dispose();
            }

            Application.Quit();
        };

        scoreLabel = new Label("Score: 0");
        levelLabel = new Label("Level: 1");
        
        Add(scoreLabel);
        //Add(levelLabel);

        ShowAll();
    }
}
