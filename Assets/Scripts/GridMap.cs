using System;
using System.Collections.Generic;
using UnityEngine;

public class GridMap
{
    public event Action OnMapGenerated;
    public int Width { get; private set; }
    public int Height { get; private set; }
    public GridCell[,] AllCells { get; private set; }
    public GridCell StartCell { get; private set; }
    public GridCell EndCell { get; private set; }

    private float blockRatio; // no longer used, but kept for signature

    public GridMap(int width, int height, float blockRatio)
    {
        Width = width;
        Height = height;
        this.blockRatio = blockRatio;

        GenerateGrid();
        GenerateMaze();            // <-- replace random blocks
        PickRandomStartEnd();      // <-- picks start/end half-maze apart

        OnMapGenerated?.Invoke();
    }

    private void GenerateGrid()
    {
        AllCells = new GridCell[Width, Height];
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                AllCells[x, y] = new GridCell(x, y);
    }

    private void GenerateMaze()
    {
        // Start with every cell open
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                AllCells[x, y].SetBlocked(false);

        // Recursively divide the area into a perfect maze
        Divide(0, 0, Width, Height);
    }

    private void Divide(int x, int y, int width, int height)
    {
        // Stop if the region is too small
        if (width < 2 || height < 2)
            return;

        bool horizontal = width < height;

        if (horizontal)
        {
            // horizontal division
            int divideY = y + UnityEngine.Random.Range(1, height); // row to place wall
            int passageX = x + UnityEngine.Random.Range(0, width);

            // draw the wall
            for (int ix = x; ix < x + width; ix++)
            {
                if (ix == passageX) continue;
                AllCells[ix, divideY].SetBlocked(true);
            }

            // recurse above and below
            int topHeight = divideY - y;
            int bottomHeight = height - topHeight;
            Divide(x, y, width, topHeight);
            Divide(x, divideY + 1, width, bottomHeight - 1);
        }
        else
        {
            // vertical division
            int divideX = x + UnityEngine.Random.Range(1, width); // column to place wall
            int passageY = y + UnityEngine.Random.Range(0, height);

            // draw the wall
            for (int iy = y; iy < y + height; iy++)
            {
                if (iy == passageY) continue;
                AllCells[divideX, iy].SetBlocked(true);
            }

            // recurse left and right
            int leftWidth = divideX - x;
            int rightWidth = width - leftWidth;
            Divide(x, y, leftWidth, height);
            Divide(divideX + 1, y, rightWidth - 1, height);
        }
    }

    private void PickRandomStartEnd()
    {
        // 1) Place Start in any non-blocked cell
        while (true)
        {
            int sx = UnityEngine.Random.Range(0, Width);
            int sy = UnityEngine.Random.Range(0, Height);
            var s = AllCells[sx, sy];
            if (!s.IsBlocked)
            {
                StartCell = s;
                break;
            }
        }

        // 2) Ensure End is at least half the maximum Manhattan distance away
        int maxManhattan = (Width - 1) + (Height - 1);
        int minDistance = maxManhattan / 2;

        var candidates = new List<GridCell>();
        for (int ix = 0; ix < Width; ix++)
        {
            for (int iy = 0; iy < Height; iy++)
            {
                var c = AllCells[ix, iy];
                if (c.IsBlocked || c == StartCell)
                    continue;

                int dist = Mathf.Abs(c.X - StartCell.X)
                         + Mathf.Abs(c.Y - StartCell.Y);
                if (dist >= minDistance)
                    candidates.Add(c);
            }
        }

        if (candidates.Count > 0)
        {
            EndCell = candidates[UnityEngine.Random.Range(0, candidates.Count)];
        }
        else
        {
            // fallback
            EndCell = StartCell;
            Debug.LogWarning("No distant end found; using the start cell as end.");
        }
    }
}
