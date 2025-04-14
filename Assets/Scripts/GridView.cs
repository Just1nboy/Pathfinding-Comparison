using UnityEngine;

public class GridView : MonoBehaviour
{
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField][Range(0, 0.5f)] private float blockChance = 0.2f;
    [SerializeField] private GameObject cellPrefab;

    private GridMap _gridMap;

private void Start()
{
    _gridMap = new GridMap(width, height, blockChance);
    GenerateGridVisuals();
}



    private void CenterCamera()
{
    Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10f);
    Camera.main.orthographicSize = Mathf.Max(width, height) / 2f + 2;
}

    private void GenerateGridVisuals()
{
    if (cellPrefab == null)
    {
        Debug.LogError("Cell Prefab is NOT assigned!");
        return;
    }

    Debug.Log("Generating grid visuals...");
    for (int x = 0; x < _gridMap.Width; x++)
    {
        for (int y = 0; y < _gridMap.Height; y++)
        {
            Vector2 position = new Vector2(x, y);
            GameObject cellObject = Instantiate(cellPrefab, position, Quaternion.identity, transform);
            CellVisual visual = cellObject.GetComponent<CellVisual>();

            GridCell cell = _gridMap.AllCells[x, y];
            cell.SetVisual(visual);
        }
    }
}

}