using System;

class LoggerPool : IDisposable
{
    private bool _isDisposed = false;

    private Dictionary<string, ILogger> _loggerDictionary;

    public LoggerPool()
    {
        _loggerDictionary = new Dictionary<string, ILogger>();
    }

    public void AddLogger(string loggerName, ILogger logger)
    {
        _loggerDictionary[loggerName] = logger;
    }

    public void RemoveLogger(string loggerName)
    {
        if (_loggerDictionary.ContainsKey(loggerName))
        {
            _loggerDictionary[loggerName].Dispose();
            _loggerDictionary.Remove(loggerName);
        }

    }

    public ILogger GetLogger(string loggerName)
    {
        return _loggerDictionary[loggerName];
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            foreach (ILogger logger in _loggerDictionary.Values)
            {
                logger.Dispose();
            }
            _isDisposed = true;
        }
    }
}
