using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace GestorTareas2.Utils;

public class JsonHelper
{
    public static void SaveWorkTask<T>(string file, List<T> data)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(data, options);
        File.WriteAllText(file, json);
    }

    public static List<T> LoadWorkTask<T>(string file)
    {
        if (!File.Exists(file))
        {
            return new List<T>();
        }

        string json = File.ReadAllText(file);

        return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
    }
}