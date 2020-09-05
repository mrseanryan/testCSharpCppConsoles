using System;
using System.Collections.Generic;
using System.Text;

namespace testConsole_dotNET
{
    class Program
    {
        static void Main(string[] args)
        {
            TestEventThreads test = new TestEventThreads();
            test.Go();

            Console.WriteLine("Press ENTER to finish");
            Console.ReadLine();
        }
    }
}
