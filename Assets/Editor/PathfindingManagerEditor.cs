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

        GUILayout.Space(10);
        GUILayout.Label("Benchmark Tests", EditorStyles.boldLabel);

        if (GUILayout.Button("Benchmark A* (100 runs)"))
            manager.RunAStarBenchmark();

        if (GUILayout.Button("Benchmark Greedy (100 runs)"))
            manager.RunGreedyBenchmark();

        if (GUILayout.Button("Benchmark Dijkstra (100 runs)"))
            manager.RunDijkstraBenchmark();

        GUILayout.Space(10);
        GUILayout.Label("Full Comparison Benchmark", EditorStyles.boldLabel);
        if (GUILayout.Button("Run Full Benchmark (100×3)"))
            manager.RunFullBenchmark();



        GUI.backgroundColor = Color.white;
    }
}
