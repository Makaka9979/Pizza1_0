using System;

namespace Libs
{
    class FileLogger : ILogger
    {
        private bool _isDisposed = false;

        private StreamWriter _writer;

        public FileLogger(string path)
        {
            _writer = new StreamWriter(path, true);
        }

        public void Debug(string description)
        {
            _writer.WriteLineAsync("DEBUG::" + description);
        }

        public void Info(string description)
        {
            _writer.WriteLineAsync("INFO::" + description);
        }

        public void Warning(string description)
        {
            _writer.WriteLineAsync("WARNING::" + description);
        }

        public void Error(string description)
        {
            _writer.WriteLineAsync("ERROR::" + description);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _writer.Dispose();
                _isDisposed = true;
            }
        }
    }
}