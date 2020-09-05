using System;
using System.Collections.Generic;
using System.Text;

namespace testConsole_dotNET.Logging
{
    class Output
    {
        internal static void Log(string message)
        {
            Console.WriteLine(message);
        }

        internal static void LogTestStart(string testName)
        {
            Log(testName + "========================================");
        }
    }
}
