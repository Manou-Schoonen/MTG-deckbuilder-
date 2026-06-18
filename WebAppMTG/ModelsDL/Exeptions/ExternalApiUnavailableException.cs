using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppMTGModelsDL.Exeptions
{
    public class ExternalApiUnavailableException : Exception
    {
        public ExternalApiUnavailableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
