using System;
using System.Diagnostics;

static class StringExtensions
{
    public static void Dump(this string str)
    {
        Console.WriteLine(str);
        Trace.WriteLine(str); // Visual Studio - Output window
    }
}