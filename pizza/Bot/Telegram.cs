using System;
using System.IO;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections;
using Telegram.Bot.Types.Enums;

namespace Bot
{
    internal class Telegram
    {
        private string _token = "";
        private int _condition = 0;
        private TelegramBotClient _client;
        private static LoggerPool logger;
        private ArrayList _menu;
        private ArrayList addOrder = new ArrayList();
        Controller.Client user;
        private byte countBasket = 0;

        private static long admin_id = 562489554;

        private string contactFName = "Пiцца Харкiв";
        private static string contactPhone = "+111111111111";
        private static string contact_email = "info@ct-college.net";
        private string vCardTg = $"BEGIN:VCARD\n VERSION:3.0\nN:Харків;Пiцца\nORG:Пiццерiя\n" +
            $"TEL;TYPE=voice,work,pref:{contactPhone}\nEMAIL:{contact_email}\nEND:VCARD";
        private double gpsLatitude = 49.999366f;
        private double gpsLongitude = 36.243200f;
        private string gpsTitle = "ВСП «ХКТФК НТУ «ХПI»";
        private string gpsAddress = "вулиця Манiзера, 4, Харкiв, Харкiвська область, Украина, 61000";

        private ReplyKeyboardMarkup index = new(new[] {
            new KeyboardButton[] { "Меню", "Корзина" },
            new KeyboardButton[] { "Контакти" }
        })
        { ResizeKeyboard = true };
        private ReplyKeyboardMarkup contact = new(new[] {
            new KeyboardButton[] { "Головне меню" }
        })
        { ResizeKeyboard = true };
        private ReplyKeyboardMarkup other = new(new[] {
            new KeyboardButton[] { "Головне меню" }
        })
        { ResizeKeyboard = false };
        private ReplyKeyboardMarkup ifAllGood = new(new[] {
            new KeyboardButton[] { "Yes", "No" }
        })
        { ResizeKeyboard = true };
        private ReplyKeyboardMarkup paymend = new(new[] {
            new KeyboardButton[] { "Картою", "Готiвкою" }
        })
        { ResizeKeyboard = true };
        private ReplyKeyboardMarkup basket = new(new[] {
            new KeyboardButton[] { "Замовити", "Очистити" },
            new KeyboardButton[] { "Головне меню" }
        })
        { ResizeKeyboard = true };
        private ReplyKeyboardMarkup menu = new(new[] {
            new KeyboardButton[] { "⏪ Назад", "⏩ Вперед" },
            new KeyboardButton[] { "➕ Додати до кошика" },
            new KeyboardButton[] { "Корзина", "Головне меню" }
        })
        { ResizeKeyboard = true };

