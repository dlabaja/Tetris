using DesktopTetris;

namespace Tests.GameTest;

public class CompleteLineTest : TestBase
{
    [Test]
    public void TestCompleteLineNoRemnants()
    {
        AddNewBlock(blockTypes[BlockType.Square], (0, 15));
        AddNewBlock(blockTypes[BlockType.Square], (2, 15), false);
        AddNewBlock(blockTypes[BlockType.Square], (4, 15), false);
        AddNewBlock(blockTypes[BlockType.Square], (6, 15), false);
        AddNewBlock(blockTypes[BlockType.Square], (8, 15), false);
        
        game.NextTurn();
        
        Assert.That(game.Blocks.Count, Is.EqualTo(1));
    }

    [Test]
    public void TestCompleteLineWithRemnants()
    {
        AddNewBlock(blockTypes[BlockType.S], (0, 14), false);
        AddNewBlock(blockTypes[BlockType.S], (2, 14), false);
        AddNewBlock(blockTypes[BlockType.S], (4, 14), false);
        AddNewBlock(blockTypes[BlockType.S], (6, 14), false);
        
        var block = AddNewBlock(blockTypes[BlockType.J]);
        block.RotateRight();
        block.RotateRight();
        block.RotateRight();
        block.MoveTo((8, 13));
        
        game.NextTurn();
        Assert.That(game.Blocks.Count(x => x.MatriceToList().Count(x => x) == 2), Is.EqualTo(5));
    }

    [Test]
    public void TestFallDownAfterCompletition()
    {
        TestCompleteLineWithRemnants();
        var blocksAndRows = game.Blocks.ToDictionary(block => block, block => block.Anchor.y);

        game.NextTurn();

        foreach (var (block, row) in blocksAndRows)
        {
            Assert.That(block.Anchor.y, Is.EqualTo(row + 1));
        }
    }
}
