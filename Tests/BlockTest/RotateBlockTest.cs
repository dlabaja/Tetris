using DesktopTetris;

namespace Tests.BlockTest;

public class Tests : BlockTest
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
    
    [Test]
    public void TestRotateBlock()
    {
        // test all types
        foreach (var matrice in blockTypes)
        {
            block = new Block(matrice, null);
            
            // rotate around
            for (int i = 0; i < 4; i++)
            {
                block.RotateRight();
            }
            Assert.That(block.Matrice, Is.EqualTo(matrice));
        
            // check if the block is correctly rotated
            block.RotateRight();
            for (int x = 0; x < matrice.GetLength(1); x++)
            {
                for (int y = 0; y < matrice.GetLength(0); y++)
                {
                    Assert.That(block.Matrice[x, matrice.GetLength(0) - 1 - y], Is.EqualTo(matrice[y, x]));
                }
            }
        }
    }
}
