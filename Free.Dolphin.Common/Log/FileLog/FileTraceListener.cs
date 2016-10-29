using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Free.Dolphin.Core
{
    internal class FileTraceListener : TraceListener
    {
        LogFileStream _logFileStream = new LogFileStream();

        public override void Write(string message)
        {
            _logFileStream.Write(message);
        }

        public override void WriteLine(string message)
        {
            _logFileStream.WriteLine(message);
        }

    }
}
