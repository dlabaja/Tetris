namespace Tests.GameTest;

public class GameTest : TestBase
{
    [Test]
    public void TestAddBlock()
    {
        var block = AddNewBlock();
        Assert.That(game.Blocks.First(), Is.EqualTo(block));
    }
    
    [Test]
    public void TestRemoveBlock()
    {
        var block = AddNewBlock();
        game.RemoveBlock(block);
        Assert.That(game.Blocks.Count(), Is.EqualTo(0));
    }
}
