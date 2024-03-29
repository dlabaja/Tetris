using DesktopTetris;
using DesktopTetris.GtkWindows;

namespace Tests.GameTest;

public class MapTest : TestBase
{
    private BlocksMap map = null!;

    [SetUp]
    public new void Setup()
    {
        map = new BlocksMap();
    }

    [Test]
    public void TestAddBlockOutsideOfMap()
    {
        var block = new Block(blockTypes[BlockType.I]);
        
        map.AddBlock(block);
        Assert.That(!map.ToList().Any(x => x.Count > 0));
    }
    
    [Test]
    public void TestRemoveBlock()
    {
        var block = new Block(blockTypes[BlockType.I], (0, 0));
        
        map.AddBlock(block);
        map.RemoveBlock(block);
        Assert.That(!map.ToList().Any(x => x.Count > 0));
    }
    
    [Test]
    public void TestRemoveNonExistentBlock()
    {
        var block = new Block(blockTypes[BlockType.I]);
        map.RemoveBlock(block);
    }
    
    [Test]
    public void TestAddBlockInsideOfMap()
    {
        var block = new Block(blockTypes[BlockType.I], (0, 0));
        
        map.AddBlock(block);
        Assert.That(map.ToList().Count(x => x.Count == 1), Is.EqualTo(4)); 
    }
    
    [Test]
    public void TestEmpty()
    {
        Assert.That(!map.ToList().Any(x => x.Count > 0));
    }
    
    [Test]
    public void TestFilledRowsNextToEachOther()
    {
        map.AddBlock(new Block(blockTypes[BlockType.Square], (0, 0)));
        map.AddBlock(new Block(blockTypes[BlockType.Square], (2, 0)));
        map.AddBlock(new Block(blockTypes[BlockType.Square], (4, 0)));
        map.AddBlock(new Block(blockTypes[BlockType.Square], (6, 0)));
        map.AddBlock(new Block(blockTypes[BlockType.Square], (8, 0)));
        
        Assert.That(map.GetFilledRowsIndexes(), Is.EqualTo(new List<int>{0, 1}));
    }
}
