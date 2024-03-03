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
    private List<int> latestRemovedRows = new List<int>();
    private bool canSplitBlocks;
    public event EventHandler GameEndedEvent;
    public event EventHandler ScoreChanged;
    
    public int Score;
    private readonly Dictionary<int, int> rowsAndScores = new Dictionary<int, int>{
        {1, 40},
        {2, 100},
        {3, 300},
        {4, 400}
    };

    public Game()
    {
        currentGame = this;

        nextTurnTimer = new Timer(400);
        nextTurnTimer.AutoReset = true;
        nextTurnTimer.Elapsed += OnNextTurn;
        nextTurnTimer.Start();
        
        GameEndedEvent += OnGameEnded;
    }

    public void EndGame()
    {
        GameEndedEvent?.Invoke(this, EventArgs.Empty);
    }

    private void OnGameEnded(object? sender, EventArgs e)
    {
        nextTurnTimer.Elapsed -= OnNextTurn;
        nextTurnTimer.Stop();
    }

    private void OnNextTurn(object? sender, ElapsedEventArgs e) => NextTurn();

    public void NextTurn()
    {
        if (latestRemovedRows.Any())
        {
            ShiftAllBlocksDown(latestRemovedRows.Max());
            latestRemovedRows.Remove(latestRemovedRows.Max());
        }

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

        if (!canSplitBlocks)
            return;

        if (map.GetFilledRowsIndexes().Any())
            AddScore(map.GetFilledRowsIndexes().Count);
        
        foreach (var row in map.GetFilledRowsIndexes())
        {
            SplitBlocks(row);
            latestRemovedRows.Add(row);
        }

        canSplitBlocks = false;
    }
    
    public bool GameEnded()
    {
        for (int x = 0; x < Map.GetLength(1); x++)
            if (Map[0, x].Count > 1)
                return true;

        return false;
    }
    
    private void AddScore(int rowCount)
    {
        Score += rowsAndScores[rowCount];
        ScoreChanged?.Invoke(this, EventArgs.Empty);
    }

    private void ShiftAllBlocksDown(int row)
    {
        var _blocks = new List<Block>();
        var _map = Map;
        for (int y = 0; y < _map.GetLength(0); y++)
        {
            for (int x = 0; x < _map.GetLength(1); x++)
            {
                if (_map[y, x].Any() && y < row && !_blocks.Contains(_map[y, x][0]))
                {
                    _blocks.Add(_map[y, x][0]);
                }
            }
        }

        foreach (var block in _blocks)
        {
            block.MoveTo((block.Anchor.x, block.Anchor.y + 1));
        }
    }

    private void OnBlockHitLowerBorder(object? sender, EventArgs e)
    {
        AddBlock(new Block());
        canSplitBlocks = true;
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
