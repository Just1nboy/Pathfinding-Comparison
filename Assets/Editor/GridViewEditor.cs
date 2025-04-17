using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridView))]
public class GridViewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridView gv = (GridView)target;

        GUILayout.Space(10);
        if (GUILayout.Button("Regenerate Maze"))
        {
            gv.GenerateMaze();
        }
    }
}
