class Program
{
    async static Task Main(string[] args)
    {
        Bot.Telegram bot = new();
        bot.Start();
        Console.ReadLine();
    }
}
//