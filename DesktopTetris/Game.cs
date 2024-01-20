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
    public Block CurrentBlock { get; private set; }
    public List<Block> Blocks { get; private set; } = new List<Block>();
    public int GameTime { get; private set; } = 0;
    public int Level { get; private set; } = 1;
    public int Score { get; private set; } = 0;
    public (int, int) Size { get; private set; }

    public bool[,] fallenBlocksMap = new bool[16, 10];
    public static int mapWidth;
    public static int mapHeight;

    private Timer blockFallTimer;

    public Game((int, int) size)
    {
        Size = size;

        InitTimers();
        CurrentBlock = new Block();
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

    private void EndGame()
    {
        blockFallTimer.Stop();

        foreach (var block in Blocks)
        {
            block.Color = new Color(128, 128, 128);
        }
        
        foreach (var (_, window) in WindowManager.windows)
        {
            Application.Invoke((_, _) => window.ModifyBg(StateType.Normal, new Color(128, 128, 128)));
        }
    }

    private void SpawnNewBlock()
    {
        CurrentBlock.UnhookEvents();
        Blocks.Add(CurrentBlock);
        RegenMap();
        Score++;
        WindowManager.mainWindow.ChangeScore(Score);

        var block = new Block();
        
        // check if there is a room for block to spawn
        for (int y = 0; y < block.Matrice.GetLength(0); y++)
        {
            for (int x = 0; x < block.Matrice.GetLength(0); x++)
            {
                var pos = block.GetMapRelativePosition(x, y);
                if (!block.Matrice[y, x] || !fallenBlocksMap[pos.y, pos.x])
                    continue;
                EndGame();
                return;
            }
        }

        CurrentBlock = block;
    }

    private void MoveBlockDown()
    {
        // game has ended
        if (!blockFallTimer.Enabled)
        {
            return;
        }
        
        CurrentBlock.Move(0, 1);
        if (IsAtBottom() || Collided())
        {
            SpawnNewBlock();
        }
    }

    private bool IsAtBottom()
    {
        var lowestY = 0;
        for (int y = 0; y < CurrentBlock.Matrice.GetLength(0); y++)
        {
            for (int x = 0; x < CurrentBlock.Matrice.GetLength(1); x++)
            {
                if (CurrentBlock.Matrice[y, x])
                {
                    lowestY = CurrentBlock.GetMapRelativePosition(x, y).y;
                }
            }
        }

        return lowestY > 14;
    }

    private bool Collided()
    {
        for (int y = 0; y < CurrentBlock.Matrice.GetLength(0); y++)
        {
            for (int x = 0; x < CurrentBlock.Matrice.GetLength(1); x++)
            {
                var pos = CurrentBlock.GetMapRelativePosition(x, y);
                if (CurrentBlock.Matrice[y, x] && fallenBlocksMap[pos.y + 1, pos.x])
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void PrintMap()
    {
        var s = new StringBuilder();
        s.Append("--------");
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
