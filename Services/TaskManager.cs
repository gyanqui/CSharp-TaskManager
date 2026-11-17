using System;
using System.Collections.Generic;
using System.Linq;
using PersonalTaskManager.Models;

namespace PersonalTaskManager.Services;

/// <summary>
/// FUNCTIONS requirement: holds the task list and the methods that add,
/// display, complete and organize tasks. Program.cs only handles the menu
/// and user input; the real work happens here.
/// </summary>
public class TaskManager
{
    private readonly List<TaskItem> _tasks; // all tasks in memory
    private readonly string _filePath;      // where tasks are saved
    private int _nextId;                    // id given to the next new task

    /// <summary>Creates the manager and loads any previously saved tasks.</summary>
    public TaskManager(string filePath)
    {
        _filePath = filePath;
        _tasks = FileStorage.Load(filePath);

        // EXPRESSION + CONDITIONAL: continue numbering after the highest id.
        _nextId = _tasks.Count == 0 ? 1 : _tasks.Max(t => t.Id) + 1;
    }

    /// <summary>Total number of tasks.</summary>
    public int Count => _tasks.Count;

    /// <summary>Adds a new task and saves the list.</summary>
    public TaskItem AddTask(string title, Priority priority, DateTime dueDate)
    {
        var task = new TaskItem(_nextId, title, priority, dueDate);
        _nextId++;
        _tasks.Add(task);
        Save();
        return task;
    }

    /// <summary>
    /// Marks a task as completed. Returns false if the id does not exist
    /// or the task was already completed.
    /// </summary>
    public bool CompleteTask(int id)
    {
        TaskItem? task = _tasks.FirstOrDefault(t => t.Id == id);

        if (task == null || task.IsCompleted)
        {
            return false;
        }

        task.IsCompleted = true;
        Save();
        return true;
    }

    /// <summary>
    /// Organizes the list: pending tasks first, then earliest due date,
    /// then highest priority. The result is kept in the list and saved.
    /// </summary>
    public void OrganizeTasks()
    {
        List<TaskItem> sorted = _tasks
            .OrderBy(t => t.IsCompleted)
            .ThenBy(t => t.DueDate)
            .ThenByDescending(t => t.PriorityLevel)
            .ToList();

        _tasks.Clear();
        _tasks.AddRange(sorted);
        Save();
    }

    /// <summary>Prints every task in a table.</summary>
    public void DisplayTasks()
    {
        PrintTable(_tasks, "No tasks yet. Add one from the menu!");
    }

    /// <summary>Prints only the overdue tasks.</summary>
    public void DisplayOverdueTasks()
    {
        List<TaskItem> overdue = _tasks.Where(t => t.GetStatus() == "Overdue").ToList();
        PrintTable(overdue, "Great news: no overdue tasks!");
    }

    /// <summary>
    /// EXPRESSION: percentage of completed tasks = completed / total * 100.
    /// Returns 0 when there are no tasks (avoids dividing by zero).
    /// </summary>
    public double GetCompletionPercentage()
    {
        if (_tasks.Count == 0)
        {
            return 0;
        }

        int completed = _tasks.Count(t => t.IsCompleted);
        return (double)completed / _tasks.Count * 100;
    }

    /// <summary>Prints counts and the completion percentage.</summary>
    public void DisplayStatistics()
    {
        int completed = _tasks.Count(t => t.IsCompleted);
        int overdue = _tasks.Count(t => t.GetStatus() == "Overdue");
        int pending = _tasks.Count(t => t.GetStatus() == "Pending");

        Console.WriteLine("\n--- Statistics ---");
        Console.WriteLine($"Total tasks : {_tasks.Count}");
        Console.WriteLine($"Completed   : {completed}");
        Console.WriteLine($"Pending     : {pending}");
        Console.WriteLine($"Overdue     : {overdue}");
        Console.WriteLine($"Completion  : {GetCompletionPercentage():F1}%");
    }

    /// <summary>
    /// Shows a reminder for each unfinished task that is due within the
    /// next 2 days or already overdue. Uses the abstract Reminder class
    /// through its derived TaskReminder class (POLYMORPHISM).
    /// </summary>
    public void ShowReminders()
    {
        Console.WriteLine("\n--- Reminders ---");
        bool anyShown = false;

        foreach (TaskItem task in _tasks)
        {
            if (task.IsCompleted)
            {
                continue; // skip finished tasks and go to the next one
            }

            // The reminder starts 2 days before the due date.
            Reminder reminder = new TaskReminder(task, task.DueDate.AddDays(-2));

            if (reminder.IsDue(DateTime.Now))
            {
                reminder.Show();
                anyShown = true;
            }
        }

        if (!anyShown)
        {
            Console.WriteLine("No reminders right now.");
        }
    }

    // ----- Private helpers -----

    /// <summary>Saves the current list to the file.</summary>
    private void Save()
    {
        FileStorage.Save(_tasks, _filePath);
    }

    /// <summary>Prints a list of tasks as an aligned table (LOOP over the list).</summary>
    private static void PrintTable(List<TaskItem> tasks, string emptyMessage)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine($"\n{emptyMessage}");
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"{"ID",-4}{"Title",-28}{"Priority",-10}{"Due",-12}{"Status",-10}");
        Console.WriteLine(new string('-', 64));

        foreach (TaskItem task in tasks)
        {
            // Shorten long titles so the table stays aligned.
            string title = task.Title.Length > 26 ? task.Title.Substring(0, 23) + "..." : task.Title;

            // In an interpolation hole the alignment (,-12) comes BEFORE the format (:yyyy-MM-dd).
            Console.WriteLine(
                $"{task.Id,-4}{title,-28}{task.Priority.Label,-10}{task.DueDate,-12:yyyy-MM-dd}{task.GetStatus(),-10}");
        }
    }
}
