using Libs;

class Program
{
    async static Task Main(string[] args) 
    {
        try
        {
            string fileAddress = "infoLog.log";

            var menuController = new Controller.MenuController();
            ControllerRegistry.Add("/start", menuController);
            ControllerRegistry.Add("Головне меню", menuController);
            ControllerRegistry.Add("Контакти", menuController);

            var backetController = new Controller.BacketController();
            ControllerRegistry.Add("Меню", backetController);
            ControllerRegistry.Add("⏪ Назад", backetController);
            ControllerRegistry.Add("⏩ Вперед", backetController);
            ControllerRegistry.Add("➕ Додати до кошика", backetController);
            ControllerRegistry.Add("Корзина", backetController);
            ControllerRegistry.Add("Очистити", backetController);
            ControllerRegistry.Add("✅Замовити", backetController);
            ControllerRegistry.Add("🚫Замовити", backetController);
            ControllerRegistry.Add("Додати комментарiй", backetController);
            ControllerRegistry.Add("🍪Готово", backetController);

            LoggerRegistry.AddLogger("cli", new CLILogger());
            LoggerRegistry.AddLogger("file", new FileLogger(fileAddress));

            Bot.Telegram bot = new Bot.Telegram();

            string now = $" START [{DateTime.Now}]\n";
            LoggerRegistry.GetLogger("cli").Info(now);
            LoggerRegistry.GetLogger("file").Info(now);

            bot.Run();
            Console.ReadLine();
        }
        finally
        {
            LoggerRegistry.GetLogger("file").Info($"\n STOP [{DateTime.Now}]\n");
            LoggerRegistry.Clear();
        }
    }
}
//logger.GetLogger("file").Info($"[{DateTime.Now}] #{message.Chat.Id}_[{message.MessageId}] '{message.Text}'");