using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testConsole_dotNET
{
    class EnumTests
    {
        enum Colours
        {
            Unset = 0,
            Red,
            Orange,
            Yellow,
            Green,
            Blue,
            Indigo,
            Violet
        }

        internal void Go()
        {
            if ((int)Colours.Red != 1)
            {
                throw new ApplicationException("Expected compiler to add 1 to previous enum value");
            }

            //enum is a value type, so cannot be null:
            //Colours colour = null;

            Nullable<Colours> nullableColour = null;
        }
    }
}
