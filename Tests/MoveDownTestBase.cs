namespace Tests;

public class MoveDownTestBase : TestBase
{
    [Test]
    public void TestMoveDown()
    {
        var block = AddNewBlock();
        var anchor = block.GetAnchorPosition();
        block.MoveDown();

        Assert.That(block.GetAnchorPosition(), Is.EqualTo((anchor.x, anchor.y + 1)));
    }

    [Test]
    public void TestSpawnPosition()
    {
        var block = AddNewBlock();
        Assert.That(block.GetAnchorPosition(), Is.EqualTo((5, -block.Matrice.GetLength(0))));
    }

    [Test]
    public void TestCollideWithBorder()
    {
        var anchor = (0, 15);
        var block = AddNewBlock(anchorPosition: anchor);
        block.MoveDown();
        Assert.Multiple(() =>
        {
            Assert.That(block.GetAnchorPosition(), Is.EqualTo(anchor));
            Assert.That(block.AlreadyFallen, Is.EqualTo(true));
        });
    }

    [Test]
    public void TestMoveDownCollision()
    {
        AddNewBlock(blockTypes[BlockType.I], (0, 15));
        var block = AddNewBlock(blockTypes[BlockType.I], (0, 14));

        var anchor = block.GetAnchorPosition();
        block.MoveDown();
        Assert.That(block.GetAnchorPosition(), Is.EqualTo(anchor));
    }

    [Test]
    public void TestMoveDownAboveMap()
    {
        var block = AddNewBlock(blockTypes[BlockType.I], (0, -5));
        
        block.MoveDown();
        
        Assert.That(block.GetAnchorPosition(), Is.EqualTo((0, -4)));
    }
    
    [Test]
    public void TestMoveDownNextToABlock()
    {
        var block = AddNewBlock(blockTypes[BlockType.I], (2, 14));
        AddNewBlock(blockTypes[BlockType.Square], (0, 14));
        AddNewBlock(blockTypes[BlockType.Square], (6, 14));
        
        block.MoveDown();
        
        Assert.That(block.GetAnchorPosition(), Is.EqualTo((2, 15)));
    }
}
