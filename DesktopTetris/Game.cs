using DesktopTetris.GtkWindows;
using System.Diagnostics;
using System.Text;
using System.Timers;
using Timer = System.Timers.Timer;

namespace DesktopTetris;

public class Game
{
    public static Game currentGame = null!;
    public List<Block> Blocks { get; set; } = new List<Block>();
    private int GameTime { get; set; }
    public int Level { get; private set; } = 1;
    private int Score { get; set; }

    public bool[,] fallenBlocksMap = new bool[mapHeight, mapWidth];
    public const int mapWidth = 10;
    public const int mapHeight = 16;
    public event EventHandler GameEnded;
    public bool canSpawnNewBlock;

    private Timer moveDownTimer;

    public Game()
    {
        currentGame = this;
        InitTimers();
        Blocks.Add(new Block());

        GameEnded += OnGameEnded;
    }

    private void InitTimers()
    {
        var gameTimer = new Timer(1000);
        gameTimer.Elapsed += (_, _) => GameTime++;
        gameTimer.Start();

        moveDownTimer = new Timer(400);
        moveDownTimer.Elapsed += OnMoveDownTimerElapsed;
        moveDownTimer.Start();
    }

    private void OnMoveDownTimerElapsed(object? o, ElapsedEventArgs elapsedEventArgs)
    {
        UpdateMap();
        RemoveFilledParts();
        UpdateMap();

        var countedBlocks = new List<Block>();
        for (int i = 0; i < Blocks.Count; i++)
        {
            var allFallen = true;
            foreach (var block in Blocks.Where(x => !countedBlocks.Contains(x)))
            {
                if (block.MoveDown())
                {
                    UpdateMap();
                    countedBlocks.Add(block);
                    allFallen = false;
                }
            }

            if (allFallen)
                break;
        }

        if (canSpawnNewBlock)
        {
            canSpawnNewBlock = false;
            SpawnNewBlock();
        }
    }

    private void OnGameEnded(object? sender, EventArgs e)
    {
        moveDownTimer.Stop();
    }

    private bool NoRoomForNewBlock()
    {
        for (int x = 0; x < mapWidth; x++)
            if (fallenBlocksMap[0, x])
                return true;

        return false;
    }

    private void SpawnNewBlock()
    {
        Score++;
        WindowManager.mainWindow.ChangeScore(Score);

        if (NoRoomForNewBlock())
        {
            GameEnded.Invoke(this, EventArgs.Empty);
            return;
        }
        
        Blocks.Add(new Block());
    }

    private void PrintMap()
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

    private List<int> GetFilledRowsIndexes()
    {
        var filledRows = new List<int>();
        for (int y = 0; y < fallenBlocksMap.GetLength(0); y++)
        {
            var filled = true;
            for (int x = 0; x < fallenBlocksMap.GetLength(1); x++)
            {
                if (fallenBlocksMap[y, x])
                    continue;

                filled = false;
                break;
            }

            if (filled)
            {
                filledRows.Add(y);
            }
        }

        return filledRows;
    }

    private void RemoveFilledParts()
    {
        var affectedBlocks = new List<Block>();
        var rows = GetFilledRowsIndexes();
        
        if (!rows.Any())
            return;

        foreach (var block in Blocks)
        {
            for (int y = 0; y < block.Matrice.GetLength(0); y++)
            {
                if (!rows.Contains(block.GetMapRelativePosition(0, y).y))
                    continue;

                for (int x = 0; x < block.Matrice.GetLength(1); x++)
                    block.Matrice[y, x] = false;

                if (!affectedBlocks.Contains(block))
                    affectedBlocks.Add(block);
            }
        }

        foreach (var block in affectedBlocks)
        {
            SplitBlocks(block, rows);
            /*if (IsMatriceEmpty(block.Matrice))
            {
                Blocks.Remove(block);
            }*/
        }
    }
    
    private void SplitBlocks(Block block, ICollection<int> rows)
    {
        var newMatrice = new bool[block.Matrice.GetLength(0), block.Matrice.GetLength(1)];
        var lowestY = 0;
        for (int y = 0; y < block.Matrice.GetLength(0); y++)
        {
            if (rows.Contains(block.GetMapRelativePosition(0, y).y) || y == block.Matrice.GetLength(0) - 1)
            {
                if (!IsMatriceEmpty(newMatrice))
                {
                    Blocks.Add(new Block(newMatrice, block.Color, block.GetMapRelativePosition(0, lowestY)));   
                }
                lowestY = y + 1;
                newMatrice = new bool[block.Matrice.GetLength(0), block.Matrice.GetLength(1)];
                continue;
            }

            for (int x = 0; x < block.Matrice.GetLength(1); x++)
                if (block.Matrice[y, x])
                    newMatrice[y, x] = true;
        }

        Blocks.Remove(block);
    }

    private static bool IsMatriceEmpty(bool[,] matrice)
    {
        for (int x = 0; x < matrice.GetLength(1); x++)
        for (int y = 0; y < matrice.GetLength(0); y++)
            if (matrice[y, x])
                return false;
        return true;
    }

    public void UpdateMap()
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
