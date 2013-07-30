using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using testConsole_dotNET.Logging;

namespace testConsole_dotNET
{
    /// <summary>
    /// test the CodeAnalysis feature in VS2012
    /// </summary>
    public class CodeAnalysisTests
    {
        public void Go()
        {
            int result = MethodThatCouldOverflow(int.MaxValue);

            Output.Log("MethodThatCouldOverflow(" + int.MaxValue + ") = " + result);
        }

        //CA2233 - only detected, if method is *public* !
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "x+1")]
        public int MethodThatCouldOverflow(int x)
        {
            return x + 1;
        }

        //CA2233
        public static int Decrement(int input)
        {
            // Violates this rule            
            input--;
            return input;
        } 
    }
}
