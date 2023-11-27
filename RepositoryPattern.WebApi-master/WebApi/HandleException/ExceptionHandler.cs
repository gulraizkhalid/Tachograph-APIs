using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.HandleException
{
    public class ExceptionHandler : Exception
    {
        public ExceptionHandler() : base()
        {
        }

        public ExceptionHandler(string message) : base(message)
        {
        }

        public ExceptionHandler(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
