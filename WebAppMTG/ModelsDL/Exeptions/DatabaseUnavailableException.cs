using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppMTGModelsDL.Exceptions
{
    public class DatabaseUnavailableException : Exception
    {
        public DatabaseUnavailableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

