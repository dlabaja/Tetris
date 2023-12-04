namespace Tetris;

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
        new[,]
        {
            { false, false, true },
            { true, true, true },
            { false, false, false }
        },
    };

    public int[] anchorPosition = new int[2]; // upper left corner
    public short color;
    private readonly bool[,] matrice;

    public Block()
    {
        matrice = blockTypes[new Random().Next(blockTypes.Count)];
        color = (short)new Random().Next(1, 8);
    }
    
    public bool[,] GetMatrice() => matrice;

    public void RotateLeft()
    {
        var newMatrix = new bool[matrice.GetLength(0), matrice.GetLength(1)];
        for (int x = 0; x < matrice.GetLength(0); x++)
        {
            for (int y = 0; y < matrice.GetLength(1); y++)
            {
                newMatrix[y, matrice.GetLength(0) - 1 - x] = matrice[x, y];
            }
        }
    }
    
    public void RotateRight()
    {
        var newMatrix = new bool[3, 3];
        for (int x = 0; x < matrice.GetLength(0); x++)
        {
            for (int y = 0; y < matrice.GetLength(1); y++)
            {
                newMatrix[x, matrice.GetLength(0) - 1 - y] = matrice[x, y];
            }
        }
    }

    public void MoveDown()
    {
        
    }

    public void MoveRight()
    {
        
    }

    public void MoveLeft()
    {
        
    }
}
