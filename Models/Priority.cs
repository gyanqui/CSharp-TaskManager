using System;

namespace PersonalTaskManager.Models;

/// <summary>
/// STRUCT DEMO (Structures requirement).
/// A struct is a VALUE type: it is copied when assigned or passed to a method,
/// unlike a class, which is a REFERENCE type. It is a good fit for small,
/// simple values such as a task priority.
/// "readonly" means the value can never change after it is created.
/// </summary>
public readonly struct Priority
{
    /// <summary>Numeric level: 1 = Low, 2 = Medium, 3 = High.</summary>
    public int Level { get; }

    /// <summary>
    /// Creates a priority. Values outside 1-3 are forced into that range
    /// (Math.Clamp) so an invalid priority can never exist.
    /// </summary>
    public Priority(int level)
    {
        Level = Math.Clamp(level, 1, 3);
    }

    /// <summary>Human-readable name of the level (uses a switch expression).</summary>
    public string Label => Level switch
    {
        1 => "Low",
        2 => "Medium",
        _ => "High"
    };

    /// <summary>Lets the struct print nicely, e.g. in string interpolation.</summary>
    public override string ToString() => Label;
}
