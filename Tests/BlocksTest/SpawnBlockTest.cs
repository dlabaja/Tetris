namespace Tests.BlocksTest;

public class SpawnBlockTest : TestBase
{
    [Test]
    public void TestSpawnBlockAfterFallingDown()
    {
        var block = AddNewBlock(blockTypes[BlockType.I], (0, 0));
        for (int i = 0; i < 15; i++)
        {
            block.MoveDown();
        }
        
        Assert.That(block.Anchor, Is.EqualTo((0, 15)));
        
        block.MoveDown();
        
        Assert.That(game.Blocks.Count(), Is.EqualTo(2));
    }
    
    [Test]
    public void TestSpawnBlockAfterCollision()
    {
        AddNewBlock(blockTypes[BlockType.Square], (0, 14), false);
        var block = AddNewBlock(blockTypes[BlockType.Square], (0, 12));
        
        block.MoveDown();
        Assert.Multiple(() =>
        {
            Assert.That(block.Anchor, Is.EqualTo((0, 12)));
            Assert.That(game.Blocks.Count(), Is.EqualTo(3));
        });
    }

    [Test]
    public void TestCanSpawnNewBlockOnlyOnce()
    {
        var block = AddNewBlock(blockTypes[BlockType.I], (0, 15));
        block.MoveDown();
        
        Assert.That(game.Blocks.Count, Is.EqualTo(2));
        
        block.MoveTo((0, 15));
        block.MoveDown();
        Assert.That(game.Blocks.Count, Is.EqualTo(2));
    }
}
