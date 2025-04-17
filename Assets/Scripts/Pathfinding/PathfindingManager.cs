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

    public void RunAStar()
    {
        var map = gridView.GetMap();
        gridView.ResetVisuals();
        aStar.FindPath(map.StartCell, map.EndCell, map.AllCells);
    }

    public void RunGreedy()
    {
        var map = gridView.GetMap();
        gridView.ResetVisuals();
        greedy.FindPath(map.StartCell, map.EndCell, map.AllCells);
    }

    public void RunDijkstra()
    {
        var map = gridView.GetMap();
        gridView.ResetVisuals();
        dijkstra.FindPath(map.StartCell, map.EndCell, map.AllCells);
    }

}
