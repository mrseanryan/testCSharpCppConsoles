using System;
using System.Collections.Generic;
using System.Text;

namespace testConsole_dotNET
{
    class Program
    {
        static void Main(string[] args)
        {
            Test test = new Test();
            test.Go();

            Console.WriteLine("Press ENTER to finish");
            Console.ReadLine();
        }
    }
}
