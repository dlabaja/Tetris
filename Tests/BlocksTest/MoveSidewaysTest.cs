namespace Tests.BlocksTest;

public class MoveSidewaysTest : TestBase
{
    [Test]
    public void TestMoveSideways()
    {
        var block = AddNewBlock(anchorPosition: (0, 0));
        block.MoveSideways(1);
        Assert.That(block.Anchor.x, Is.EqualTo(1));
        
        block.MoveSideways(-1);
        block.MoveSideways(-1);
        Assert.That(block.Anchor.x, Is.EqualTo(0));
    }
    
    [Test]
    public void TestMoveSidewaysAboveMap()
    {
        var block = AddNewBlock(anchorPosition: (5, -10));
        block.MoveSideways(-1);
        Assert.That(block.Anchor.x, Is.EqualTo(4));
    }

    [Test]
    public void TestMoveThroughBorder()
    {
        var block = AddNewBlock(anchorPosition: (0, 0));
        block.MoveSideways(-1);
        Assert.That(block.Anchor.x, Is.EqualTo(0));
    }
    
    [Test]
    public void TestMoveSidewaysAboveBottomBorder()
    {
        var block = AddNewBlock(anchorPosition: (0, 15));
        block.MoveSideways(1);
        Assert.That(block.Anchor.y, Is.EqualTo(15));
    }
    
    [Test]
    public void TestMoveSidewaysAboveBlock()
    {
        var block = AddNewBlock(blockTypes[BlockType.I], (0, 0));
        AddNewBlock(blockTypes[BlockType.I], (0, 1));
        
        block.MoveSideways(1);
        Assert.That(block.Anchor.x, Is.EqualTo(1));
    }
    
    [Test]
    public void TestMoveSidewaysThroughToBlock()
    {
        var block = AddNewBlock(blockTypes[BlockType.Square], (0, 0));
        AddNewBlock(blockTypes[BlockType.I], (2, 0));
        
        block.MoveSideways(1);
        Assert.That(block.Anchor.x, Is.EqualTo(0));
    }
}
