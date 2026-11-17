using System.Runtime.InteropServices;

namespace PersonalTaskManager.Models;

/// <summary>
/// UNION DEMO (Unions requirement).
/// C# has no "union" keyword like C does, but the same effect is possible with
/// an explicit struct layout: every field below starts at byte offset 0, so
/// they all SHARE THE SAME 4 BYTES of memory.
/// Writing to one field changes how the other fields read that memory.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public struct NumericValue
{
    /// <summary>The 4 bytes interpreted as a 32-bit integer.</summary>
    [FieldOffset(0)]
    public int AsInt;

    /// <summary>The same 4 bytes interpreted as a 32-bit floating point number.</summary>
    [FieldOffset(0)]
    public float AsFloat;
}
