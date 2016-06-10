using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Free.Dolphin.Common
{
    public static class LogManager
    {
        static Log _log = new Log();
        public static Log Log
        {
            get { return _log; }
        }
        static LogManager()
        {
            Trace.Listeners.Add(new FileTraceListener());
        }
    }


}
