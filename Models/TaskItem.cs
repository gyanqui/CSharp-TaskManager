using System;
using System.Text.Json.Serialization;

namespace PersonalTaskManager.Models;

/// <summary>
/// CLASS DEMO (Classes requirement).
/// Represents one task. The class is named TaskItem (not Task) because C#
/// already has System.Threading.Tasks.Task and the names would conflict.
/// The properties below are the VARIABLES that store each task's data.
/// </summary>
public class TaskItem
{
    // ----- Variables (stored data) -----

    /// <summary>Unique number used to select the task in the menu.</summary>
    public int Id { get; set; }

    /// <summary>Short description of the task.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Priority stored as a plain number (1-3) so it saves to JSON easily.
    /// Use the <see cref="Priority"/> property to get the struct version.
    /// </summary>
    public int PriorityLevel { get; set; } = 1;

    /// <summary>Date the task must be finished by.</summary>
    public DateTime DueDate { get; set; }

    /// <summary>True once the user marks the task as done.</summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Parameterless constructor. Required by System.Text.Json so it can
    /// create TaskItem objects when loading tasks from the file.
    /// </summary>
    public TaskItem() { }

    /// <summary>Constructor used when the user adds a new task.</summary>
    public TaskItem(int id, string title, Priority priority, DateTime dueDate)
    {
        Id = id;
        Title = title;
        PriorityLevel = priority.Level;
        DueDate = dueDate.Date; // keep only the date, drop the time of day
        IsCompleted = false;
    }

    /// <summary>
    /// The priority as a struct (value type). [JsonIgnore] keeps it out of the
    /// saved file, because PriorityLevel already stores the same information.
    /// </summary>
    [JsonIgnore]
    public Priority Priority => new Priority(PriorityLevel);

    /// <summary>
    /// CONDITIONALS requirement: decides if the task is Completed, Overdue
    /// or Pending using if / else if / else.
    /// </summary>
    public string GetStatus()
    {
        if (IsCompleted)
        {
            return "Completed";
        }
        else if (DueDate.Date < DateTime.Today) // EXPRESSION: comparing two dates
        {
            return "Overdue";
        }
        else
        {
            return "Pending";
        }
    }

    /// <summary>
    /// EXPRESSION: number of days until the due date.
    /// Negative means the due date has already passed.
    /// </summary>
    public int DaysUntilDue()
    {
        return (DueDate.Date - DateTime.Today).Days;
    }
}
