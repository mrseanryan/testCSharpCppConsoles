using System;
using System.Collections.Generic;
using System.Text;
using testConsole_dotNET.Logging;

namespace testConsole_dotNET.Inheritance
{
    internal abstract class Base
    {
        private string name;

        internal Base()
        {
            Output.Log("Base.Base()");
        }

        public Base(string name)
            : this()
        {
            this.name = name;

            Output.Log("Base.Base(string)");
        }

        internal abstract void Initialize();
    }
}
