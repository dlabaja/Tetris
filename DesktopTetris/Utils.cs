namespace DesktopTetris;

public static class Utils
{
    public static bool InMapRange((int x, int y) pos)
    {
        return InMapXRange(pos) && pos.y is >= 0 and < Game.mapHeight;
    }
    
    public static bool InMapXRange((int x, int y) pos)
    {
        return pos.x is >= 0 and < Game.mapWidth;
    }
}