        private async Task Menu(Message message)
        {
            if (_condition < 0) { _condition = 0; }
            if (_condition >= _menu.Count) { _condition = _menu.Count - 1; }
            string currentState = $"{_condition + 1} / {_menu.Count}";
            string text = text = $"<a><b>  {((Model.Menu)_menu[_condition]).Name}</b></a>\n" +
                        $"<a>{((Model.Menu)_menu[_condition]).Text}</a>\n\n" +
                        $"<i>Цiна:</i> <a>{((Model.Menu)_menu[_condition]).Price}</a>\n\n" +
                        $"<a>Сторiнка меню </a>[<b><i>{currentState}</i></b>]";

            Message sentMessage = await _client.SendPhotoAsync(
                        chatId: message.Chat.Id,
                        photo: ((Model.Menu)_menu[_condition]).Link,
                        caption: text,
                        parseMode: ParseMode.Html,
                        replyMarkup: menu);
            logger.GetLogger("file").Info($"[{DateTime.Now}] #{message.Chat.Id}_[{message.MessageId}] '{message.Text}'");
        }
        private string LogSaveUser(Message message)
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
            return userData + $"Id:'{message.Chat.Id}'\n";
        }
        public Telegram() { Console.WriteLine("class Telegram error"); }
        public Telegram(TelegramBotClient _client, string _token, LoggerPool loggerPool, ArrayList _menu, Controller.Client user)
        {
            this._client = _client;
            this._token = _token;
            this._client.StartReceiving(Update, ErrorMessage);
            logger = loggerPool;
            this._menu = _menu;
            this.user = user;
        }
        private async Task PlaceAnOrder(Message message)
        {
            Message sentMessage;
            if (countBasket == 1)
            {
                user.name = message.Text;
                countBasket++;
                sentMessage = await _client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Твiй номер телефону? (На нього зателефонує кур'єр)",
                    replyMarkup: other);
            }
            else if (countBasket == 2)
            {
                user.phoneNumber = message.Text;
                countBasket++;
                sentMessage = await _client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Залишилось ще трошки");
                Thread.Sleep(250);
                sentMessage = await _client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Яка адреса доставки?",
                    replyMarkup: other);
            }
            else if (countBasket == 3)
            {
                user.deliveryAdress = message.Text;
                countBasket++;
                sentMessage = await _client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Оплата картою чи готівкою?",
                    replyMarkup: paymend);
            }
            else if (countBasket == 5)
            {
                countBasket++;
                sentMessage = await _client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Твiй коментар (якщо немає, просто вiдправ точку)",
                    replyMarkup: other);
            }
            else if (countBasket == 6)
            {
                if (message.Text == ".")
                {
                    user.comment = "//";
                }
                else
                {
                    user.comment = message.Text;
                }
                sentMessage = await _client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: user.ToString());
                sentMessage = await _client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Чи все вiрно?",
                    replyMarkup: ifAllGood);
            }

            if (message.Text == "Картою")
            {
                user.payment = false;
                countBasket++;
            }
            else if (message.Text == "Готiвкою")
            {
                user.payment = true;
                countBasket++;
            }
            else if (message.Text == "Yes")
            {
                countBasket = 0;
                string data_time = $"{message.Chat.Id} {DateTime.Now}";
                user.data_time = data_time;
                sentMessage = await _client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Супер!");
                sentMessage = await _client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: $"Твоє замовлення:\n" +
                            $"{user.order}\n" +
                            $"Доставимо ->\n" +
                            $"{user.ToString()}\n" +
                            $"Змовлення номер: {user.data_time}");
                sentMessage = await _client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Вибирай що хочеш зробити",
                        replyMarkup: index);
                sentMessage = await _client.SendTextMessageAsync(
                            chatId: admin_id,
                            text: $"Замовлення {user.data_time}\n" +
                            $"{user.order}\n" +
                            $"Доставимо ->\n" +
                            $"{user.ToString()}");
            }
            else if (message.Text == "No")
            {
                countBasket = 0;
                sentMessage = await _client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Дуже шкода ):");
                Thread.Sleep(200);
                string listOrders = "";
                for (int i = 0; i < addOrder.Count - 1; i++)
                {
                    listOrders += $"{((Controller.Order)addOrder[i]).ToString()}\n";
                }
                sentMessage = await _client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: listOrders,
                            replyMarkup: basket);
            }
        }

        private async Task Update(ITelegramBotClient botClient, Update update, CancellationToken botToken)
        {
            var message = update.Message;
            if (message == null)
                return;
            if (message.Text == null)
                return;
            else
            {
                Message sentMessage;
                Thread.Sleep(250);
                if (message.Text == "/start")
                {
                    await _client.SendTextMessageAsync(message.Chat.Id, "А я тебе вже зачекався 🌚");
                    Thread.Sleep(250);
                    sentMessage = await _client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Вибирай що хочеш зробити",
                        replyMarkup: index);
                    logger.GetLogger("file").Info(LogSaveUser(message));
                }
                else if (message.Text == "Контакти")
                {
                    sentMessage = await _client.SendContactAsync(
                    chatId: message.Chat.Id,
                    phoneNumber: contactPhone,
                    firstName: contactFName,
                    vCard: vCardTg,
                    replyMarkup: contact);
                    Thread.Sleep(250);
                    sentMessage = await _client.SendVenueAsync(
                        chatId: message.Chat.Id,
                        latitude: gpsLatitude,
                        longitude: gpsLongitude,
                        title: gpsTitle,
                        address: gpsAddress);
                }
                else if (message.Text == "Меню")
                {
                    _condition = 0;
                    await Menu(message);
                }
                else if (message.Text == "⏪ Назад")
                {
                    _condition--;
                    await Menu(message);
                }
                else if (message.Text == "⏩ Вперед")
                {
                    _condition++;
                    await Menu(message);
                }
                else if (message.Text == "➕ Додати до кошика")
                {
                    addOrder.Add((Model.Menu)_menu[_condition]);
                    sentMessage = await _client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: $"Так, я добавив пiцу '{((Model.Menu)_menu[_condition]).Name}' до кошику");
                    _condition++;
                    await Menu(message);
                }
                else if (message.Text == "Корзина")
                {
                    if (addOrder.Count < 1)
                    {
                        sentMessage = await _client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Твiй кошик пустий.",
                        replyMarkup: basket);
                        /*} else {
                            string listOrders = "";
                            for (int i = 0; i < (addOrder.Count - 1); i++)
                            {
                                listOrders += $"{((Controller.Order)addOrder[i]).ToString()}\n";
                            }
                            user.order = listOrders;
                            sentMessage = await _client.SendTextMessageAsync(
                                        chatId: message.Chat.Id,
                                        text: listOrders,
                                        replyMarkup: basket); */
                    }
                }
                else if (message.Text == "Головне меню")
                {
                    sentMessage = await _client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Вибирай що хочеш зробити",
                        replyMarkup: index);
                }
                else if (message.Text == "Замовити")
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
                }
                else if (message.Text == "Очистити")
                {
                    addOrder.Clear();
                    sentMessage = await _client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Тепер твiй кошик пустий.",
                        replyMarkup: basket);
                }
                else { await PlaceAnOrder(message); }
                logger.GetLogger("file").Info($"[{DateTime.Now}] #{message.Chat.Id}_[{message.MessageId}] '{message.Text}'");
            }
        }
        private static Task ErrorMessage(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            string msg = "";
            try
            {
                msg = $"[{DateTime.Now}] {exception.Message}";
            }
            catch (NotImplementedException e)
            {
                msg = $"[{DateTime.Now}] {e.Message}";
            }
            logger.GetLogger("cli").Warning(msg);
            logger.GetLogger("file").Warning(msg);

            return Task.CompletedTask;
        }
    }
}
