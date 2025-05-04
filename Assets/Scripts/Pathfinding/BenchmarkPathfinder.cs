using System.Threading.Tasks;

public class BenchmarkPathfinder
{
    private IPathfinder algo;

    public BenchmarkPathfinder(IPathfinder algo)
    {
        this.algo = algo;
    }

    public async Task<PathfindingStats> RunAsync(GridCell start, GridCell end, GridCell[,] grid)
    {
        var tcs = new System.Threading.Tasks.TaskCompletionSource<PathfindingStats>();
        PathfindingStats captured = null;

        if (algo is AStarPathfinder a)
            a.OnComplete = (s) => { captured = s; tcs.SetResult(s); };
        else if (algo is GreedyPathfinder g)
            g.OnComplete = (s) => { captured = s; tcs.SetResult(s); };
        else if (algo is DijkstraPathfinder d)
            d.OnComplete = (s) => { captured = s; tcs.SetResult(s); };

        algo.FindPath(start, end, grid);

        return await tcs.Task;
    }
}
