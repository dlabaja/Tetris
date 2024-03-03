using DesktopTetris.GtkWindows;
using System.Diagnostics;
using System.Timers;
using Timer = System.Timers.Timer;

namespace DesktopTetris;

public class Game
{
    public const int mapHeight = 16;
    public const int mapWidth = 10;

    public static Game currentGame = null!;

    public List<Block>[,] Map => (List<Block>[,])map.map.Clone();
    public IEnumerable<Block> Blocks => blocks.AsEnumerable();

    private readonly List<Block> blocks = new List<Block>();
    private readonly BlocksMap map = new BlocksMap();

    private readonly Timer nextTurnTimer;

    public Game()
    {
        currentGame = this;

        nextTurnTimer = new Timer();
        nextTurnTimer.AutoReset = true;
        nextTurnTimer.Elapsed += OnNextTurn;
        // todo zapnout po debugingu
        // nextTurnTimer.Start();
    }

    private void OnNextTurn(object? sender, ElapsedEventArgs e) => NextTurn();

    public void NextTurn()
    {
        var _blocks = blocks.ToList();
        for (int i = 0; i < blocks.Count; i++)
        {
            foreach (var block in _blocks.ToList())
            {
                block.MoveDown();
                _blocks.Remove(block);
            }

            if (!_blocks.Any())
                break;
        }
        
        foreach (var row in map.GetFilledRowsIndexes())
            SplitBlocks(row);
    }

    private void OnBlockHitLowerBorder(object? sender, EventArgs e)
    {
        AddBlock(new Block());
    }

    private IEnumerable<Block> GetBlocksForSplitting(int row)
    {
        var blockList = new List<Block>();
        for (int x = 0; x < Map.GetLength(1); x++)
        {
            if (!map.map[row, x].Any() || blockList.Contains(map.map[row, x][0]))
                continue;
            
            blockList.Add(map.map[row, x][0]);
        }

        return blockList;
    }

    private void SplitBlocks(int row)
    {
        foreach (var block in GetBlocksForSplitting(row))
        {
            var newMatrice = new bool[block.Matrice.GetLength(0), block.Matrice.GetLength(1)];
            var matriceNotEmpty = false;
            for (int y = 0; y < block.Matrice.GetLength(0); y++)
            {
                for (int x = 0; x < block.Matrice.GetLength(1); x++)
                {
                    if (!block.Matrice[y, x] || block.Anchor.y + y == row)
                        continue;
                    newMatrice[y, x] = true;
                    matriceNotEmpty = true;
                }

                if (block.Anchor.y + y != row && y != block.Matrice.GetLength(0) - 1 || !matriceNotEmpty)
                    continue;
                
                AddBlock(new Block(newMatrice, (block.Anchor.x, block.Anchor.y), false, block.Color));
                newMatrice = new bool[block.Matrice.GetLength(0), block.Matrice.GetLength(1)]; 
                matriceNotEmpty = false;
            }
            
            RemoveBlock(block);
        }
    }

    public void AddBlock(Block block)
    {
        blocks.Add(block);
        map.AddBlock(block);

        block.BlockHitLowerBorder += OnBlockHitLowerBorder;
    }

    public void RemoveBlock(Block block)
    {
        blocks.Remove(block);
        map.RemoveBlock(block);

        block.BlockHitLowerBorder -= OnBlockHitLowerBorder;
    }

    public void UpdateMap(Block block)
    {
        map.UpdateMap(block);
    }

    public bool CollisionDetected()
    {
        return map.CollisionDetected();
    }
}
