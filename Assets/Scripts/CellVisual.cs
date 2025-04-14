using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CellVisual : MonoBehaviour
{
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateVisual(bool isBlocked)
    {
        _renderer.color = isBlocked ? Color.black : Color.white;
    }

    public void SetAsStart()
    {
        _renderer.color = Color.green;
    }

    public void SetAsEnd()
    {
        _renderer.color = Color.red;
    }

    public void SetAsPath()
    {
        _renderer.color = Color.cyan;
    }

    public void SetAsVisited()
    {
        _renderer.color = Color.yellow;
    }
}