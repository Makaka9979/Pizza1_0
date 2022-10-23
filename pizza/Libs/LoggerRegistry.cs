using System;

namespace Libs
{
    public static class LoggerRegistry
    {
        private static Dictionary<string, ILogger> _loggerDictionary = new Dictionary<string, ILogger>();

        static LoggerRegistry()
        {
        }

        public static void AddLogger(string loggerName, ILogger logger)
        {
            _loggerDictionary[loggerName] = logger;
        }

        public static void RemoveLogger(string loggerName)
        {
            if (_loggerDictionary.ContainsKey(loggerName))
            {
                _loggerDictionary[loggerName].Dispose();
                _loggerDictionary.Remove(loggerName);
            }
        }

        public static ILogger GetLogger(string loggerName)
        {
            return _loggerDictionary[loggerName];
        }

        public static void Clear()
        {
            foreach (ILogger logger in _loggerDictionary.Values)
            {
                logger.Dispose();
            }
            _loggerDictionary.Clear();
        }
    }
}