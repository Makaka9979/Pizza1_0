using System.Collections;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Controller 
{
    public class BacketController : Libs.IController
    {
        private const ushort deliveryPrise = 59; //Незмінна ціна доставки піци.
        private const long admin_id = 562489554;

        private Model.User user;
        private ArrayList _menu = Bot.Keyboard.AddMenu();

        
        private async Task Menu(ITelegramBotClient _client, Message message, int _page)
        {
            if (_page < 0) 
            {
                _page = 0;
                Libs.SessionRegistry.Sessions[message.Chat.Id].State["currentPage"] = (object)_page;
            } 
            else if (_page >= _menu.Count) 
            { 
                _page = _menu.Count - 1;
                Libs.SessionRegistry.Sessions[message.Chat.Id].State["currentPage"] = (object)_page;
            }
            string currentState = $"{_page + 1} / {_menu.Count}";
            string text = text = $"<a><b>  {((Model.Menu)_menu[_page]).Name}</b></a>\n" +
                $"<a>{((Model.Menu)_menu[_page]).Text}</a>\n\n" +
                $"<i>Цiна:</i> <a>{((Model.Menu)_menu[_page]).Price}</a>\n\n" +
                $"<a>Сторiнка меню </a>[<b><i>{currentState}</i></b>]";
            await _client.SendPhotoAsync(
                chatId: message.Chat.Id,
                photo: ((Model.Menu)_menu[_page]).Link,
                caption: text,
                parseMode: ParseMode.Html,
                replyMarkup: Bot.Keyboard.menu);
        }
        public async void HandleAdd(ITelegramBotClient _client, Update update)
        {
            ((ArrayList)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["orders"]).Add((Model.Menu)_menu[(int)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["currentPage"]]);
             await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"Так, я добавив пiцу '{((Model.Menu)_menu[(int)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["currentPage"]]).Name}' до кошику");
            int allPriseOrder = (int)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["allPriseOrder"];
            allPriseOrder += ((Model.Menu)_menu[(int)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["currentPage"]]).Price;
            Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["allPriseOrder"] = (object)allPriseOrder;
            await Menu(_client, update.Message, (int)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["currentPage"]);
        }
        public async void HandleMenuNext(ITelegramBotClient _client, Update update)
        {
            Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["currentPage"] = (object)((int)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["currentPage"] + 1);
            await Menu(_client, update.Message, (int)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["currentPage"]);
        }
        public async void HandleMenuPrev(ITelegramBotClient _client, Update update)
        {
            Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["currentPage"] = (object)((int)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["currentPage"] - 1);
            await Menu(_client, update.Message, (int)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["currentPage"]);
        }
        public async void HandleMenu(ITelegramBotClient _client, Update update)
        {
            Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["currentPage"] = (object)0;
            await Menu(_client, update.Message, 0);
        }
        public async void HandleOrder(ITelegramBotClient _client, Update update)
        {
            if (((Model.User)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).readyToOrder)
            {
                if (!((Model.User)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).ifHaveCommand)
                {
                    await _client.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        text: ($"{((Model.User)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).order}\n----------\n{((Model.User)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).ToString()}"));
                    await _client.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        text: "Хочеш додати коментарiй?",
                        replyMarkup: Bot.Keyboard.ifComment0);
                }
                else
                {
                    await _client.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        text: ($"{((Model.User)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).order}\n----------\n{((Model.User)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).ToString()}"),
                        replyMarkup: Bot.Keyboard.ifComment1);
                }
            }
            else
            {
                await _client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "Спочатку заповни iнформацiю про доставку",
                    replyMarkup: Bot.Keyboard.errorAddDeliveryInfo);
            }
        }
        public async void HandleComment(ITelegramBotClient _client, Update update)
        {
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Що ти хочеш повiдомити менi?",
                replyMarkup: Bot.Keyboard.order);
            Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userPage"] = (object)(9);
        }
        public async Task HandleHotovo(ITelegramBotClient _client, Update update)
        {
            user = (Model.User)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"];
            user.Data_Time();
            Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"] = (object)user;
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"Твоє замовлення:\n{((Model.User)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).ThisOrder()}",
                replyMarkup: Bot.Keyboard.other);
            await _client.SendTextMessageAsync(
                chatId: admin_id,
                text: $"Нове замовлення:\n{((Model.User)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).ThisOrder()}",
                replyMarkup: Bot.Keyboard.other);
            ((Model.User)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).Clear();
            Libs.SessionRegistry.Sessions.Remove(update.Message.Chat.Id);
        }
        public async void HandleClear(ITelegramBotClient _client, Update update)
        {
            ((ArrayList)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["orders"]).Clear();
            Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["allPriseOrder"] = (object)0;
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Тепер твiй кошик пустий.",
                replyMarkup: Bot.Keyboard.other);
        }
        public async void HandleShowBacketContent(ITelegramBotClient _client, Update update)
        {
            if (((ArrayList)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["orders"]).Count < 1)
            {
                await _client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "Твiй кошик пустий.");
                await _client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "Меню:",
                    replyMarkup: Bot.Keyboard.other);
                await Menu(_client, update.Message, (int)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["currentPage"]);
                return;
            }
            string listOrders = "";
            for (int i = 0; i < ((ArrayList)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["orders"]).Count; i++)
            {
                listOrders += $"{((ArrayList)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["orders"])[i].ToString()}\n";
            }
            listOrders += $"\n Доставка, {deliveryPrise} грн\nВсього: {((int)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["allPriseOrder"]) + (int)deliveryPrise} гривень, 0 копійок";
            user = ((Model.User)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]);
            user.order = listOrders;
            Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"] = (object)user;

            if (((Model.User)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).readyToOrder)
            {
                await _client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: $"Твоє замовлення:\n{listOrders}",
                    replyMarkup: Bot.Keyboard.basket2);
            } 
            else
            {
                await _client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: $"Твоє замовлення:\n{listOrders}",
                    replyMarkup: Bot.Keyboard.basket1);
            }
        }
        public async void Run(ITelegramBotClient _client, Update update)
        {
            var message = update.Message.Text;
            if (message == "Меню")
                HandleMenu(_client, update);
            else if (message == "⏪ Назад")
                HandleMenuPrev(_client, update);
            else if (message == "⏩ Вперед")
                HandleMenuNext(_client, update);
            else if (message == "➕ Додати до кошика")
                HandleAdd(_client, update);
            else if (message == "Очистити")
                HandleClear(_client, update);
            else if (message == "Корзина")
                HandleShowBacketContent(_client, update);
            else if (message == "✅Замовити" || message == "🚫Замовити")
                HandleOrder(_client, update);
            else if (message == "Додати комментарiй")
                HandleComment(_client, update);
            else if (message == "🍪Готово")
            {
                if (((Model.User)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).readyToOrder)
                {
                    await HandleHotovo(_client, update);
                }
                else
                {
                    await _client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "Твiй кошик досi пустий.",
                    replyMarkup: Bot.Keyboard.index);
                }
            }
        }
    }
}
//LoggerRegistry.GetLogger("file").Info($"[{DateTime.Now}] #{message.Chat.Id}_[{message.MessageId}] '{message.Text}'");