using Gdk;

namespace DesktopTetris;

public class SplitBlock : Block
{
    public SplitBlock(bool[,] matrice, (int x, int y) anchor, Color color)
    {
        Matrice = matrice;
        AnchorPosition = new[]{anchor.x, anchor.y};
        Color = color;
        alreadyFallen = true;
        
        DisableInput();
    }
}
