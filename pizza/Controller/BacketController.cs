using Libs;
using System;
using System.Collections;
using System.ComponentModel;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Controller 
{
    class BacketController : IController
    {
        private ArrayList _menu = AddMenu();
        private int _page = 0;
        Model.User user = new();

        private ArrayList addOrder = new ArrayList();
        private ReplyKeyboardMarkup other = new(new[] {
            new KeyboardButton[] { "Головне меню" }
        }) { ResizeKeyboard = false };
        private ReplyKeyboardMarkup index = new(new[] {
            new KeyboardButton[] { "Головне меню" }
        }) { ResizeKeyboard = true };
        private ReplyKeyboardMarkup ifAllGood = new(new[] {
            new KeyboardButton[] { "Yes", "No" }
        }) { ResizeKeyboard = true };
        private ReplyKeyboardMarkup paymend = new(new[] {
            new KeyboardButton[] { "Картою", "Готiвкою" }
        }) { ResizeKeyboard = true };
        private ReplyKeyboardMarkup basket = new(new[] {
            new KeyboardButton[] { "Замовити", "Очистити" },
            new KeyboardButton[] { "Головне меню" }
        }) { ResizeKeyboard = true };
        private ReplyKeyboardMarkup menu = new(new[] {
            new KeyboardButton[] { "⏪ Назад", "⏩ Вперед" },
            new KeyboardButton[] { "➕ Додати до кошика" },
            new KeyboardButton[] { "Корзина", "Головне меню" }
        }) { ResizeKeyboard = true };

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
        private async Task Menu(ITelegramBotClient _client, Message message)
        {
            if (_page < 0) { _page = 0; }
            if (_page >= _menu.Count) { _page = _menu.Count - 1; }
            string currentState = $"{_page + 1} / {_menu.Count}";
            string text = text = $"<a><b>  {((Model.Menu)_menu[_page]).Name}</b></a>\n" +
                        $"<a>{((Model.Menu)_menu[_page]).Text}</a>\n\n" +
                        $"<i>Цiна:</i> <a>{((Model.Menu)_menu[_page]).Price}</a>\n\n" +
                        $"<a>Сторiнка меню </a>[<b><i>{currentState}</i></b>]";

            Message sentMessage = await _client.SendPhotoAsync(
                        chatId: message.Chat.Id,
                        photo: ((Model.Menu)_menu[_page]).Link,
                        caption: text,
                        parseMode: ParseMode.Html,
                        replyMarkup: menu);
            //logger.GetLogger("file").Info($"[{DateTime.Now}] #{message.Chat.Id}_[{message.MessageId}] '{message.Text}'");
        }
        public async void HandleAdd(ITelegramBotClient _client, Update update)
        {
            addOrder.Add((Model.Menu)_menu[_page]);
             await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"Так, я добавив пiцу '{((Model.Menu)_menu[_page]).Name}' до кошику");
            _page++;
            await Menu(_client, update.Message);
        }
        public async void HandleMenuNext(ITelegramBotClient _client, Update update)
        {
            _page++;
            await Menu(_client, update.Message);
        }
        public async void HandleMenuPrev(ITelegramBotClient _client, Update update)
        {
            _page--;
            await Menu(_client, update.Message);
        }
        public async void HandleMenu(ITelegramBotClient _client, Update update)
        {
            _page = 0;
            await Menu(_client, update.Message);
        }
        public async void HandleClear(ITelegramBotClient _client, Update update)
        {
            addOrder.Clear();
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Тепер твiй кошик пустий.",
                replyMarkup: index);
        }
        public async void HandleShowBacketContent(ITelegramBotClient _client, Update update)
        {
            if (addOrder.Count < 1)
            {
                await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Твiй кошик пустий.",
                replyMarkup: index);
                await Menu(_client, update.Message);
                return;
            }
            string listOrders = "";
            for (int i = 0; i < addOrder.Count; i++)
            {
                listOrders += $"{(addOrder[i]).ToString()}\n";
            }
            user.order = listOrders;
            await _client.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        text: listOrders,
                        replyMarkup: basket);
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
        }

    }
}
