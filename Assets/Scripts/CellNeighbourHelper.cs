using UnityEngine;
using System.Collections.Generic;

public static class CellNeighbourHelper
{
    private static readonly Vector2Int[] Directions = new Vector2Int[]
    {
        new Vector2Int( 0,  1),
        new Vector2Int( 1,  0),
        new Vector2Int( 0, -1),
        new Vector2Int(-1,  0),
    };

    public static List<GridCell> GetNeighbours(GridCell[,] grid, int x, int y)
    {
        var list = new List<GridCell>();
        int w = grid.GetLength(0), h = grid.GetLength(1);

        foreach (var d in Directions)
        {
            int nx = x + d.x, ny = y + d.y;
            if (nx >= 0 && ny >= 0 && nx < w && ny < h)
            {
                var c = grid[nx, ny];
                if (!c.IsBlocked)
                    list.Add(c);
            }
        }
        return list;
    }
}