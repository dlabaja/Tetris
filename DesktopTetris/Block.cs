using Gdk;
using System.Diagnostics;

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

    public bool[,] Matrice => (bool[,])matrice.Clone();
    public (int x, int y) Anchor
    {
        get => (anchor[0], anchor[1]);
    }
    public Color Color { get; }

    private bool[,] matrice;
    private readonly int[] anchor; // upper left corner
    private bool canSpawnNewBlock;
    private Game game;
    
    public event EventHandler BlockHitLowerBorder;

    private readonly List<Color> colors = new List<Color>{
        new Color(3, 65, 174),
        new Color(114, 203, 59),
        new Color(255, 213, 0),
        new Color(255, 151, 28),
        new Color(255, 50, 19)
    };

    public Block(bool[,]? matrice = null, (int x, int y)? anchor = null, bool canSpawnNewBlock = true, Color? color = null)
    {
        game = Game.currentGame;
        this.matrice = matrice ?? blockTypes[new Random().Next(blockTypes.Count)];
        this.anchor = anchor == null ? new[]{5, -this.matrice.GetLength(0)} : new[]{anchor.Value.x, anchor.Value.y};
        this.canSpawnNewBlock = canSpawnNewBlock;
        Color = color ?? colors[new Random().Next(colors.Count)];
    }

    public (int x, int y) GetMapRelativePosition(int x, int y) => (anchor[0] + x, anchor[1] + y);

    public (int x, int y) GetAnchorPosition() => (anchor[0], anchor[1]);

    public void MoveTo((int x, int y) pos)
    {
        anchor[0] = pos.x;
        anchor[1] = pos.y;
        
        game.UpdateMap(this);
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

        var _matrice = matrice;
        matrice = newMatrice; // nahrazení matice
        game.UpdateMap(this);

        if (CollidesWithSideBorder() || CollidesWithBottomBorder() || game.CollisionDetected())
        {
            matrice = _matrice;
            return;
        }
        
        game.UpdateMap(this);
    }

    private bool CollidesWithSideBorder()
    {
        for (int y = 0; y < matrice.GetLength(0); y++)
        for (int x = 0; x < matrice.GetLength(1); x++)
        {
            var pos = GetMapRelativePosition(x, y);
            if (!Utils.InMapXRange(pos))
                return true;
        }

        return false;
    }
    
    private bool CollidesWithBottomBorder()
    {
        for (int y = 0; y < matrice.GetLength(0); y++)
        for (int x = 0; x < matrice.GetLength(1); x++)
            if (GetMapRelativePosition(x, y).y >= Game.mapHeight)
                return true;

        return false;
    }

    public void MoveDown()
    {
        anchor[1]++;
        game.UpdateMap(this);

        if (CollidesWithBottomBorder() || game.CollisionDetected())
        {
            if (CollidesWithBottomBorder() && canSpawnNewBlock)
            {
                anchor[1]--;
                game.UpdateMap(this);
                
                canSpawnNewBlock = false;
                BlockHitLowerBorder(this, EventArgs.Empty);
                return;
            }
            
            anchor[1]--;
        }
        
        game.UpdateMap(this);
    }

    public void MoveSideways(int xOffset)
    {
        anchor[0] += xOffset;
        game.UpdateMap(this);

        if (CollidesWithSideBorder() || game.CollisionDetected())
        {
            anchor[0] -= xOffset;
        }

        game.UpdateMap(this);
    }
    
    public IEnumerable<bool> MatriceToList()
    {
        var flattenedList = new List<bool>();

        for (int y = 0; y < Matrice.GetLength(0); y++)
        for (int x = 0; x < Matrice.GetLength(1); x++)
            flattenedList.Add(Matrice[y, x]);

        return flattenedList;
    }
}
