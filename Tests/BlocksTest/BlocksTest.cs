namespace Tests.BlocksTest;

public class BlocksTest : TestBase
{
    [Test]
    public void TestSpawnValues()
    {
        var block = AddNewBlock();
        
        Assert.That(block.Anchor, Is.EqualTo((5, -block.Matrice.GetLength(0))));
    }
    
    [Test]
    public void TestMoveTo()
    {
        var block = AddNewBlock(blockTypes[BlockType.Square] ,(0, 0));
        block.MoveTo((5, 5));
        
        Assert.That(game.Map[5, 5][0], Is.EqualTo(block));
    } 
}
