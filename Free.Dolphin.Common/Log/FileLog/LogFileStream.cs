using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Free.Dolphin.Common
{
    internal class LogFileStream : IDisposable
    {
        private DateTime fileDate;
        private FileStream _fileStream;
        private StreamWriter _logStream;
        private readonly string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
        private const string fileName = "Log{0}.txt";
        public LogFileStream()
        {
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }
            InitFileStream();
        }

        private void InitFileStream()
        {
            fileDate = DateTime.Now;
            _fileStream = new FileStream(Path.Combine
                (logDir, string.Format(fileName, fileDate.ToString("yyyyMMdd"))),
                FileMode.Append, FileAccess.Write);
            _logStream = new StreamWriter(_fileStream, Encoding.Default) { AutoFlush = true };
        }

        public void WriteLine(string context)
        {
            CheckFileDate();
            _logStream.WriteLine(context);
        }

        public void Write(string context)
        {
            CheckFileDate();
            _logStream.Write(context);
        }

        private void CheckFileDate()
        {
            if (fileDate.ToString("yyyyMMdd") != DateTime.Now.ToString("yyyyMMdd"))
            {
                Close();
                InitFileStream();
            }
        }



        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Take object out the finalization queue to prevent finalization code for it from executing a second time.
        }

        private void Dispose(bool disposing)
        {
            if (this._disposed) return; // if already disposed, just return

            if (disposing) // only dispose managed resources if we're called from directly or in-directly from user code.
            {
                Close();
            }

            this._logStream = null;
            this._fileStream = null;

            _disposed = true;
        }


        private void Close()
        {
            _logStream.Close();
            _logStream.Dispose();
            _fileStream.Close();
            _fileStream.Dispose();

        }

        ~LogFileStream() { Dispose(false); } // finalizer called by the runtime. we should only dispose unmanaged objects and should NOT reference managed ones.
    }
}
