using System;
using UnityEngine;

public class GridMap
{
    public Action OnGridGenerated;

    public int Width { get; private set; }
    public int Height { get; private set; }
    public GridCell[,] AllCells { get; private set; }

    private float _blockCellChance;

    public GridMap(int width, int height, float blockChance)
    {
        Width = width;
        Height = height;
        _blockCellChance = blockChance;
        AllCells = new GridCell[Width, Height];

        GenerateGrid();
        RandomizeBlockedCells();
        OnGridGenerated?.Invoke();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                AllCells[x, y] = new GridCell(this, x, y);
            }
        }
    }

    private void RandomizeBlockedCells()
    {
        int totalCells = Width * Height;
        int targetBlocked = Mathf.FloorToInt(totalCells * _blockCellChance);
        int blocked = 0;
        int attempts = 0;

        while (blocked < targetBlocked && attempts < totalCells * 2)
        {
            int randX = UnityEngine.Random.Range(0, Width);
            int randY = UnityEngine.Random.Range(0, Height);
            GridCell cell = AllCells[randX, randY];

            if (!cell.IsBlocked)
            {
                cell.SetBlocked(true);
                blocked++;
            }
            attempts++;
        }
    }
}