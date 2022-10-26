using Telegram.Bot;
using Telegram.Bot.Types;
using System.Collections;
using Telegram.Bot.Types.ReplyMarkups;
using Libs;

static class Keyboard
{
    //namespace Bot
    public static ReplyKeyboardMarkup other = new(new[] {
            new KeyboardButton[] { "Головне меню" }
    }) { ResizeKeyboard = true }; 
    public static ReplyKeyboardMarkup dataUserClear = new(new[] {
            new KeyboardButton[] { "Змiнити iнформацiю", "Головне меню" }
     }) { ResizeKeyboard = true }; 
    public static ReplyKeyboardMarkup ifAllGood = new(new[] {
            new KeyboardButton[] { "Yes", "No" },
            new KeyboardButton[] { "Головне меню" }
    }) { ResizeKeyboard = true }; 
    public static ReplyKeyboardMarkup order = new(new[] {
            new KeyboardButton[] { "✅Замовити", "Головне меню" }
    }) { ResizeKeyboard = true };

    //namespace Controller 
    public static ReplyKeyboardMarkup basket1 = new(new[] {
            new KeyboardButton[] { "🚫Замовити", "Очистити" },
            new KeyboardButton[] { "🚫Додати iнформацiю про доставку" },
            new KeyboardButton[] { "Головне меню" }
    }) { ResizeKeyboard = true };
    public static ReplyKeyboardMarkup basket2 = new(new[] {
            new KeyboardButton[] { "✅Замовити", "Очистити" },
            new KeyboardButton[] { "✅Додати iнформацiю про доставку" },
            new KeyboardButton[] { "Головне меню" }
    }) { ResizeKeyboard = true };
    public static ReplyKeyboardMarkup errorAddDeliveryInfo = new(new[] {
            new KeyboardButton[] { "🚫Додати iнформацiю про доставку" ,"Головне меню" }
    }) { ResizeKeyboard = true };
    public static ReplyKeyboardMarkup hotovo = new(new[] {
            new KeyboardButton[] { "🍪Готово", "Головне меню" }
    }) { ResizeKeyboard = true };
    public static ReplyKeyboardMarkup ifComment0 = new(new[] {
            new KeyboardButton[] { "Додати комментарiй", "🍪Готово" },
            new KeyboardButton[] { "Головне меню" }
    }) { ResizeKeyboard = true };
    public static ReplyKeyboardMarkup ifComment1 = new(new[] {
            new KeyboardButton[] { "🍪Готово" , "Головне меню" }
    }) { ResizeKeyboard = true };
    public static ReplyKeyboardMarkup menu = new(new[] {
            new KeyboardButton[] { "⏪ Назад", "⏩ Вперед" },
            new KeyboardButton[] { "➕ Додати до кошика" },
            new KeyboardButton[] { "Корзина", "Головне меню" }
    }) { ResizeKeyboard = true };

    public static ReplyKeyboardMarkup index = new(new[] {
            new KeyboardButton[] { "Меню", "Корзина" },
            new KeyboardButton[] { "Контакти" }
    }) { ResizeKeyboard = true };

    public static string[] greenCardCommandsList = { "Меню", "Корзина", "Контакти", "Головне меню", "⏪ Назад", "⏩ Вперед",
            "➕ Додати до кошика", "Очистити", "✅Замовити", "🚫Замовити", "Додати комментарiй", "🍪Готово" };

    public static string NewUserMsg(Message message)
    {
        string userData = "...\nNEW_USER:: ";
        if (message.Chat.LastName != null) { userData += $"LastName:'{message.Chat.LastName}'/, "; }
        if (message.Chat.FirstName != null) { userData += $"FirstName:'{message.Chat.FirstName}'/ "; }
        if (message.Chat.Username != null) { userData += $"Username:'{message.Chat.Username}'/ "; }
        if (message.Chat.LinkedChatId != null) { userData += $"LinkedChatId:'{message.Chat.LinkedChatId}'/ "; }
        if (message.Chat.Bio != null) { userData += $"Bio:'{message.Chat.Bio}'/ "; }
        if (message.Chat.Title != null) { userData += $"Title:'{message.Chat.Title}'/ "; }
        if (message.Chat.InviteLink != null) { userData += $"InviteLink:'{message.Chat.InviteLink}'/ "; }
        if (message.Chat.StickerSetName != null) { userData += $"StickerSetName:'{message.Chat.StickerSetName}'/ "; }
        if (message.Chat.Description != null) { userData += $"Description:'{message.Chat.Description}'/ "; }
        return (userData + $"Id:'{message.Chat.Id}'\n");
    }
}

namespace Bot
{
    internal class Telegram
    {
        private TelegramBotClient _client;
        private Model.User user = new();

        public Telegram()
        {
            string _token = "5529174269:AAFdFoselL-cnp7wt4EveCQ-cyMXxKNHJro";
            _client = new TelegramBotClient(_token); ;
        }
        public void Run() 
        {
            _client.StartReceiving(Update, ErrorMessage);
        }
        /*
        user -> Name
        user -> Phone
        user -> Delivery Adress
        user -> Comment
        all good?
         */
        public async void HandleDeliveryData(ITelegramBotClient _client, Update update)
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
                            text: ($"{((Model.User)SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).order}\n----------\n{((Model.User)SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).ToString()}"));
                        Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userPage"] = (object)(-1);
                        break;
                    }
                default:
                    break;
            }
        }
        private async void UserInformationDelivery(Update update)
        {
            var message = update.Message;
            if (!Keyboard.greenCardCommandsList.Contains(message.Text))
            {
                if (message.Text == "🚫Додати iнформацiю про доставку")
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
                HandleDeliveryData(_client, update);
                return;
            }
            else
            {
                Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userPage"] = (object)(-1);
            }
        }

        private async Task Update(ITelegramBotClient botClient, Update update, CancellationToken botToken)
        {
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
                session.State.Add("id", (object)message.Chat.Id);
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

            UserInformationDelivery(update);
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
