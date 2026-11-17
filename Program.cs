using System;
using System.Globalization;
using PersonalTaskManager.Models;
using PersonalTaskManager.Services;

namespace PersonalTaskManager;

/// <summary>
/// Entry point of the Personal Task Manager. This file only handles the menu
/// and reading input from the user; the task logic lives in Services/TaskManager.cs.
/// </summary>
internal class Program
{
    private static void Main()
    {
        // VARIABLES: where tasks are saved and the object that manages them.
        string filePath = System.IO.Path.Combine("data", "tasks.json");
        var manager = new TaskManager(filePath);

        Console.WriteLine("=== Personal Task Manager ===");
        Console.WriteLine($"Loaded {manager.Count} saved task(s).");

        // Show reminders right away so overdue / upcoming tasks are not missed.
        manager.ShowReminders();

        // LOOP: the menu repeats until the user chooses to exit.
        bool running = true;
        while (running)
        {
            PrintMenu();
            string choice = ReadText("Choose an option: ");

            // CONDITIONAL: switch picks the action for the chosen option.
            switch (choice)
            {
                case "1":
                    AddTask(manager);
                    break;
                case "2":
                    manager.DisplayTasks();
                    break;
                case "3":
                    CompleteTask(manager);
                    break;
                case "4":
                    manager.DisplayOverdueTasks();
                    break;
                case "5":
                    manager.OrganizeTasks();
                    Console.WriteLine("\nTasks organized: pending first, by due date, then priority.");
                    manager.DisplayTasks();
                    break;
                case "6":
                    manager.DisplayStatistics();
                    break;
                case "7":
                    manager.ShowReminders();
                    break;
                case "8":
                    RunStructAndUnionDemo();
                    break;
                case "9":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please enter a number from 1 to 9.");
                    break;
            }
        }

        Console.WriteLine("\nTasks are saved. Goodbye!");
    }

    /// <summary>Prints the list of menu options.</summary>
    private static void PrintMenu()
    {
        Console.WriteLine("\n--- Menu ---");
        Console.WriteLine("1. Add task");
        Console.WriteLine("2. Show all tasks");
        Console.WriteLine("3. Complete task");
        Console.WriteLine("4. Show overdue tasks");
        Console.WriteLine("5. Organize tasks (sort)");
        Console.WriteLine("6. Show statistics");
        Console.WriteLine("7. Show reminders");
        Console.WriteLine("8. Struct and union demo");
        Console.WriteLine("9. Save and exit");
    }

    /// <summary>Asks for the details of a new task and adds it.</summary>
    private static void AddTask(TaskManager manager)
    {
        string title = ReadNonEmptyText("Task title: ");
        Priority priority = ReadPriority();
        DateTime dueDate = ReadDate("Due date (yyyy-MM-dd): ");

        TaskItem task = manager.AddTask(title, priority, dueDate);
        Console.WriteLine($"\nAdded task #{task.Id}: {task.Title} (due {task.DueDate:yyyy-MM-dd}).");
    }

    /// <summary>Shows the tasks, asks for an id and marks that task completed.</summary>
    private static void CompleteTask(TaskManager manager)
    {
        manager.DisplayTasks();

        if (manager.Count == 0)
        {
            return; // nothing to complete
        }

        int id = ReadInt("\nEnter the id of the task to complete: ");

        if (manager.CompleteTask(id))
        {
            Console.WriteLine("Task marked as completed. Nice work!");
        }
        else
        {
            Console.WriteLine("No pending task found with that id.");
        }
    }

    /// <summary>
    /// Demonstrates the struct (Priority) and the union (NumericValue).
    /// Run this option while recording the video to explain both concepts.
    /// </summary>
    private static void RunStructAndUnionDemo()
    {
        Console.WriteLine("\n--- Struct demo: Priority (value type) ---");

        // LOOP: for loop over the three priority levels.
        for (int level = 1; level <= 3; level++)
        {
            var p = new Priority(level);
            Console.WriteLine($"Level {p.Level} = {p.Label}");
        }

        // Values outside the range are corrected by the struct's constructor.
        var tooHigh = new Priority(99);
        Console.WriteLine($"Priority(99) is corrected to: {tooHigh.Label}");

        Console.WriteLine("\n--- Union demo: NumericValue (shared memory) ---");

        // Store a float, then read the SAME 4 bytes as an int.
        var number = new NumericValue { AsFloat = 1.0f };
        Console.WriteLine($"AsFloat = {number.AsFloat}  ->  AsInt = {number.AsInt} (the float's raw bits)");

        // Store an int, then read the SAME 4 bytes as a float.
        number.AsInt = 1078530011;
        Console.WriteLine($"AsInt = {number.AsInt}  ->  AsFloat = {number.AsFloat.ToString(CultureInfo.InvariantCulture)}");

        // Structs are copied on assignment, so changing the copy leaves the original alone.
        NumericValue copy = number;
        copy.AsInt = 0;
        Console.WriteLine($"After changing the copy: original AsInt = {number.AsInt}, copy AsInt = {copy.AsInt}");
    }

    // ----- Input helpers (FUNCTIONS that keep asking until the input is valid) -----

    /// <summary>Reads a line of text. Returns "" if the input stream ends.</summary>
    private static string ReadText(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    /// <summary>Keeps asking until the user types something that is not empty.</summary>
    private static string ReadNonEmptyText(string prompt)
    {
        string text = ReadText(prompt);

        while (text.Length == 0)
        {
            Console.WriteLine("This cannot be empty.");
            text = ReadText(prompt);
        }

        return text;
    }

    /// <summary>Keeps asking until the user types a whole number.</summary>
    private static int ReadInt(string prompt)
    {
        int value;

        while (!int.TryParse(ReadText(prompt), out value))
        {
            Console.WriteLine("Please enter a valid number.");
        }

        return value;
    }

    /// <summary>Asks for a priority from 1 to 3 and returns it as a Priority struct.</summary>
    private static Priority ReadPriority()
    {
        int level = ReadInt("Priority (1 = Low, 2 = Medium, 3 = High): ");

        while (level < 1 || level > 3)
        {
            Console.WriteLine("Priority must be 1, 2 or 3.");
            level = ReadInt("Priority (1 = Low, 2 = Medium, 3 = High): ");
        }

        return new Priority(level);
    }

    /// <summary>Keeps asking until the user types a valid date such as 2026-12-31.</summary>
    private static DateTime ReadDate(string prompt)
    {
        DateTime date;

        while (!DateTime.TryParseExact(
                   ReadText(prompt), "yyyy-MM-dd",
                   CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            Console.WriteLine("Please use the format yyyy-MM-dd, for example 2026-12-31.");
        }

        return date;
    }
}
