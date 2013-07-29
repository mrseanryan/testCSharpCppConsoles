using System;
using System.Collections.Generic;
using System.Text;
using testConsole_dotNET.Logging;

namespace testConsole_dotNET.Inheritance
{
    internal class Derived1 : Base
    {
        internal Derived1()
            : base()
        {
            Output.Log("Derived1()");

            Initialize();
        }

        internal Derived1(string name)
            : base(name)
        {
            Output.Log("Derived1(string)");

            Initialize(); //question: can we remove this ?  answer: NO.  base(string) calls base() but Derived1() is NOT called.
        }

        internal override void Initialize()
        {
            Output.Log("Derived1.Initialize()");
        }
    }
}
