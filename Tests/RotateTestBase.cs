using DesktopTetris;

namespace Tests;

public class RotateTestBase : TestBase
{
    [Test]
    public void TestRotateAround()
    {
        foreach (var matrice in blockTypes.Values)
        {
            var block = new Block(matrice);

            for (int i = 0; i < 4; i++)
            {
                block.RotateRight();
            }
            Assert.That(block.Matrice, Is.EqualTo(matrice));
        }
    }

    [Test]
    public void TestRotateOnce()
    {
        foreach (var matrice in blockTypes.Values)
        {
            var block = new Block(matrice);

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

    [Test]
    public void TestNotRotateAgainstWall()
    {
        var block = AddNewBlock(blockTypes[BlockType.I]);
        var anchor = (9, 0);
        block.RotateRight();
        block.MoveTo(anchor);

        var matrice = block.Matrice;
        block.RotateRight();
        Assert.Multiple(() =>
        {
            Assert.That(block.Matrice, Is.EqualTo(matrice));
            Assert.That(block.GetAnchorPosition(), Is.EqualTo(anchor));
        });
    }
    
    [Test]
    public void TestNotRotateAgainstLowerBorder()
    {
        var block = AddNewBlock(blockTypes[BlockType.I], (5, 15));
        var matrice = block.Matrice;
        
        block.RotateRight();

        Assert.That(block.Matrice, Is.EqualTo(matrice));
    }
    
    [Test]
    public void TestRotateAboveMap()
    {
        var block = AddNewBlock(blockTypes[BlockType.I], (5, -5));

        block.RotateRight();

        Assert.That(block.Matrice.GetLength(0), Is.EqualTo(4));
    }
}
