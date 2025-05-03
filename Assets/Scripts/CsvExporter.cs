using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CsvExporter
{
    public static void Export(string filename, List<string[]> rows)
    {
        string path = Path.Combine(Application.dataPath, filename);
        using (var writer = new StreamWriter(path))
        {
            foreach (var row in rows)
                writer.WriteLine(string.Join(",", row));
        }
        Debug.Log($"CSV exported to: {path}");
    }
}
