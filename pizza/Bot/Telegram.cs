using Telegram.Bot;
using Telegram.Bot.Types;
using System.Collections;
using Libs;

namespace Bot
{
    internal class Telegram
    {
        private TelegramBotClient _client;
        private Model.User user = new();

        public Telegram()
        {
            string _token = "5529174269:AAEohLr_pkez9C5pYt_SHeO_nLNFZFVTLG4";
            _client = new TelegramBotClient(_token);
        }
        public void Run() 
        {
            _client.StartReceiving(Update, ErrorMessage);
        }
        public async Task HandleDeliveryData(ITelegramBotClient _client, Update update)
        {
            string message = update.Message.Text;
            user = (Model.User)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"];
            switch ((int)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userPage"])
            {
                case 0:
                    {
                        await _client.SendTextMessageAsync(
                            chatId: update.Message.Chat.Id,
                            text: "Як до тебе звертатися?",
                            replyMarkup: Keyboard.other);
                        Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userPage"] = (object)((int)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userPage"]+1);
                        break;
                    }
                case 1:
                    {
                        user.name = message;
                        await _client.SendTextMessageAsync(
                            chatId: update.Message.Chat.Id,
                            text: "На який номер телефону я можу тобi зателефонувати?",
                            replyMarkup: Keyboard.other);
                        Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userPage"] = (object)((int)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userPage"] + 1);
                        Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"] = (object)user;
                        break;
                    }
                case 2:
                    {
                        user.phoneNumber = message;
                        await _client.SendTextMessageAsync(
                            chatId: update.Message.Chat.Id,
                            text: "Куди менi везти твоє замовлення?",
                            replyMarkup: Keyboard.other);
                        Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userPage"] = (object)((int)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userPage"] + 1);
                        Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"] = (object)user;
                        break;
                    }
                case 3:
                    {
                        user.deliveryAdress = message;
                        await _client.SendTextMessageAsync(
                            chatId: update.Message.Chat.Id,
                            text: "Чи все вiрно?",
                            replyMarkup: Keyboard.ifAllGood);
                        await _client.SendTextMessageAsync(
                                chatId: update.Message.Chat.Id,
                                text: user.GetShortUserString());
                        Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userPage"] = (object)((int)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userPage"] + 1);
                        Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"] = (object)user;
                        break;
                    }
                case 4:
                    {
                        if (message == "Yes")
                        {
                            user.readyToOrder = true;
                            user.ifHaveCommand = false;
                            await _client.SendTextMessageAsync(
                            chatId: update.Message.Chat.Id,
                            text: "Супер!",
                            replyMarkup: Keyboard.order);

                        }
                        else if (message == "No")
                        {
                            await _client.SendTextMessageAsync(
                                chatId: update.Message.Chat.Id,
                                text: "Дуже шкода.",
                                replyMarkup: Keyboard.other);
                        }
                        Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"] = (object)user;
                        Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userPage"] = (object)(-1);
                        break;
                    }
                case 9:
                    {
                        user = ((Model.User)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]);
                        user.comment = message;
                        user.ifHaveCommand = true;
                        Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"] = (object)user;
                        await _client.SendTextMessageAsync(
                            chatId: update.Message.Chat.Id,
                            text: ($"{((Model.User)SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).order}" +
                            $"\n----------\n" +
                            $"{((Model.User)SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).ToString()}"),
                            replyMarkup: Bot.Keyboard.ifComment1);
                        Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userPage"] = (object)(-1);
                        break;
                    }
                default:
                    break;
            }
        }
        private async Task UserInformationDelivery(Update update)
        {
            var message = update.Message;
            if (!Keyboard.greenCardCommandsList.Contains(message.Text))
            {
                if (message.Text == "⁉️Додати iнформацiю про доставку")
                {
                    Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userPage"] = (object)(0);
                }
                else if (message.Text == "✅Додати iнформацiю про доставку")
                {
                    await _client.SendTextMessageAsync(
                            chatId: update.Message.Chat.Id,
                            text: user.GetShortUserString(),
                            replyMarkup: Keyboard.dataUserClear);
                    return;
                }
                else if (message.Text == "Змiнити iнформацiю")
                {
                    ((Model.User)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).Clear();
                    Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userPage"] = (object)(0);
                }
                await HandleDeliveryData(_client, update);
                return;
            }
            else
            {
                Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userPage"] = (object)(-1);
            }
        }
        private async Task Update(ITelegramBotClient botClient, Update update, CancellationToken botToken)
        {
            //..............................
            Console.WriteLine($"_info: /{DateTime.UtcNow}/ /{update.Message.Chat.Id}/ /{update.Message.Text}/");
            //..............................
            var message = update.Message;
            if (message == null)
                return;
            if (message.Text == null)
                return;
            if (!Libs.SessionRegistry.Sessions.ContainsKey(message.Chat.Id))
            {
                Libs.Session session = new();
                ArrayList orders = new ArrayList();
                session.State.Add("orders", (object)orders);
                session.State.Add("currentPage", (object)0);
                session.State.Add("userPage", (object)(-1));
                session.State.Add("allPriseOrder", (object)(0));
                user.readyToOrder = false;
                user.userId = message.Chat.Id;
                session.State.Add("userInformation", (object)user);
                Libs.SessionRegistry.Sessions.Add(message.Chat.Id, session);
            } 
            Thread.Sleep(100);
            Libs.LoggerRegistry.GetLogger("file").Info($"[{DateTime.Now}] #{message.Chat.Id}_[{message.MessageId}] '{message.Text}'");

            await UserInformationDelivery(update);
            Libs.ControllerRegistry.Get(message.Text)?.Run(_client, update);
        }

        private static Task ErrorMessage(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Libs.LoggerRegistry.GetLogger("cli").Warning($"[{DateTime.Now}] {exception.Message}");
            Libs.LoggerRegistry.GetLogger("file").Warning($"[{DateTime.Now}] {exception.Message}");
            return Task.CompletedTask;
        }
    }
}
