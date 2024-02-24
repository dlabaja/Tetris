namespace DesktopTetris.GtkWindows;

public class BlocksMap
{
    public List<Block>[,] map = new List<Block>[Game.mapHeight, Game.mapWidth];

    public BlocksMap()
    {
        for (int y = 0; y < map.GetLength(0); y++)
        for (int x = 0; x < map.GetLength(1); x++)
            map[y, x] = new List<Block>();
    }
    
    public void RemoveBlock(Block block)
    {
        for (int y = 0; y < map.GetLength(0); y++)
        for (int x = 0; x < map.GetLength(1); x++)
            if (map[y, x].Contains(block))
                map[y, x].Remove(block);
    }

    private static bool InRange((int x, int y) pos) => pos.x is >= 0 and < Game.mapWidth && pos.y is >= 0 and < Game.mapHeight;

    public void AddBlock(Block block)
    {
        for (int y = 0; y < block.Matrice.GetLength(0); y++)
        for (int x = 0; x < block.Matrice.GetLength(1); x++)
        {
            var pos = block.GetMapRelativePosition(x, y);
            if (block.Matrice[y, x] && InRange((pos.x, pos.y)))
                map[pos.y, pos.x].Add(block);
        }
    }
    
    public IEnumerable<List<Block>> ToList()
    {
        var flattenedList = new List<List<Block>>();

        for (int y = 0; y < map.GetLength(0); y++)
        for (int x = 0; x < map.GetLength(1); x++)
            flattenedList.Add(map[y, x]);

        return flattenedList;
    }
}
