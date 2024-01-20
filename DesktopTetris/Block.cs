using Gdk;

namespace DesktopTetris;

public class Block
{
    private List<bool[,]> blockTypes = new List<bool[,]>{
        new[,]
        {
            { false, false, true },
            { true, true, true },
        },
        new[,]
        {
            { true, true, true, true },
        },
        new[,]
        {
            { true, true },
            { true, true }
        },
        new[,]
        {
            { false, true, true },
            { true, true, false },
        },
        new[,]
        {
            { true, true, false },
            { false, true, true },
        },
        new[,]
        {
            { true, false, false },
            { true, true, true },
        },
    };
    
    public enum Rotation
    {
        Left,
        Right
    }

    private readonly int[] AnchorPosition; // upper left corner
    public Color Color { get; set; }
    public bool[,] Matrice { get; private set; }

    private List<Color> colors = new List<Color>{
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
        AnchorPosition = new[]{5, 0};
        Color = colors[new Random().Next(colors.Count)];

        Controls.DownPress += OnMoveDown;
        Controls.LeftPress += OnMoveLeft;
        Controls.RightPress += OnMoveRight;
        Controls.RotatePress += OnRotate;
    }

    private void OnRotate(object? sender, EventArgs e) => Rotate();

    private void OnMoveDown(object? sender, EventArgs e) => Move(0, 1);

    private void OnMoveLeft(object? sender, EventArgs e) => Move(-1, 0);

    private void OnMoveRight(object? sender, EventArgs e) => Move(1, 0);

    public void UnhookEvents()
    {
        Controls.DownPress -= OnMoveDown;
        Controls.LeftPress -= OnMoveLeft;
        Controls.RightPress -= OnMoveRight;
        Controls.RotatePress -= OnRotate;
    }

    public void Rotate()
    {
        var newMatrice = new bool[Matrice.GetLength(1), Matrice.GetLength(0)];
        Console.WriteLine($"{Matrice.GetLength(1)}x{Matrice.GetLength(0)} -> {Matrice.GetLength(0)}x{Matrice.GetLength(1)}");
        for (int x = 0; x < Matrice.GetLength(1); x++)
        for (int y = 0; y < Matrice.GetLength(0); y++)
        {
            newMatrice[x, Matrice.GetLength(0) - 1 - y] = Matrice[y, x];
            Console.WriteLine($"{x},{y} -> {Matrice.GetLength(0) - 1 - y}, {x}");
        }

        Matrice = newMatrice;
        Program.currentGame.RegenMap();
    }

    public void Move(int xOffset, int yOffset)
    {
        if (IsOutOfBorders(xOffset, yOffset))
            return;

        AnchorPosition[0] += xOffset;
        AnchorPosition[1] += yOffset;
        Program.currentGame.RegenMap();
    }

    private bool IsOutOfBorders(int xOffset, int yOffset)
    {
        for (int x = 0; x < Matrice.GetLength(1); x++)
        for (int y = 0; y < Matrice.GetLength(0); y++)
        {
            if (!Matrice[y, x])
                continue;
            
            if (AnchorPosition[0] + xOffset + x is < 0 or >= 10 || AnchorPosition[1] + yOffset + y >= 16)
            {
                return true;
            }
        }

        return false;

    }
}
