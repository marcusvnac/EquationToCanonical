using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationToCanonical
{
    /// <summary>
    /// Exception raised when user inputs an invalid equation. 
    /// An equation is invalid when it doesn't have any variables.
    /// </summary>
    public class InvalidEquationException: Exception
    {
        public InvalidEquationException()
        {
        }

        public InvalidEquationException(string message)
        : base(message)
         {
        }

        public InvalidEquationException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
