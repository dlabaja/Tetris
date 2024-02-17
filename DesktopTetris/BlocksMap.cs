namespace DesktopTetris;

public class BlocksMap
{
    public List<Block>[,] map;

    public BlocksMap()
    {
        map = new List<Block>[Game.mapHeight, Game.mapWidth];
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                map[y, x] = new List<Block>();
            }
        }
    }

    public void ClearMap()
    {
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                map[y, x].Clear();
            }
        }
    }
}
