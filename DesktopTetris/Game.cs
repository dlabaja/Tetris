using DesktopTetris.GtkWindows;
using Gdk;
using Gtk;
using System.Diagnostics;
using System.Text;
using System.Timers;
using Timer = System.Timers.Timer;

namespace DesktopTetris;

public class Game
{
    public static Game currentGame = null!;
    public Block CurrentBlock { get; private set; }
    public List<Block> Blocks { get; } = new List<Block>();
    private int GameTime { get; set; }
    public int Level { get; private set; } = 1;
    private int Score { get; set; }

    public bool[,] fallenBlocksMap = new bool[mapHeight, mapWidth];
    public const int mapWidth = 10;
    public const int mapHeight = 16;
    public event EventHandler GameEnded;

    private Timer blockFallTimer;

    public Game()
    {
        currentGame = this;
        InitTimers();
        CurrentBlock = new Block();

        GameEnded += OnGameEnded;
    }

    private void InitTimers()
    {
        var gameTimer = new Timer(1000);
        gameTimer.Elapsed += (_, _) => GameTime++;
        gameTimer.Start();

        blockFallTimer = new Timer(400);
        blockFallTimer.Elapsed += (_, _) => MoveBlockDown();
        blockFallTimer.Start();
    }

    private void OnGameEnded(object? sender, EventArgs e)
    {
        blockFallTimer.Stop();
        
    }

    private bool NoRoomForNewBlock()
    {
        for (int x = 0; x < mapWidth; x++)
            if (fallenBlocksMap[0, x])
                return true;

        return false;
    }

    public void SpawnNewBlock()
    {
        Blocks.Add(CurrentBlock);

        Score++;
        WindowManager.mainWindow.ChangeScore(Score);

        RegenMap();
        
        if (NoRoomForNewBlock())
        {
            GameEnded.Invoke(this, EventArgs.Empty);
            return;
        }
        
        CurrentBlock = new Block();
    }

    private void MoveBlockDown()
    {
        CurrentBlock.Move(0, 1);
    }

    public void PrintMap()
    {
        var s = new StringBuilder();
        s.Append("--------\n");
        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (fallenBlocksMap[y, x])
                {
                    s.Append("1");
                    continue;
                }

                s.Append("0");
            }

            s.Append("\n");
        }

        s.Append("--------");
        Debug.WriteLine(s);
    }

    public void RegenMap()
    {
        var _map = new bool[16, 10];
        foreach (var block in Blocks)
        {
            for (int y = 0; y < block.Matrice.GetLength(0); y++)
            {
                for (int x = 0; x < block.Matrice.GetLength(1); x++)
                {
                    var pos = block.GetMapRelativePosition(x, y);

                    if (pos.x is < 0 or >= 10 || pos.y is < 0 or >= 16)
                        continue;

                    if (block.Matrice[y, x])
                        _map[pos.y, pos.x] = block.Matrice[y, x];
                }
            }
        }

        fallenBlocksMap = _map;
    }
}
