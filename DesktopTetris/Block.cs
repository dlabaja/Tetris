using Gdk;

namespace DesktopTetris;

public class Block
{
    private List<bool[,]> blockTypes = new List<bool[,]>{
        new[,]
        {
            { false, false, true },
            { true, true, true },
            { false, false, false }
        },
        new[,]
        {
            { false, false, false, false },
            { true, true, true, true },
            { false, false, false, false },
            { false, false, false, false }
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
            { false, false, false }
        },
        new[,]
        {
            { true, true, false },
            { false, true, true },
            { false, false, false }
        },
        new[,]
        {
            { true, false, false },
            { true, true, true },
            { false, false, false }
        },
    };
    
    public enum Rotation
    {
        Left,
        Right
    }

    public readonly int[] AnchorPosition; // upper left corner
    public Color Color { get; private set; }
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
        AnchorPosition = new[]{5, -Matrice.GetLength(0)};
        Color = colors[new Random().Next(colors.Count)];
    }

    public void Rotate(Rotation rotation)
    {
        var newMatrice = new bool[Matrice.GetLength(0), Matrice.GetLength(1)];
        for (int x = 0; x < Matrice.GetLength(1); x++)
        {
            for (int y = 0; y < Matrice.GetLength(0); y++)
            {
                if (rotation == Rotation.Left)
                {
                    newMatrice[y, Matrice.GetLength(1) - 1 - x] = Matrice[x, y];
                    continue;
                }
                newMatrice[x, Matrice.GetLength(0) - 1 - y] = Matrice[x, y];
            }
        }

        Matrice = newMatrice;
    }

    public void Move(int xOffset, int yOffset)
    {
        AnchorPosition[0] += xOffset;
        AnchorPosition[1] += yOffset;
    }
}
