using System.Collections.Generic;

public class CellNeighbourHelper
{
    private GridCell _cell;
    private GridMap _map;

    public CellNeighbourHelper(GridCell cell, GridMap map)
    {
        _cell = cell;
        _map = map;
    }

    public GridCell[] GetNeighbours()
    {
        List<GridCell> neighbours = new List<GridCell>();
        int[,] directions = new int[,]
        {
            { 0, 1 },  // Up
            { 1, 0 },  // Right
            { 0, -1 }, // Down
            { -1, 0 }  // Left
        };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int newX = _cell.X + directions[i, 0];
            int newY = _cell.Y + directions[i, 1];

            if (IsValidPosition(newX, newY))
            {
                GridCell neighbour = _map.AllCells[newX, newY];
                if (!neighbour.IsBlocked)
                {
                    neighbours.Add(neighbour);
                }
            }
        }

        return neighbours.ToArray();
    }

    private bool IsValidPosition(int x, int y)
    {
        return x >= 0 && y >= 0 && x < _map.Width && y < _map.Height;
    }
}