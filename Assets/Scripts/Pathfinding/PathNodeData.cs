public class PathNodeData
{
    public GridCell Cell;
    public GridCell Parent;
    public int GCost;
    public int HCost;
    public int FCost => GCost + HCost;

    public PathNodeData(GridCell cell)
    {
        Cell = cell;
        GCost = int.MaxValue;
        HCost = int.MaxValue;
    }
}
