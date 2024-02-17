using Gdk;

namespace DesktopTetris;

public class Block
{
    private readonly List<bool[,]> blockTypes = new List<bool[,]>{
        new[,]{
            {false, false, true},
            {true, true, true}
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
            {true, true, false}
        },
        new[,]{
            {true, true, false},
            {false, true, true}
        },
        new[,]{
            {true, false, false},
            {true, true, true}
        }
    };

    private readonly int[] anchorPosition; // upper left corner
    public Color Color { get; private set; }
    public bool[,] Matrice { get; private set; }
    // ReSharper disable once MemberCanBePrivate.Global
    public bool AlreadyFallen { get; private set; }

    private readonly List<Color> colors = new List<Color>{
        new Color(3, 65, 174),
        new Color(114, 203, 59),
        new Color(255, 213, 0),
        new Color(255, 151, 28),
        new Color(255, 50, 19)
    };

    public Block(bool[,]? matrice = null, (int x, int y)? anchorPosition = null, bool alreadyFallen = false, Color? color = null)
    {
        Matrice = matrice ?? blockTypes[new Random().Next(blockTypes.Count)];
        this.anchorPosition = anchorPosition == null ? new[]{5, -Matrice.GetLength(0)} : new[]{anchorPosition.Value.x, anchorPosition.Value.y};
        Color = color ?? colors[new Random().Next(colors.Count)];
        
        AlreadyFallen = alreadyFallen;
        if (!alreadyFallen)
            HookEvents();
    }
    
    public (int x, int y) GetMapRelativePosition(int x, int y) => (anchorPosition[0] + x, anchorPosition[1] + y);

    public (int x, int y) GetAnchorPosition() => (anchorPosition[0], anchorPosition[1]);

    public void MoveTo((int x, int y) pos)
    {
        anchorPosition[0] = pos.x;
        anchorPosition[1] = pos.y;
    }

    private void OnGameEnded(object? sender, EventArgs e)
    {
        Color = new Color(128, 128, 128);
        
        DisableInput();
        Game.currentGame.GameEnded -= OnGameEnded;
    }

    private void OnRotate(object? sender, EventArgs e) => RotateRight();

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

    private void DisableInput()
    {
        Controls.DownPress -= OnMoveDown;
        Controls.LeftPress -= OnMoveLeft;
        Controls.RightPress -= OnMoveRight;
        Controls.RotatePress -= OnRotate;
    }

    public void RotateRight()
    {
        var originalMatrice = Matrice; // uložení originální matice do paměti
        var newMatrice = new bool[Matrice.GetLength(1), Matrice.GetLength(0)]; // nová matice s prohozenými dimenzemi
        for (int x = 0; x < Matrice.GetLength(1); x++)
        for (int y = 0; y < Matrice.GetLength(0); y++)
        {
            newMatrice[x, Matrice.GetLength(0) - 1 - y] = Matrice[y, x]; // přepsání matice podle vzorečku
        }

        Matrice = newMatrice; // nahrazení matice

        if (IsOutOfBorders() || Collided()) // pokud je nové otočená matice mimo hru nebo v jiném bloku, vrátí změny
            Matrice = originalMatrice;
    }

    public bool MoveDown()
    {
        anchorPosition[1] += 1;

        if (IsAtBottom() || Collided())
        {
            anchorPosition[1] -= 1;

            // blok již jednou spadnul, ochrana proti
            // nekonečnému tvoření nových bloků
            if (!AlreadyFallen)
            {
                AlreadyFallen = true;
                DisableInput(); // vypne ovládání bloku
                Game.currentGame.canSpawnNewBlock = true;
            }

            return false;
        }

        Game.currentGame.UpdateMap();
        return true;
    }

    public void Move(int xOffset)
    {
        anchorPosition[0] += xOffset;
        if (IsOutOfBorders() || Collided())
            anchorPosition[0] -= xOffset;
    }

    private bool IsOutOfBorders()
    {
        for (int x = 0; x < Matrice.GetLength(1); x++)
        for (int y = 0; y < Matrice.GetLength(0); y++)
        {
            if (!Matrice[y, x])
                continue;

            if (anchorPosition[0] + x is < 0 or >= Game.mapWidth || anchorPosition[1] + y >= Game.mapHeight)
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

    private bool Collided()
    {
        var map = Game.currentGame.map.map;
        for (int y = 0; y < Matrice.GetLength(0); y++)
        {
            for (int x = 0; x < Matrice.GetLength(1); x++)
            {
                var pos = GetMapRelativePosition(x, y);
                if (!Matrice[y, x] || IsOutOfBorders() || pos.y < 0)
                    continue;

                if (map[pos.y, pos.x].Count >= 2 && Matrice[y, x])
                    return true;
            }
        }

        return false;
    }
}
