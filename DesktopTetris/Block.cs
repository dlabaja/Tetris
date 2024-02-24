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

    private readonly List<Color> colors = new List<Color>{
        new Color(3, 65, 174),
        new Color(114, 203, 59),
        new Color(255, 213, 0),
        new Color(255, 151, 28),
        new Color(255, 50, 19)
    };

    public Block(bool[,]? matrice = null, (int x, int y)? anchorPosition = null, Color? color = null)
    {
        Matrice = matrice ?? blockTypes[new Random().Next(blockTypes.Count)];
        this.anchorPosition = anchorPosition == null ? new[]{5, -Matrice.GetLength(0)} : new[]{anchorPosition.Value.x, anchorPosition.Value.y};
        Color = color ?? colors[new Random().Next(colors.Count)];
    }
    
    public (int x, int y) GetMapRelativePosition(int x, int y) => (anchorPosition[0] + x, anchorPosition[1] + y);

    public (int x, int y) GetAnchorPosition() => (anchorPosition[0], anchorPosition[1]);

    public void MoveTo((int x, int y) pos)
    {
        anchorPosition[0] = pos.x;
        anchorPosition[1] = pos.y;
    }

    private void OnRotate(object? sender, EventArgs e) => RotateRight();

    private void HookEvents()
    {
        Controls.RotatePress += OnRotate;
    }

    private void DisableInput()
    {
        Controls.RotatePress -= OnRotate;
    }

    public void RotateRight()
    {
        var newMatrice = new bool[Matrice.GetLength(1), Matrice.GetLength(0)]; // nová matice s prohozenými dimenzemi
        for (int x = 0; x < Matrice.GetLength(1); x++)
        for (int y = 0; y < Matrice.GetLength(0); y++)
        {
            newMatrice[x, Matrice.GetLength(0) - 1 - y] = Matrice[y, x]; // přepsání matice podle vzorečku
        }

        Matrice = newMatrice; // nahrazení matice
    }
}
