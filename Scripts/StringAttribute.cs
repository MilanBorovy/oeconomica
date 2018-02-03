using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oeconomica
{
    //General purpose string attribute for enumerators
    class StringAttribute : Attribute
    {
        internal string String;

        public StringAttribute(string String)
        {
            this.String = String;
        }
    }
}
