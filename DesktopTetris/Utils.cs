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
    
    public static IEnumerable<List<Block>> BlockMapToList(List<Block>[,] map)
    {
        var flattenedList = new List<List<Block>>();

        for (int y = 0; y < map.GetLength(0); y++)
        for (int x = 0; x < map.GetLength(1); x++)
            flattenedList.Add(map[y, x]);

        return flattenedList;
    }
}
