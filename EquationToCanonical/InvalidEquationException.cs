using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationToCanonical
{
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
