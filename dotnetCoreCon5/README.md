# dotnetCoreCon5 README

A console built on .NET Core 5.

Uses some C# 9 features, such as:

- records
- one-line type declaration
- top-level statements

The console is a simple ATM emulator, implemented as an immutable 'mostly FP' state machine.

## My Learnings

What I learned:

Records do not force code to be immutable (you can still have 'set' on properties for example) BUT it _requires less coding_ than the `class` equivalent, because:

- records behave like values not references: `recA = recB` will copy recB's values to the separate `recA`
- records have a built-in copy constructor
- records can be declared in 1 line of code

## References

https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9

https://devblogs.microsoft.com/dotnet/c-9-0-on-the-record/

https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8
