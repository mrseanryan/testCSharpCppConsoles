using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using testConsole_dotNET.Logging;

namespace testConsole_dotNET
{
    class MathTests
    {
        internal void TestScientificNotation()
        {
            Output.LogTestStart("TestScientificNotation");

            double tenToThePowerOfMinusFifteen = 1e-15;

            Output.Log("tenToThePowerOfMinusFifteen = " + tenToThePowerOfMinusFifteen);
        }
    }
}
