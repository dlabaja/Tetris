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
            {true, true, true, true},
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

    private readonly int[] AnchorPosition; // upper left corner
    public Color Color { get; private set; }
    public bool[,] Matrice { get; private set; }

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
        AnchorPosition = new[]{(int)Math.Round((double)Game.mapWidth / 2), -Matrice.GetLength(0)};
        Color = colors[new Random().Next(colors.Count)];

        HookEvents();
    }

    private void OnGameEnded(object? sender, EventArgs e)
    {
        UnhookEvents();
        Color = new Color(128, 128, 128);
    }

    private void OnRotate(object? sender, EventArgs e) => Rotate();

    private void OnMoveDown(object? sender, EventArgs e) => Move(0, 1);

    private void OnMoveLeft(object? sender, EventArgs e) => Move(-1, 0);

    private void OnMoveRight(object? sender, EventArgs e) => Move(1, 0);

    private void HookEvents()
    {
        Controls.DownPress += OnMoveDown;
        Controls.LeftPress += OnMoveLeft;
        Controls.RightPress += OnMoveRight;
        Controls.RotatePress += OnRotate;
        Game.currentGame.GameEnded += OnGameEnded;
    }

    private void UnhookEvents()
    {
        Controls.DownPress -= OnMoveDown;
        Controls.LeftPress -= OnMoveLeft;
        Controls.RightPress -= OnMoveRight;
        Controls.RotatePress -= OnRotate;
        Game.currentGame.GameEnded -= OnGameEnded;
    }

    private void Rotate()
    {
        var newMatrice = new bool[Matrice.GetLength(1), Matrice.GetLength(0)];
        for (int x = 0; x < Matrice.GetLength(1); x++)
        for (int y = 0; y < Matrice.GetLength(0); y++)
        {
            newMatrice[x, Matrice.GetLength(0) - 1 - y] = Matrice[y, x];
        }

        Matrice = newMatrice;
    }

    public void Move(int xOffset, int yOffset)
    {
        if (IsOutOfBorders(xOffset, yOffset))
            return;

        AnchorPosition[0] += xOffset;
        AnchorPosition[1] += yOffset;

        if (IsAtBottom() || Collided())
        {
            UnhookEvents();
            Game.currentGame.SpawnNewBlock();
        }
        
        Game.currentGame.RegenMap();
    }

    private bool IsOutOfBorders(int xOffset, int yOffset)
    {
        for (int x = 0; x < Matrice.GetLength(1); x++)
        for (int y = 0; y < Matrice.GetLength(0); y++)
        {
            if (!Matrice[y, x])
                continue;

            if (AnchorPosition[0] + xOffset + x is < 0 or >= Game.mapWidth || AnchorPosition[1] + yOffset + y >= Game.mapHeight)
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

        return lowestY > Game.mapHeight;
    }

    private bool Collided()
    {
        var fallenBlocksMap = Game.currentGame.fallenBlocksMap;
        for (int y = 0; y < Matrice.GetLength(0); y++)
        {
            for (int x = 0; x < Matrice.GetLength(1); x++)
            {
                var pos = GetMapRelativePosition(x, y);
                if (pos.y < 0)
                    continue;

                if (pos.y + 1 >= Game.mapHeight)
                    return true;

                if (Matrice[y, x] && fallenBlocksMap[pos.y + 1, pos.x])
                    return true;
            }
        }

        return false;
    }
}
