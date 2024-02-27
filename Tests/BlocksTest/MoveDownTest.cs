namespace Tests.BlocksTest;

public class MoveDownTest : TestBase
{
    [Test]
    public void TestMoveDown()
    {
        var block = AddNewBlock(anchorPosition: (0, 0));
        block.MoveDown();
        Assert.That(block.Anchor.y, Is.EqualTo(1));
    }

    [Test]
    public void TestMoveDownAboveMap()
    {
        var block = AddNewBlock(anchorPosition: (0, -10));
        block.MoveDown();
        Assert.That(block.Anchor.y, Is.EqualTo(-9));
    }

    [Test]
    public void TestMoveDownAboveBottomBorder()
    {
        var block = AddNewBlock(anchorPosition: (0, 15));
        block.MoveDown();
        Assert.That(block.Anchor.y, Is.EqualTo(15));
    }

    [Test]
    public void TestMoveDownAboveBlock()
    {
        var block = AddNewBlock(blockTypes[BlockType.I], (0, 0));
        AddNewBlock(blockTypes[BlockType.I], (0, 1));
        
        block.MoveDown();
        Assert.That(block.Anchor.y, Is.EqualTo(0));
    }
    
    [Test]
    public void TestMoveDownNextToBlock()
    {
        var block = AddNewBlock(blockTypes[BlockType.Square], (0, 0));
        AddNewBlock(blockTypes[BlockType.I], (2, 0));
        
        block.MoveDown();
        Assert.That(block.Anchor.y, Is.EqualTo(1));
    }
}
