using UnityEngine;

public class GridView : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private int mapWidth = 10;
    [SerializeField] private int mapHeight = 10;
    [SerializeField][Range(0f, 0.5f)] private float blockRatio = 0.2f;

    [Header("References")]
    [SerializeField] private CameraController cameraController;

    private GridMap gridMap;
    public GridMap GetMap() => gridMap;

    private void Start()
    {
        GenerateMaze();
    }

    public void GenerateMaze()
    {
        // Cleanup old
        foreach (Transform t in transform) DestroyImmediate(t.gameObject);

        // New map
        gridMap = new GridMap(mapWidth, mapHeight, blockRatio);

        for (int x = 0; x < gridMap.Width; x++)
        {
            for (int y = 0; y < gridMap.Height; y++)
            {
                Vector3 pos = new Vector3(x, y, 0);
                var go = Instantiate(cellPrefab, pos, Quaternion.identity, transform);
                var visual = go.GetComponent<CellVisual>();
                var cell = gridMap.AllCells[x, y];
                cell.SetVisual(visual);
            }
        }

        gridMap.StartCell.MarkAsStart();
        gridMap.EndCell.MarkAsEnd();

        cameraController?.FocusOnGrid(gridMap.Width, gridMap.Height);
    }

    public void ResetVisuals()
    {
       foreach (var cell in gridMap.AllCells)
      {
          if (!cell.IsBlocked && !cell.IsStart && !cell.IsEnd)
          {
              cell.Visual.ShowBlocked(false);  // white
          }
       }
    }


}