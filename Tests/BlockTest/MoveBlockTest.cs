namespace Tests.BlockTest;

public class MoveBlockTest : BlockTest
{
    [Test]
    public void TestMoveDown()
    {
        var anchor = block.GetAnchorPosition();
        
        block.MoveDown();

        Assert.That((anchor.x, anchor.y + 1), Is.EqualTo(block.GetAnchorPosition()));
    }
}
