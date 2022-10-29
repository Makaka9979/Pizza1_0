namespace Libs
{
    public static class SessionRegistry
    {
        static public Dictionary<long, Session> Sessions { get; } = new();
        static SessionRegistry() { }
    }
}