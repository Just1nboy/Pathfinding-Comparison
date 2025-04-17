using System;
using UnityEngine;

public class GridMap
{
    public event Action OnMapGenerated;
    public int Width { get; private set; }
    public int Height { get; private set; }
    public GridCell[,] AllCells { get; private set; }
    public GridCell StartCell { get; private set; }
    public GridCell EndCell   { get; private set; }

    private float blockRatio;

    public GridMap(int width, int height, float blockRatio)
    {
        Width = width;
        Height = height;
        this.blockRatio = blockRatio;

        GenerateGrid();
        BlockRandomCells();
        PickRandomStartEnd();

        OnMapGenerated?.Invoke();
    }

    private void GenerateGrid()
    {
        AllCells = new GridCell[Width, Height];
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                AllCells[x, y] = new GridCell(x, y);
    }

    private void BlockRandomCells()
    {
        int totalCells   = Width * Height;
        int targetBlocked = Mathf.FloorToInt(totalCells * blockRatio);
        int blocked       = 0;

        while (blocked < targetBlocked)
        {
            int x = UnityEngine.Random.Range(0, Width);
            int y = UnityEngine.Random.Range(0, Height);
            var cell = AllCells[x, y];
            if (!cell.IsBlocked)
            {
                cell.SetBlocked(true);
                blocked++;
            }
        }
    }

    private void PickRandomStartEnd()
    {
        // Pick Start
        while (true)
        {
            int x = UnityEngine.Random.Range(0, Width);
            int y = UnityEngine.Random.Range(0, Height);
            var c = AllCells[x, y];
            if (!c.IsBlocked)
            {
                StartCell = c;
                break;
            }
        }

        // Pick End (different from Start)
        while (true)
        {
            int x = UnityEngine.Random.Range(0, Width);
            int y = UnityEngine.Random.Range(0, Height);
            var c = AllCells[x, y];
            if (!c.IsBlocked && c != StartCell)
            {
                EndCell = c;
                break;
            }
        }
    }
}