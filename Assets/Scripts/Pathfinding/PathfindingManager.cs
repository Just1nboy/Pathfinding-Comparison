using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    [SerializeField] private GridView gridView;

    private IPathfinder aStar;
    private IPathfinder greedy;
    private IPathfinder dijkstra;

    private void Awake()
    {
        aStar = new AStarPathfinder();
        greedy = new GreedyPathfinder();
        dijkstra = new DijkstraPathfinder();
    }
    public void RunAStar() => RunOnce(aStar);
    public void RunGreedy() => RunOnce(greedy);
    public void RunDijkstra() => RunOnce(dijkstra);

    private void RunOnce(IPathfinder algo)
    {
        var map = gridView.GetMap();
        gridView.ResetVisuals();
        algo.FindPath(map.StartCell, map.EndCell, map.AllCells);
    }

    public async void RunAStarBenchmark() => await RunBenchmark(aStar, "astar_benchmark.csv");
    public async void RunGreedyBenchmark() => await RunBenchmark(greedy, "greedy_benchmark.csv");
    public async void RunDijkstraBenchmark() => await RunBenchmark(dijkstra, "dijkstra_benchmark.csv");

    private async Task RunBenchmark(IPathfinder algo, string fileName)
    {
        Debug.Log($"Starting benchmark: {fileName}");
        var statsList = new List<PathfindingStats>();

        for (int i = 0; i < 100; i++)
        {
            gridView.GenerateMaze();
            await Task.Delay(100);

            var map = gridView.GetMap();
            var runner = new BenchmarkPathfinder(algo);
            var stats = await runner.RunAsync(map.StartCell, map.EndCell, map.AllCells);
            statsList.Add(stats);
        }

        var rows = new List<string[]>();
        rows.Add(new[] { "Run", "TimeSeconds", "VisitedCells", "PathLength" });
        for (int i = 0; i < statsList.Count; i++)
        {
            var d = statsList[i];
            rows.Add(new[]
            {
                (i + 1).ToString(),
                d.TimeSeconds.ToString("F4"),
                d.VisitedCells.ToString(),
                d.PathLength.ToString()
            });
        }

        CsvExporter.Export(fileName, rows);
        Debug.Log($"Finished benchmark: {fileName}");
    }

    public async void RunFullBenchmark()
    {
        Debug.Log("Starting full benchmark (100 mazes × 3 algos)");

        var allRows = new List<string[]>();
        allRows.Add(new[] { "Run", "Algorithm", "TimeSeconds", "VisitedCells", "PathLength" });

        for (int i = 0; i < 100; i++)
        {
            gridView.GenerateMaze();
            await Task.Delay(100);

            var map = gridView.GetMap();

            gridView.ResetVisuals();
            var aStats = await new BenchmarkPathfinder(new AStarPathfinder())
                                .RunAsync(map.StartCell, map.EndCell, map.AllCells);
            allRows.Add(new[]
            {
            (i + 1).ToString(), "AStar",
            aStats.TimeSeconds.ToString("F4"),
            aStats.VisitedCells.ToString(),
            aStats.PathLength.ToString()
        });

            gridView.ResetVisuals();
            var gStats = await new BenchmarkPathfinder(new GreedyPathfinder())
                                .RunAsync(map.StartCell, map.EndCell, map.AllCells);
            allRows.Add(new[]
            {
            (i + 1).ToString(), "Greedy",
            gStats.TimeSeconds.ToString("F4"),
            gStats.VisitedCells.ToString(),
            gStats.PathLength.ToString()
        });

            gridView.ResetVisuals();
            var dStats = await new BenchmarkPathfinder(new DijkstraPathfinder())
                                .RunAsync(map.StartCell, map.EndCell, map.AllCells);
            allRows.Add(new[]
            {
            (i + 1).ToString(), "Dijkstra",
            dStats.TimeSeconds.ToString("F4"),
            dStats.VisitedCells.ToString(),
            dStats.PathLength.ToString()
        });

            Debug.Log($"Completed maze {i + 1}/100");
        }

        CsvExporter.Export("combined_benchmark.csv", allRows);
        Debug.Log("Full benchmark complete.");
    }

}
