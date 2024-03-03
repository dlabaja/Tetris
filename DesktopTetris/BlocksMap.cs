namespace DesktopTetris;

public class BlocksMap
{
    public readonly List<Block>[,] map = new List<Block>[Game.mapHeight, Game.mapWidth];

    public BlocksMap()
    {
        for (int y = 0; y < map.GetLength(0); y++)
        for (int x = 0; x < map.GetLength(1); x++)
            map[y, x] = new List<Block>();
    }

    public void UpdateMap(Block block)
    {
        RemoveBlock(block);
        AddBlock(block);
    }

    public bool CollisionDetected()
    {
        return ToList().Any(x => x.Count >= 2);
    }

    public void RemoveBlock(Block block)
    {
        for (int y = 0; y < map.GetLength(0); y++)
        for (int x = 0; x < map.GetLength(1); x++)
            if (map[y, x].Contains(block))
                map[y, x].Remove(block);
    }
    
    public void AddBlock(Block block)
    {
        for (int y = 0; y < block.Matrice.GetLength(0); y++)
        for (int x = 0; x < block.Matrice.GetLength(1); x++)
        {
            var pos = block.GetMapRelativePosition(x, y);
            if (block.Matrice[y, x] && Utils.InMapRange((pos.x, pos.y)))
                map[pos.y, pos.x].Add(block);
        }
    }

    public List<int> GetFilledRowsIndexes()
    {
        var indexes = new List<int>();
        for (int y = 0; y < map.GetLength(0); y++)
        {
            var isFilled = true;
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x].Any())
                    continue;
                
                isFilled = false;
                break;
            }

            if (isFilled)
                indexes.Add(y);
        }

        return indexes;
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
