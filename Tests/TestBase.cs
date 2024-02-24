using DesktopTetris;
using Gdk;

namespace Tests;

public class TestBase
{
    protected Game game;

    protected enum BlockType
    {
        L,
        I,
        Square,
        S,
        Z,
        J
    }

    protected static readonly Dictionary<BlockType, bool[,]> blockTypes = new Dictionary<BlockType, bool[,]>
    {
        { BlockType.L, new[,]
            {
                { false, false, true },
                { true, true, true }
            }
        },
        { BlockType.I, new[,]
            {
                { true, true, true, true }
            }
        },
        { BlockType.Square, new[,]
            {
                { true, true },
                { true, true }
            }
        },
        { BlockType.S, new[,]
            {
                { false, true, true },
                { true, true, false }
            }
        },
        { BlockType.Z, new[,]
            {
                { true, true, false },
                { false, true, true }
            }
        },
        { BlockType.J, new[,]
            {
                { true, false, false },
                { true, true, true }
            }
        }
    };

    [SetUp]
    public void Setup()
    {
        game = new Game();
    }

    protected Block AddNewBlock(bool[,]? matrice = null, (int x, int y)? anchorPosition = null, bool alreadyFallen = false, Color? color = null)
    {
        var block = new Block(matrice, anchorPosition, alreadyFallen, color);
        game.AddBlock(block);
        return block;
    }
}
