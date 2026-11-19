# Overview

I wanted to deepen my understanding of C# and the .NET platform by building a complete application from scratch rather than following a tutorial. This project is a console-based **Personal Task Manager** that lets a user add tasks with a title, priority, and due date, mark tasks as complete, view overdue tasks, organize the list, and track overall progress. Tasks are saved to a JSON file so they are still there the next time the program runs.

The program is built to exercise the core features of the C# language:

- **Variables and expressions:** task data is stored in typed properties, and the program calculates the completion percentage and compares due dates.
- **Conditionals and loops:** a menu loop drives the program, and conditionals decide whether each task is completed, pending, or overdue.
- **Functions and classes:** the logic is split into small methods across `TaskItem`, `TaskManager`, and `FileStorage` classes.
- **Structs and unions:** `Priority` is a `readonly struct` (a value type), and `NumericValue` uses an explicit struct layout to behave like a union, where two fields share the same memory.
- **Inheritance:** an `abstract` `Reminder` class defines shared behavior, and `TaskReminder` inherits from it and overrides its abstract and virtual methods.
- **File input and output:** tasks are written to and read from `data/tasks.json` using `System.Text.Json`.

My purpose in writing this software was to learn how C# organizes code with classes, namespaces, and value types, and to practice writing clear, well-documented code that is split into sensible files and folders.

[Software Demo Video](https://youtu.be/9eKV4opfvGE)

## Project Structure

```
CSharp-TaskManager/
├── Program.cs                 # Menu loop and user input
├── PersonalTaskManager.csproj
├── Models/
│   ├── TaskItem.cs            # Task class
│   ├── Priority.cs            # Struct (value type)
│   ├── NumericValue.cs        # Union demo using StructLayout
│   ├── Reminder.cs            # Abstract base class
│   └── TaskReminder.cs        # Derived class
├── Services/
│   ├── TaskManager.cs         # Add, display, complete, organize tasks
│   └── FileStorage.cs         # Save and load tasks as JSON
└── data/
    └── tasks.json             # Created automatically when the program runs
```

## How to Run

1. Install the [.NET SDK](https://dotnet.microsoft.com/download).
2. Clone this repository and open a terminal in its top folder.
3. Run:

```bash
dotnet run
```

# Development Environment

- **Editor:** Visual Studio Code with the C# Dev Kit extension
- **Version control:** Git and GitHub
- **Language:** C# running on the .NET 10 (SDK 10.0.400), built and run with the `dotnet` command-line tool
- **Libraries:** only the .NET standard library is used, mainly `System.Text.Json` for saving and loading tasks, `System.Linq` for sorting and counting, and `System.Runtime.InteropServices` for the union demonstration. No third-party packages are required.

# Useful Websites

- [C# Tutorials from Basics](https://www.w3schools.com/cs/index.php)
- [C# Documentation - Tour of C#](https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/)
- [Wikipedia - C Sharp (programming language)](https://en.wikipedia.org/wiki/C_Sharp_(programming_language))
- [Microsoft Learn - Structure types (struct)](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct)
- [Microsoft Learn - StructLayoutAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.structlayoutattribute)
- [Microsoft Learn - System.Text.Json overview](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview)
- [Markdown Guide - Cheat Sheet](https://www.markdownguide.org/cheat-sheet/)

# Future Work

- Add an option to edit or delete an existing task.
- Add task categories or tags, and a way to search and filter tasks.
- Let the user choose how many days before the due date a reminder appears.
- Add recurring tasks (daily, weekly, monthly).
- Add unit tests for `TaskManager` and `TaskItem`.
- Replace the console menu with a graphical or web interface.
