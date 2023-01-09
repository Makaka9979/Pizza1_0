class Program
{
    async static Task Main(string[] args) 
    {
        try
        {
            string fileAddress = "infoLog.log";

            var menuController = new Controller.MenuController();
            Libs.ControllerRegistry.Add("/start", menuController);
            Libs.ControllerRegistry.Add("Головне меню", menuController);
            Libs.ControllerRegistry.Add("Контакти", menuController);

            var backetController = new Controller.BacketController();
            Libs.ControllerRegistry.Add("Меню", backetController);
            Libs.ControllerRegistry.Add("⏪ Назад", backetController);
            Libs.ControllerRegistry.Add("⏩ Вперед", backetController);
            Libs.ControllerRegistry.Add("➕ Додати до кошика", backetController);
            Libs.ControllerRegistry.Add("Корзина", backetController);
            Libs.ControllerRegistry.Add("Очистити", backetController);
            Libs.ControllerRegistry.Add("✅Замовити", backetController);
            Libs.ControllerRegistry.Add("🚫Замовити", backetController);
            Libs.ControllerRegistry.Add("Додати комментарiй", backetController);
            Libs.ControllerRegistry.Add("🍪Готово", backetController);

            Libs.LoggerRegistry.AddLogger("cli", new Libs.CLILogger());
            Libs.LoggerRegistry.AddLogger("file", new Libs.FileLogger(fileAddress));

            Bot.Telegram bot = new Bot.Telegram();

            string now = $" START [{DateTime.Now}]\n";
            Libs.LoggerRegistry.GetLogger("cli").Info(now);
            Libs.LoggerRegistry.GetLogger("file").Info(now);

            bot.Run();
            Console.ReadLine();
        }
        finally
        {
            Libs.LoggerRegistry.GetLogger("file").Info($"\n STOP [{DateTime.Now}]\n");
            Libs.LoggerRegistry.Clear();
        }
    }
}
//logger.GetLogger("file").Info($"[{DateTime.Now}] #{message.Chat.Id}_[{message.MessageId}] '{message.Text}'");