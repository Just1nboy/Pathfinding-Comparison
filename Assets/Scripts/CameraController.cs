using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
            Debug.LogError("no camera found!");
    }

    public void FocusOnGrid(int width, int height)
    {
        float cx = width  / 2f - 0.5f;
        float cy = height / 2f - 0.5f;
        cam.transform.position = new Vector3(cx, cy, -10f);
        cam.orthographicSize = Mathf.Max(width, height) / 2f + 2f;
    }
}