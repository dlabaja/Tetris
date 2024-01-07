using DesktopTetris.GtkWindows;
using Gdk;
using System.Diagnostics;
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
        
        blockFallTimer = new Timer(1000);
        blockFallTimer.Elapsed += (_, _) => MoveBlockDown();
        blockFallTimer.Start();
    }

    private void EndGame()
    {
        Debug.WriteLine("end");
        blockFallTimer.Stop();
        foreach (var block in Blocks)
        {
            block.Color = new Color(128, 128, 128);
        }

        CurrentBlock.Color = new Color(128, 128, 128);
    }

    private void SpawnNewBlock()
    {
        Blocks.Add(CurrentBlock);
        CurrentBlock = new Block();
        Score++;
        WindowManager.mainWindow.ChangeScore(Score);
        
        for (int y = 0; y < CurrentBlock.Matrice.GetLength(0); y++)
        {
            for (int x = 0; x < CurrentBlock.Matrice.GetLength(1); x++)
            {
                var pos = CurrentBlock.GetMapRelativePosition(x, y);
                if (CurrentBlock.Matrice[y, x] && fallenBlocksMap[pos.y, pos.x])
                {
                    EndGame();
                }
            }
        }
    }

    private void MoveBlockDown()
    {
        CurrentBlock.Move(0,1);
        RegenMap();
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

    private void RegenMap()
    {
        var _map = new bool[16, 10];
        foreach (var block in Blocks)
        {
            for (int x = 0; x < block.Matrice.GetLength(1); x++)
            {
                for (int y = 0; y < block.Matrice.GetLength(0); y++)
                {
                    var pos = block.GetMapRelativePosition(x, y);
                    
                    if (pos.x is < 0 or >= 10 || pos.y is < 0 or >= 16)
                        continue;
                    _map[pos.y, pos.x] = block.Matrice[y, x];
                }
            }
        }

        fallenBlocksMap = _map;
    }
}
