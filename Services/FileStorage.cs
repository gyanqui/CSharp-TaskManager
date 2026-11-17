using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using PersonalTaskManager.Models;

namespace PersonalTaskManager.Services;

/// <summary>
/// FILE I/O: saves the task list to a JSON file and loads it back when the
/// program starts, so tasks are not lost when the program closes.
/// "static" means all methods belong to the class itself, so no object
/// needs to be created to use them.
/// </summary>
public static class FileStorage
{
    // WriteIndented makes the JSON file easy for a human to read.
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    /// <summary>Writes every task to the file at the given path.</summary>
    public static void Save(List<TaskItem> tasks, string path)
    {
        try
        {
            // Make sure the folder (e.g. "data") exists before writing.
            string? folder = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string json = JsonSerializer.Serialize(tasks, Options);
            File.WriteAllText(path, json);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Could not save tasks: {ex.Message}");
        }
    }

    /// <summary>
    /// Reads tasks from the file. Returns an empty list if the file does not
    /// exist yet (first run) or cannot be understood.
    /// </summary>
    public static List<TaskItem> Load(string path)
    {
        if (!File.Exists(path))
        {
            return new List<TaskItem>();
        }

        try
        {
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }
        catch (JsonException)
        {
            Console.WriteLine("The task file is damaged. Starting with an empty list.");
            return new List<TaskItem>();
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Could not read tasks: {ex.Message}");
            return new List<TaskItem>();
        }
    }
}
