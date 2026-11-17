using System;

namespace PersonalTaskManager.Models;

/// <summary>
/// INHERITANCE DEMO: TaskReminder inherits from (extends) the abstract
/// Reminder class. In C# inheritance uses a colon ":" where Java would use
/// the "extends" keyword.
/// </summary>
public class TaskReminder : Reminder
{
    /// <summary>The task this reminder is about.</summary>
    public TaskItem Task { get; }

    /// <summary>
    /// ": base(remindAt)" calls the parent (Reminder) constructor first,
    /// so the RemindAt value is set by the base class.
    /// </summary>
    public TaskReminder(TaskItem task, DateTime remindAt) : base(remindAt)
    {
        Task = task;
    }

    /// <summary>
    /// OVERRIDE of the abstract method. The message depends on how close
    /// the task is to its due date (CONDITIONALS).
    /// </summary>
    public override string BuildMessage()
    {
        int days = Task.DaysUntilDue();

        if (days < 0)
        {
            return $"OVERDUE by {-days} day(s): \"{Task.Title}\" (priority {Task.Priority})";
        }
        else if (days == 0)
        {
            return $"Due TODAY: \"{Task.Title}\" (priority {Task.Priority})";
        }
        else
        {
            return $"Due in {days} day(s): \"{Task.Title}\" (priority {Task.Priority})";
        }
    }

    /// <summary>
    /// OVERRIDE of the virtual method: adds a clock icon-style prefix and
    /// then reuses the parent's behavior with "base.Show()".
    /// </summary>
    public override void Show()
    {
        Console.Write("(!) ");
        base.Show();
    }
}
