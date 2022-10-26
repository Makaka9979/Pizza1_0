using Libs;
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
        private ArrayList _menu = AddMenu();

        private static ArrayList AddMenu()
        {
            string[] _link = new string[]
                { "https://bufet.ua/wp-content/uploads/2018/04/bavarska-2.jpg",
            "https://bufet.ua/wp-content/uploads/2014/04/firmennaya-2.jpg",
            "https://bufet.ua/wp-content/uploads/2014/04/salyami-21.jpg",
            "https://bufet.ua/wp-content/uploads/2014/04/kurica-s-ananasom-2.jpg",
            "https://bufet.ua/wp-content/uploads/2014/04/kurica-s-gribami-2.jpg",
            "https://bufet.ua/wp-content/uploads/2014/04/s-vetchinoj-2.jpg",
            "https://bufet.ua/wp-content/uploads/2014/04/myasnaya-2.jpg" };
            string[] _text = new string[]
                { " • Унiкальна вiдмiнність пiци «Баварська» – це вiдсутнiсть у нiй майонезу! Приготована пiца на тонкому тiстi. \n" +
            " • Склад: Ковбаса сирокопчена, Огiрки солонi, Помiдор, Сир, Олива, Томатний соус, Спецiї “Трави Iталiї” \n" +
            "1255 Ккал / 500 г",
            " • Можна казати багато, але назва каже сама за себе. Пiца «Фiрмова» вiд нашого кухаря – спецiально для Вас! \n" +
            " • Склад: Курка, Шинка, Майонез, Сир, Печерицi, Маслини, Томатний соус \n" +
            "1177,2 Ккал / 540 г",
            " • Iдеальний перекус: засмаженi ковбаснi палички з солоними огiрочками – це те, що робить пiцу «Салямi» незабутньою! \n" +
            " • Склад: Салямi, Томатний соус iз часником, Огiрки соленi, Крiп, Майонез, Сир \n" +
            "1277,8 Ккал / 475 г",
            " • Курка й ананас – чудове поєднання для пiци, що подарує Вам позитивнi емоцiї, а головне – втамує голод! \n" +
            " • Склад: Курка, Ананас, Томатний соус, Майонез, Сир \n" +
            "1187,2 Ккал / 530 г",
            " • Запеченi пiд сиром пiдсмаженi до золотавої скоринки печерицi та нiжне куряче фiле. Що може бути смачнiше? \n" +
            " • Склад: Печериці, Курка, Томатний соус, Майонез, Сир \n" +
            "1081,6 Ккал / 520 г",
            " • Ох вже ця ковбаска, пишна основа й нiчого зайвого. \nПiца з шинкою – вiдмiнне рiшення для закуски. \n" +
            " • Склад: Шинка, Томатний соус, Майонез, Сир \n" +
            "1180,8 Ккал / 480 г",
            " • Ви куштували нашу м’ясну пiцу?! \nЯкщо так, тодi ви точно помiтили – вона не тiльки соковита, але й смачна! \n" +
            "Легкий часниковий соус пiдкреслює смак яловичини, а зелень iз помідорами радує око! \n" +
            " • Склад: Яловичина, Помiдор, Томатний соус iз часником, Майонез, Сир \n" +
            "1118,4 Ккал / 480 г" };
            string[] _name = new string[] { "Боварська", "Фiрмова", "Салямi", "Курка з ананасом",
            "Курка з грибами", "З шинкою", "М'ясна" };
            ushort[] _price = new ushort[] { 110, 100, 100, 100, 100, 100, 110 };
            ArrayList menu = new ArrayList();
            for (int i = 0; i < _price.Length; i++)
            {
                menu.Add(new Model.Menu(_link[i], _name[i], _price[i], _text[i]));
            }
            return menu;
        }
        private async Task Menu(ITelegramBotClient _client, Message message, int _page)
        {
            if (_page < 0) { _page = 0; }
            if (_page >= _menu.Count) { _page = _menu.Count - 1; }
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
                        replyMarkup: Keyboard.menu);
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
            if (((Model.User)SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).readyToOrder)
            {
                if (!((Model.User)SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).ifHaveCommand)
                {
                    await _client.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        text: ($"{((Model.User)SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).order}\n----------\n{((Model.User)SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).ToString()}"));
                    await _client.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        text: "Хочеш додати коментарiй?",
                        replyMarkup: Keyboard.ifComment0);
                }
                else
                {
                    await _client.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        text: ($"{((Model.User)SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).order}\n----------\n{((Model.User)SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).ToString()}"),
                        replyMarkup: Keyboard.ifComment1);
                }
            }
            else
            {
                await _client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: "Спочатку заповни iнформацiю про доставку",
                    replyMarkup: Keyboard.errorAddDeliveryInfo);
            }
        }
        public async void HandleComment(ITelegramBotClient _client, Update update)
        {
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Що ти хочеш повiдомити менi?",
                replyMarkup: Keyboard.order);
            SessionRegistry.Sessions[update.Message.Chat.Id].State["userPage"] = (object)(9);
        }
        public async void HandleHotovo(ITelegramBotClient _client, Update update)
        {
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"Твоє замовлення:\n{((Model.User)SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).ThisOrder()}",
                replyMarkup: Keyboard.hotovo);
            await _client.SendTextMessageAsync(
                chatId: admin_id,
                text: $"Нове замовлення:\n{((Model.User)SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).ThisOrder()}",
                replyMarkup: Keyboard.other);
            ((Model.User)SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).Clear();
            SessionRegistry.Sessions.Remove(update.Message.Chat.Id);
        }
        public async void HandleClear(ITelegramBotClient _client, Update update)
        {
            ((ArrayList)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["orders"]).Clear();
            Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["allPriseOrder"] = (object)0;
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Тепер твiй кошик пустий.",
                replyMarkup: Keyboard.other);
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
                    replyMarkup: Keyboard.other);
                await Menu(_client, update.Message, (int)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["currentPage"]);
                return;
            }
            string listOrders = "";
            for (int i = 0; i < ((ArrayList)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["orders"]).Count; i++)
            {
                listOrders += $"{((ArrayList)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["orders"])[i].ToString()}\n";
            }
            listOrders += $"\n Доставка, {deliveryPrise} грн\nВсього: {((int)Libs.SessionRegistry.Sessions[update.Message.Chat.Id].State["allPriseOrder"]) + (int)deliveryPrise} гривень, 0 копійок";
            user = ((Model.User)SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]);
            user.order = listOrders;
            SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"] = (object)user;

            if (((Model.User)SessionRegistry.Sessions[update.Message.Chat.Id].State["userInformation"]).readyToOrder)
            {
                await _client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: $"Твоє замовлення:\n{listOrders}",
                    replyMarkup: Keyboard.basket2);
            } 
            else
            {
                await _client.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: $"Твоє замовлення:\n{listOrders}",
                    replyMarkup: Keyboard.basket1);
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
                HandleHotovo(_client, update);
        }
    }
}
//LoggerRegistry.GetLogger("file").Info($"[{DateTime.Now}] #{message.Chat.Id}_[{message.MessageId}] '{message.Text}'");
