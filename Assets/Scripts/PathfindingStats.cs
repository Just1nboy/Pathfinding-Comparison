public class PathfindingStats
{
    public float TimeSeconds;
    public int VisitedCells;
    public int PathLength;

    public override string ToString()
    {
        return $"Time: {TimeSeconds:0.000}s, Visited: {VisitedCells}, Path Length: {PathLength}";
    }
}
