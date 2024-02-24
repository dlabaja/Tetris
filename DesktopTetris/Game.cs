using DesktopTetris.GtkWindows;
using System.Timers;
using Timer = System.Timers.Timer;

namespace DesktopTetris;

public class Game
{
    public const int mapHeight = 16;
    public const int mapWidth = 10;

    public static Game currentGame = null!;
    
    public Block[,] Map => (Block[,])map.Clone();
    public IEnumerable<Block> Blocks => blocks.AsEnumerable();

    private readonly List<Block> blocks = new List<Block>();
    private readonly Block[,] map = new Block[mapHeight, mapWidth];

    public void AddBlock(Block block)
    {
        blocks.Add(block);
    }

    private void UpdateMap()
    {
        var _map = Map;
    }
}
