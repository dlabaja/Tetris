namespace Tests;

public class MoveSidewaysTestBase : TestBase
{
    [Test]
    public void TestMoveSideways()
    {
        var block = AddNewBlock();
        var anchor = block.GetAnchorPosition();
        block.MoveSideways(-1);
        Assert.That(block.GetAnchorPosition(), Is.EqualTo((anchor.x - 1, anchor.y)));
    }
    
    [Test]
    public void TestMoveSidewaysAboveLowerBorder()
    {
        var block = AddNewBlock(blockTypes[BlockType.I], (6, 15));
        
        block.MoveSideways(1);
        Assert.That(block.GetAnchorPosition(), Is.EqualTo((7, 15)));
    }
    

    [Test]
    public void TestStopAtSideBorder()
    {
        var block = AddNewBlock(anchorPosition: (0, 0));
        var anchor = block.GetAnchorPosition();
        block.MoveSideways(-1);
        Assert.That(block.GetAnchorPosition(), Is.EqualTo(anchor));
    }

    [Test]
    public void TestCollision()
    {
        var anchor = (2, 14);
        var block = AddNewBlock(blockTypes[BlockType.I], anchor);
        AddNewBlock(blockTypes[BlockType.Square], (6, 14));
        
        block.MoveSideways(1);
        
        Assert.That(block.GetAnchorPosition(), Is.EqualTo(anchor));
    }

    [Test]
    public void TestMoveSidewaysAboveMap()
    {
        var block = AddNewBlock(blockTypes[BlockType.I], (6, -2));
        
        block.MoveSideways(1);
        Assert.That(block.GetAnchorPosition(), Is.EqualTo((7, -2)));
    }
}
