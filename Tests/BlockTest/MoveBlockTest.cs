using DesktopTetris;

namespace Tests.BlockTest;

public class MoveBlockTest : BlockTest
{
    [Test]
    public void TestMoveDown()
    {
        var block = new Block();
        var anchor = block.GetAnchorPosition();
        block.MoveDown();

        Assert.That(block.GetAnchorPosition(), Is.EqualTo((anchor.x, anchor.y + 1)));
    }

    [Test]
    public void TestSpawnPosition()
    {
        var block = new Block();
        Assert.That(block.GetAnchorPosition(), Is.EqualTo((5, -block.Matrice.GetLength(0))));
    }
    
    [Test]
    public void TestCollideWithBottom()
    {
        var anchor = (0, 15);
        var block = new Block(anchorPosition: anchor);
        block.MoveDown();
        Assert.Multiple(() =>
        {
            Assert.That(block.GetAnchorPosition(), Is.EqualTo(anchor));
            Assert.That(block.AlreadyFallen, Is.EqualTo(true));
        });
    }

    [Test]
    public void TestMoveToSide()
    {
        var block = new Block();
        var anchor = block.GetAnchorPosition();
        block.Move(-1);
        Assert.That(block.GetAnchorPosition(), Is.EqualTo((anchor.x - 1, anchor.y)));
    }
    
    [Test]
    public void TestStopAtSideBorder()
    {
        var block = AddNewBlock(anchorPosition: (0, 0));
        var anchor = block.GetAnchorPosition();
        block.Move(-1);
        Assert.That(block.GetAnchorPosition(), Is.EqualTo(anchor));
    }
    
    [Test]
    public void TestCollision()
    {
        var block1 = AddNewBlock(blockTypes[BlockType.I], (0, 15));
        var block = AddNewBlock(blockTypes[BlockType.I], (0, 14));

        var anchor = block.GetAnchorPosition();
        block.MoveDown();
        Assert.That(block.GetAnchorPosition(), Is.EqualTo(anchor));
    }
}
