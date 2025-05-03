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

    // blockRatio is no longer used by this generator, but kept to match constructor signature
    public GridMap(int width, int height, float blockRatio)
    {
        Width = width;
        Height = height;

        InitializeGrid();
        GenerateMaze();
        PickRandomStartEnd();

        OnMapGenerated?.Invoke();
    }

    private void InitializeGrid()
    {
        AllCells = new GridCell[Width, Height];
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                AllCells[x, y] = new GridCell(x, y);
    }

    private void GenerateMaze()
    {
        // 1. Fill entire grid with walls
        foreach (var cell in AllCells)
            cell.SetBlocked(true);

        // 2. Standard recursive backtracking (DFS) maze generation
        var stack = new Stack<GridCell>();
        int sx = UnityEngine.Random.Range(0, Width / 2) * 2;
        int sy = UnityEngine.Random.Range(0, Height / 2) * 2;
        var start = AllCells[sx, sy];
        start.SetBlocked(false);
        stack.Push(start);

        var directions = new (int dx, int dy)[]
        {
        ( 0,  2),
        ( 2,  0),
        ( 0, -2),
        (-2,  0)
        };

        while (stack.Count > 0)
        {
            var current = stack.Peek();
            var neighbors = new List<GridCell>();

            foreach (var (dx, dy) in directions)
            {
                int nx = current.X + dx;
                int ny = current.Y + dy;

                if (nx >= 0 && ny >= 0 && nx < Width && ny < Height)
                {
                    var neighbor = AllCells[nx, ny];
                    if (neighbor.IsBlocked)
                        neighbors.Add(neighbor);
                }
            }

            if (neighbors.Count > 0)
            {
                var next = neighbors[UnityEngine.Random.Range(0, neighbors.Count)];
                int wallX = (current.X + next.X) / 2;
                int wallY = (current.Y + next.Y) / 2;

                AllCells[wallX, wallY].SetBlocked(false);
                next.SetBlocked(false);
                stack.Push(next);
            }
            else
            {
                stack.Pop();
            }
        }

        // 3. Add extra openings (optional loops)
        int extraOpenings = Mathf.FloorToInt((Width + Height) * 0.75f); // tweakable
        int attempts = 0;

        while (extraOpenings > 0 && attempts < 500)
        {
            int x = UnityEngine.Random.Range(1, Width - 1);
            int y = UnityEngine.Random.Range(1, Height - 1);

            // Must be a wall between two open cells (vertical or horizontal only)
            if (!AllCells[x, y].IsBlocked)
            {
                attempts++;
                continue;
            }

            bool vertical = AllCells[x - 1, y].IsBlocked == false && AllCells[x + 1, y].IsBlocked == false;
            bool horizontal = AllCells[x, y - 1].IsBlocked == false && AllCells[x, y + 1].IsBlocked == false;

            if (vertical || horizontal)
            {
                AllCells[x, y].SetBlocked(false);
                extraOpenings--;
            }

            attempts++;
        }
    }


    private void PickRandomStartEnd()
    {
        // 1) Pick Start on any open cell
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

        // 2) End must be at least half‐maze away
        int maxM = (Width - 1) + (Height - 1);
        int minDist = maxM / 2;

        var candidates = new List<GridCell>();
        foreach (var c in AllCells)
        {
            if (c == StartCell || c.IsBlocked) continue;
            int d = Mathf.Abs(c.X - StartCell.X) + Mathf.Abs(c.Y - StartCell.Y);
            if (d >= minDist)
                candidates.Add(c);
        }

        if (candidates.Count > 0)
        {
            EndCell = candidates[UnityEngine.Random.Range(0, candidates.Count)];
        }
        else
        {
            // fallback (should only happen on very small maps)
            EndCell = StartCell;
            Debug.LogWarning("No distant end found; using start cell as end.");
        }
    }
}
