using System;

namespace PersonalTaskManager.Models;

/// <summary>
/// ABSTRACT CLASS DEMO (Inheritance requirement).
/// An abstract class cannot be created directly with "new Reminder()".
/// It defines what every reminder has in common and forces derived classes
/// (such as TaskReminder) to supply the missing pieces.
/// </summary>
public abstract class Reminder
{
    /// <summary>Moment when the reminder should start appearing.</summary>
    public DateTime RemindAt { get; }

    /// <summary>
    /// Protected constructor: only derived classes can call it
    /// (using ": base(...)" in their own constructor).
    /// </summary>
    protected Reminder(DateTime remindAt)
    {
        RemindAt = remindAt;
    }

    /// <summary>
    /// A normal (concrete) method shared by all reminders.
    /// EXPRESSION: compares the current time with the reminder time.
    /// </summary>
    public bool IsDue(DateTime now)
    {
        return now >= RemindAt;
    }

    /// <summary>
    /// ABSTRACT method: has no body here. Every derived class MUST
    /// override it and provide its own message.
    /// </summary>
    public abstract string BuildMessage();

    /// <summary>
    /// VIRTUAL method: has a default behavior, but derived classes MAY
    /// override it to change how the reminder is displayed.
    /// </summary>
    public virtual void Show()
    {
        Console.WriteLine($"[Reminder] {BuildMessage()}");
    }
}
