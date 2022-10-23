using System;

interface ILogger : IDisposable
{
    void Debug(string description);
    void Info(string description);
    void Warning(string description);
    void Error(string description);
}
