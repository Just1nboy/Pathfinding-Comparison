using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GreedyPathfinder : IPathfinder
{
    public System.Action<PathfindingStats> OnComplete;

    public async void FindPath(GridCell start, GridCell end, GridCell[,] grid)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        var openList = new List<PathNodeData>();
        var closedList = new HashSet<GridCell>();
        var allNodes = new Dictionary<GridCell, PathNodeData>();

        foreach (var cell in grid)
            allNodes[cell] = new PathNodeData(cell);

        var startNode = allNodes[start];
        startNode.HCost = Heuristic(start, end);
        openList.Add(startNode);

        int visited = 0;

        while (openList.Count > 0)
        {
            var current = openList.OrderBy(n => n.HCost).First();

            if (current.Cell != start && current.Cell != end)
            {
                current.Cell.Visual.ShowVisited();
                visited++;
            }

            if (current.Cell == end)
            {
                stopwatch.Stop();
                var stats = await VisualizePath(current, allNodes, stopwatch.Elapsed.TotalSeconds, visited);
                Debug.Log($"Greedy Stats: {stats}");
                OnComplete?.Invoke(stats);
                return;
            }

            openList.Remove(current);
            closedList.Add(current.Cell);

            foreach (var neighbor in CellNeighbourHelper.GetNeighbours(grid, current.Cell.X, current.Cell.Y))
            {
                if (closedList.Contains(neighbor)) continue;

                var neighborNode = allNodes[neighbor];
                int h = Heuristic(neighbor, end);

                if (neighborNode.Parent == null || h < neighborNode.HCost)
                {
                    neighborNode.Parent = current.Cell;
                    neighborNode.HCost = h;

                    if (!openList.Contains(neighborNode))
                        openList.Add(neighborNode);
                }
            }

            await Awaitable.WaitForSecondsAsync(0.01f);
        }

        stopwatch.Stop();
        Debug.LogWarning("Greedy failed.");
        OnComplete?.Invoke(new PathfindingStats { TimeSeconds = (float)stopwatch.Elapsed.TotalSeconds });
    }

    private async System.Threading.Tasks.Task<PathfindingStats> VisualizePath(
        PathNodeData endNode, Dictionary<GridCell, PathNodeData> allNodes, double timeElapsed, int visited)
    {
        var path = new List<GridCell>();
        var current = endNode;

        while (current.Parent != null)
        {
            path.Add(current.Cell);
            current = allNodes[current.Parent];
        }

        path.Reverse();
        foreach (var step in path)
        {
            if (!step.IsStart && !step.IsEnd)
                step.Visual.ShowPath();
            await Awaitable.WaitForSecondsAsync(0.05f);
        }

        return new PathfindingStats
        {
            TimeSeconds = (float)timeElapsed,
            VisitedCells = visited,
            PathLength = path.Count
        };
    }

    private int Heuristic(GridCell a, GridCell b)
    {
        return (Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y)) * 10;
    }
}
