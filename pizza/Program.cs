﻿using Libs;

class Program
{
    async static Task Main(string[] args) 
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

        LoggerRegistry.AddLogger("cli", new CLILogger());
        LoggerRegistry.AddLogger("file", new FileLogger(fileAddress));

        Bot.Telegram bot = new Bot.Telegram();
        
        string now = $" START [{DateTime.Now}]\n";
        LoggerRegistry.GetLogger("cli").Info(now);
        LoggerRegistry.GetLogger("file").Info(now);

        bot.Run();
        Console.ReadLine();

        now = $" STOP [{DateTime.Now}]\n";
        LoggerRegistry.GetLogger("file").Info(now);
    }
}
/*else if (message.Text == "Замовити")
                {
                    if (addOrder.Count < 1)
                    {
                        sentMessage = await _client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Твiй кошик пустий.",
                        replyMarkup: basket);
                    }
                    else
                    {
                        user.userId = message.Chat.Id;
                        countBasket = 1;
                        sentMessage = await _client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Як до тебе звертатися?",
                            replyMarkup: other);
                    }
                }*/

//logger.GetLogger("file").Info($"[{DateTime.Now}] #{message.Chat.Id}_[{message.MessageId}] '{message.Text}'");

/*
 *  if (message.Text == "/start")
                {
                    ControllerRegistry.Get("/start")?.Run(_client, update);
                }
                else if (message.Text == "Контакти")
                {
                    ControllerRegistry.Get("Контакти")?.Run(_client, update);
                }
                else if (message.Text == "Меню")
                {
                    ControllerRegistry.Get("Меню")?.Run(_client, update);
                }
                else if (message.Text == "⏪ Назад")
                {
                    ControllerRegistry.Get("⏪ Назад")?.Run(_client, update);
                }
                else if (message.Text == "⏩ Вперед")
                {
                    ControllerRegistry.Get("⏩ Вперед")?.Run(_client, update);
                }
                else if (message.Text == "➕ Додати до кошика")
                {
                    ControllerRegistry.Get("➕ Додати до кошика")?.Run(_client, update);
                }
                else if (message.Text == "Корзина")
                {
                    ControllerRegistry.Get("Корзина")?.Run(_client, update);
                }
                else if (message.Text == "Головне меню")
                {
                    ControllerRegistry.Get("Головне меню")?.Run(_client, update);
                }
                else if (message.Text == "Очистити")
                {
                    ControllerRegistry.Get("Очистити")?.Run(_client, update);
                }
*/