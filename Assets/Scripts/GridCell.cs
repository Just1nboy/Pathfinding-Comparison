using System.Collections.Generic;

public class GridCell
{
    public int X { get; }
    public int Y { get; }
    public bool IsBlocked { get; private set; }
    public bool IsStart   { get; private set; }
    public bool IsEnd     { get; private set; }

    private CellVisual visual;

    public GridCell(int x, int y)
    {
        X = x;
        Y = y;
        IsBlocked = false;
        IsStart   = false;
        IsEnd     = false;
    }

    public void SetBlocked(bool blocked)
    {
        IsBlocked = blocked;
        visual?.ShowBlocked(blocked);
    }

    public void SetVisual(CellVisual v)
    {
        visual = v;
        visual.ShowBlocked(IsBlocked);
    }

    public void MarkAsStart()
    {
        IsStart = true;
        visual.ShowStart();
    }

    public void MarkAsEnd()
    {
        IsEnd = true;
        visual.ShowEnd();
    }
}