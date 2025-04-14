using UnityEngine;

public class GridCell
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool IsBlocked { get; private set; }
    public GridMap ParentMap { get; private set; }
    public CellVisual Visual { get; private set; }

    private CellNeighbourHelper _neighbourHelper;

    public GridCell(GridMap map, int x, int y)
    {
        ParentMap = map;
        X = x;
        Y = y;
        IsBlocked = false;
        _neighbourHelper = new CellNeighbourHelper(this, map);
    }

    public void SetVisual(CellVisual visual)
    {
        Visual = visual;
        Visual.UpdateVisual(IsBlocked);
    }

    public void SetBlocked(bool blocked)
    {
        IsBlocked = blocked;
        Visual?.UpdateVisual(IsBlocked);
    }

    public GridCell[] GetNeighbours()
    {
        return _neighbourHelper.GetNeighbours();
    }
}