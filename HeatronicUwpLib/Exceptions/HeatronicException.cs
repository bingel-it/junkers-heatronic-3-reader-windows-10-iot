using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatronicUwpLib.Exceptions
{
    public class HeatronicException : Exception
    {
        public HeatronicException(String message) : base(message)
        {

        }
    }
}
