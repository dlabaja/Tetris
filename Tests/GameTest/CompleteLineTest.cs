using DesktopTetris;

namespace Tests.GameTest;

public class CompleteLineTest : TestBase
{
    [Test]
    public void TestCompleteDoubleLineNoRemnants()
    {
        AddNewBlock(blockTypes[BlockType.Square], (0, 14));
        AddNewBlock(blockTypes[BlockType.Square], (2, 14), false);
        AddNewBlock(blockTypes[BlockType.Square], (4, 14), false);
        AddNewBlock(blockTypes[BlockType.Square], (6, 14), false);
        AddNewBlock(blockTypes[BlockType.Square], (8, 14), false);

        game.NextTurn();
        Assert.Multiple(() =>
        {
            Assert.That(game.Blocks.Count, Is.EqualTo(1));
            Assert.That(Utils.BlockMapToList(game.Map).Count(x => x.Count != 0), Is.EqualTo(0));
        });
    }
    
    [Test]
    public void TestCompleteDoubleLineWithRemnants()
    {
        AddNewBlock(blockTypes[BlockType.Square], (0, 14));
        AddNewBlock(blockTypes[BlockType.Square], (2, 14), false);
        AddNewBlock(blockTypes[BlockType.Square], (4, 14), false);
        AddNewBlock(blockTypes[BlockType.Square], (6, 14), false);
        
        var block5 = AddNewBlock(blockTypes[BlockType.I], (0, 0), false);
        block5.RotateRight();
        block5.MoveTo((8, 12));
        
        var block6 = AddNewBlock(blockTypes[BlockType.I], (0, 0), false);
        block6.RotateRight();
        block6.MoveTo((9, 12));

        game.NextTurn();
        Assert.Multiple(() =>
        {
            Assert.That(game.Blocks.Count, Is.EqualTo(3));
            Assert.That(Utils.BlockMapToList(game.Map).Count(x => x.Count != 0), Is.EqualTo(4));
        });
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
    public void TestCompleteLineBlockCollidesWithBlockAndBottom()
    {
        var block = AddNewBlock(blockTypes[BlockType.S], (0, 14));
        AddNewBlock(blockTypes[BlockType.S], (2, 14), false);
        AddNewBlock(blockTypes[BlockType.S], (4, 14), false);
        AddNewBlock(blockTypes[BlockType.S], (6, 14), false);
        var _block = AddNewBlock(blockTypes[BlockType.J], canSpawnNewBlock: false);
        _block.RotateRight();
        _block.RotateRight();
        _block.RotateRight();
        _block.MoveTo((8, 13));

        game.NextTurn();
        Assert.Multiple(() =>
        {
            Assert.That(block.Anchor, Is.EqualTo((0, 14)));
            Assert.That(game.Blocks.Count(), Is.EqualTo(6));
        });
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
