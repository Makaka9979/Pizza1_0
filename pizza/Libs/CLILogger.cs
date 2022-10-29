namespace Libs
{
    class CLILogger : ILogger
    {
        public void Debug(string description)
        {
            Console.WriteLine("DEBUG::" + description);
        }

        public void Info(string description)
        {
            Console.WriteLine("INFO::" + description);
        }

        public void Warning(string description)
        {
            Console.WriteLine("WARNING::" + description);
        }

        public void Error(string description)
        {
            Console.WriteLine("ERROR::" + description);
        }

        public void Dispose() { }
    }
}