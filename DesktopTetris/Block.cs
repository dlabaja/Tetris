using Gdk;

namespace DesktopTetris;

public class Block
{
    private readonly List<bool[,]> blockTypes = new List<bool[,]>{
        new[,]{
            {false, false, true},
            {true, true, true},
        },
        new[,]{
            {true, true, true, true}
        },
        new[,]{
            {true, true},
            {true, true}
        },
        new[,]{
            {false, true, true},
            {true, true, false},
        },
        new[,]{
            {true, true, false},
            {false, true, true},
        },
        new[,]{
            {true, false, false},
            {true, true, true},
        },
    };

    public int[] AnchorPosition; // upper left corner
    public Color Color { get; protected set; }
    public bool[,] Matrice { get; protected set; }
    public bool alreadyFallen;

    private readonly List<Color> colors = new List<Color>{
        new Color(3, 65, 174),
        new Color(114, 203, 59),
        new Color(255, 213, 0),
        new Color(255, 151, 28),
        new Color(255, 50, 19)
    };

    public (int x, int y) GetMapRelativePosition(int x, int y)
    {
        return (AnchorPosition[0] + x, AnchorPosition[1] + y);
    }

    public Block()
    {
        Matrice = blockTypes[new Random().Next(blockTypes.Count)];
        AnchorPosition = new[]{(int)Math.Round((double)Game.mapWidth / 2), 0}; //-Matrice.GetLength(0)
        Color = colors[new Random().Next(colors.Count)];

        HookEvents();
    }

    private void OnGameEnded(object? sender, EventArgs e)
    {
        Color = new Color(128, 128, 128);
        
        DisableInput();
        Game.currentGame.GameEnded -= OnGameEnded;
    }

    private void OnRotate(object? sender, EventArgs e) => Rotate();

    private void OnMoveDown(object? sender, EventArgs e) => MoveDown();

    private void OnMoveLeft(object? sender, EventArgs e) => Move(-1);

    private void OnMoveRight(object? sender, EventArgs e) => Move(1);

    private void HookEvents()
    {
        Controls.DownPress += OnMoveDown;
        Controls.LeftPress += OnMoveLeft;
        Controls.RightPress += OnMoveRight;
        Controls.RotatePress += OnRotate;
        Game.currentGame.GameEnded += OnGameEnded;
    }

    protected void DisableInput()
    {
        Controls.DownPress -= OnMoveDown;
        Controls.LeftPress -= OnMoveLeft;
        Controls.RightPress -= OnMoveRight;
        Controls.RotatePress -= OnRotate;
    }

    public bool ContainsMapRelativePosition(int xPos, int yPos)
    {
        for (int x = 0; x < Matrice.GetLength(1); x++)
        for (int y = 0; y < Matrice.GetLength(0); y++)
        {
            var pos = GetMapRelativePosition(x, y);
            if (pos.x == xPos && pos.y == yPos)
            {
                return true;
            }
        }

        return false;
    }

    private void Rotate()
    {
        var oldMatrice = Matrice;
        var newMatrice = new bool[Matrice.GetLength(1), Matrice.GetLength(0)];
        for (int x = 0; x < Matrice.GetLength(1); x++)
        for (int y = 0; y < Matrice.GetLength(0); y++)
        {
            newMatrice[x, Matrice.GetLength(0) - 1 - y] = Matrice[y, x];
        }

        Matrice = newMatrice;

        if (IsOutOfBorders() || Collided())
            Matrice = oldMatrice;
    }

    public void MoveDown()
    {
        AnchorPosition[1] += 1;
        
        if (IsAtBottom() || Collided())
        {
            AnchorPosition[1] -= 1;

            if (!alreadyFallen)
            {
                alreadyFallen = true;
                DisableInput();
                Game.currentGame.canSpawnNewBlock = true;
            }
        }
    }

    private void Move(int xOffset)
    {
        AnchorPosition[0] += xOffset;
        if (IsOutOfBorders() || Collided())
            AnchorPosition[0] -= xOffset;
    }

    private bool IsOutOfBorders()
    {
        for (int x = 0; x < Matrice.GetLength(1); x++)
        for (int y = 0; y < Matrice.GetLength(0); y++)
        {
            if (!Matrice[y, x])
                continue;

            if (AnchorPosition[0] + x is < 0 or >= Game.mapWidth || AnchorPosition[1] + y >= Game.mapHeight)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsAtBottom()
    {
        var lowestY = 0;
        for (int y = 0; y < Matrice.GetLength(0); y++)
        {
            for (int x = 0; x < Matrice.GetLength(1); x++)
            {
                if (Matrice[y, x])
                {
                    lowestY = GetMapRelativePosition(x, y).y;
                }
            }
        }

        return lowestY >= Game.mapHeight;
    }

    private bool CollidedFromBottom()
    {
        var fallenBlocksMap = Game.currentGame.fallenBlocksMap;
        for (int y = 0; y < Matrice.GetLength(0); y++)
        {
            for (int x = 0; x < Matrice.GetLength(1); x++)
            {
                var pos = GetMapRelativePosition(x, y);
                if (pos.y < 0 || !Matrice[y, x])
                    continue;

                if (pos.y + 1 >= Game.mapHeight)
                    return true;

                if (fallenBlocksMap[pos.y + 1, pos.x])
                    return true;
            }
        }

        return false;
    }

    private bool Collided()
    {
        var map = Game.currentGame.fallenBlocksMap;
        var gameBlocks = Game.currentGame.Blocks;
        for (int y = 0; y < Matrice.GetLength(0); y++)
        {
            for (int x = 0; x < Matrice.GetLength(1); x++)
            {
                var pos = GetMapRelativePosition(x, y);
                if (pos.y < 0 || !Matrice[y, x] || IsOutOfBorders())
                    continue;

                if (map[pos.y, pos.x] && Matrice[y, x] && gameBlocks.FirstOrDefault(l => l.ContainsMapRelativePosition(pos.x, pos.y)) != this)
                    return true;
            }
        }

        return false;
    }
}
