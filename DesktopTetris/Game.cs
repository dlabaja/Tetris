using DesktopTetris.GtkWindows;
using System.Timers;
using Timer = System.Timers.Timer;

namespace DesktopTetris;

public class Game
{
    public const int mapHeight = 16;
    public const int mapWidth = 10;

    public static readonly Game currentGame = null!;
    
    public List<Block>[,] Map => (List<Block>[,])map.map.Clone();
    public IEnumerable<Block> Blocks => blocks.AsEnumerable();

    private readonly List<Block> blocks = new List<Block>();
    private readonly BlocksMap map = new BlocksMap();

    public void AddBlock(Block block)
    {
        blocks.Add(block);
        map.AddBlock(block);
    }

    public void RemoveBlock(Block block)
    {
        blocks.Remove(block);
        map.RemoveBlock(block);
    }
}
