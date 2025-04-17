using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CellVisual : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void ShowBlocked(bool blocked)
    {
        sr.color = blocked ? Color.black : Color.white;
    }

    public void ShowStart()
    {
        sr.color = Color.green;
    }

    public void ShowEnd()
    {
        sr.color = Color.red;
    }
}