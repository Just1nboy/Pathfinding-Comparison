using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathfindingManager))]
public class PathfindingManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        PathfindingManager manager = (PathfindingManager)target;

        GUI.backgroundColor = Color.cyan;
        if (GUILayout.Button("Run A* Pathfinder"))
        {
            manager.RunAStar();
        }

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Run Greedy Pathfinder"))
        {
            manager.RunGreedy();
        }

        GUI.backgroundColor = Color.magenta;
        if (GUILayout.Button("Run Dijkstra Pathfinder"))
        {
            manager.RunDijkstra();
        }


        GUI.backgroundColor = Color.white;
    }
}
