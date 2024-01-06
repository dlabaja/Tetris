using System.Timers;
using Timer = System.Timers.Timer;

namespace DesktopTetris;

public class Game
{
    public Block CurrentBlock { get; private set; }
    private List<int[]> collided;
    public List<Block> Blocks { get; private set; } = new List<Block>();
    public int GameTime { get; private set; } = 0;
    public int Level { get; private set; } = 1;
    public (int, int) Size { get; private set; }

    public bool[,] map = new bool[16, 10];

    public Game((int, int) size)
    {
        Size = size;

        InitTimers();
        CurrentBlock = new Block();
        Blocks.Add(CurrentBlock);
    }

    private void InitTimers()
    {
        var gameTimer = new Timer(1000);
        gameTimer.Elapsed += (_, _) => GameTime++;
        gameTimer.Start();
        
        var blockFallTimer = new Timer(1000);
        blockFallTimer.Elapsed += (_, _) => MoveBlockDown();
        blockFallTimer.Start();
    }

    private void MoveBlockDown()
    {
        CurrentBlock.Move(0,1);
        RegenMap();
    }

    private void CheckMaxY(Block block)
    {
        
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

        map = _map;
    }

    private void CheckCollision()
    {
        
    }

    private void EndGame()
    {
        
    }
}
