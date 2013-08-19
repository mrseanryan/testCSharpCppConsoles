using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using testConsole_dotNET.Logging;

namespace testConsole_dotNET
{
    class DateTimeTests
    {
        internal void Go()
        {
            testDateTimeToString();
        }

        private void testDateTimeToString()
        {
            Output.LogTestStart("testDateTimeToString");

            Output.Log("DateTime: " + DateTime.Now.ToString());
        }
    }
}
