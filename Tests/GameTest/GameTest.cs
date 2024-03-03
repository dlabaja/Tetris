using DesktopTetris;

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

    [Test]
    public void TestNextTurn()
    {
        var block = AddNewBlock();
        var anchor = block.Anchor;
        game.NextTurn();
        Assert.That(block.Anchor.y, Is.EqualTo(anchor.y + 1));
    }

    [Test]
    public void TestIsCurrentBlock()
    {
        Assert.That(Game.currentGame, Is.EqualTo(game));
    }
}
