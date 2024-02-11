using DesktopTetris;

namespace Tests.BlockTest;

public class BlockTest
{
    protected Game game;
    protected Block block;

    [SetUp]
    public void Setup()
    {
        game = new Game();
        block = new Block();
    }
}
