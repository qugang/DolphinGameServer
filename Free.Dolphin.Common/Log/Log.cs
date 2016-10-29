using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Globalization;

namespace Free.Dolphin.Core
{
    public class Log
    {
        static Log()
        {
        }
        internal Log()
        {

        }
        public void Error(string message)
        {
            WriteLog(Level.Error, message, null);
        }

        public void ErrorFormat(string message, params object[] args)
        {
            WriteLog(Level.Error, message, args);
        }

        public void Error(string message, Exception ex)
        {
            WriteLogExection(Level.Error, message, ex, null);
        }

        public void ErrorFormat(string message, Exception ex)
        {
            WriteLogExection(Level.Error, message, ex, null);
        }

        public void ErrorFormat(string message, Exception ex, params object[] args)
        {
            WriteLogExection(Level.Error, message, ex, args);
        }

        public void Warning(string message)
        {
            WriteLog(Level.Warning, message, null);
        }

        public void Warning(string message, Exception ex)
        {
            WriteLogExection(Level.Warning, message, ex, null);
        }

        public void WarningFormat(string message, params object[] args)
        {
            WriteLog(Level.Warning, message, args);
        }

        public void WarningFormat(string message, Exception ex)
        {
            WriteLogExection(Level.Warning, message, ex, null);
        }

        public void WarningFormat(string message, Exception ex, params object[] args)
        {
            WriteLogExection(Level.Warning, message, ex, args);
        }

        public void Debug(string message)
        {
            WriteLog(Level.Debug, message, null);
        }

        public void Debug(string message, Exception ex)
        {
            WriteLogExection(Level.Debug, message, ex, null);
        }

        public void DebugFormat(string message, params object[] args)
        {
            WriteLog(Level.Debug, message, args);
        }

        public void DebugFormat(string message, Exception ex)
        {
            WriteLogExection(Level.Debug, message, ex, null);
        }
        public void DebugFormat(string message, Exception ex, params object[] args)
        {
            WriteLogExection(Level.Debug, message, ex, args);
        }

        public void Info(string message)
        {
            WriteLog(Level.Info, message, null);
        }

        public void Info(string message, Exception ex)
        {
            WriteLogExection(Level.Info, message, ex, null);
        }

        public void InfoFormat(string message, params object[] args)
        {
            WriteLog(Level.Info, message, args);
        }

        public void InfoFormat(string message, Exception ex)
        {
            WriteLogExection(Level.Info, message, ex, null);
        }
        public void InfoFormat(string message, Exception ex, params object[] args)
        {
            WriteLogExection(Level.Info, message, ex, args);
        }
        private void WriteLog(Level level, string message, object[] args)
        {
            message = args == null ? message : string.Format(CultureInfo.InvariantCulture, message, args);

            string timeStamp = "[" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "]";

            lock (this) // we need this here until we seperate gs / moonet /raist
            {
                Trace.WriteLine(string.Format("{0}[{1}]: {2}", timeStamp, level.ToString().PadLeft(7), message));
            }
        }

        private void WriteLogExection(Level level, string message, Exception ex, object[] args)
        {
            message = args == null ? message : string.Format(CultureInfo.InvariantCulture, message, args);

            string timeStamp = "[" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "]";

            lock (this) // we need this here until we seperate gs / moonet /raist
            {
                Trace.WriteLine(string.Format("{0}[{1}]: {2} - [Exception] {3}", timeStamp, level.ToString().PadLeft(7), message, ex));
            }
        }
    }


    internal enum Level
    {
        Error,
        Warning,
        Debug,
        Info
    }
}
