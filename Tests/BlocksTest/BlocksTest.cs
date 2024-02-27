namespace Tests.BlocksTest;

public class BlocksTest : TestBase
{
    [Test]
    public void TestSpawnValues()
    {
        var block = AddNewBlock();
        
        Assert.That(block.Anchor, Is.EqualTo((5, -block.Matrice.GetLength(0))));
    } 
}
