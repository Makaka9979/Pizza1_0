using System.Collections;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot
{
    static class Keyboard
    {
        //.....
        //namespace Bot
        //.....
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
        public static string[] greenCardCommandsList = { "Меню", "Корзина", "Контакти", "Головне меню", "⏪ Назад", "⏩ Вперед",
            "➕ Додати до кошика", "Очистити", "✅Замовити", "🚫Замовити", "Додати комментарiй", "🍪Готово" };
        //.....
        //namespace Controller 
        //.....
        public static ReplyKeyboardMarkup basket1 = new(new[] {
            new KeyboardButton[] { "🚫Замовити", "Очистити" },
            new KeyboardButton[] { "⁉️Додати iнформацiю про доставку" },
            new KeyboardButton[] { "Головне меню" }
        }) { ResizeKeyboard = true };
        public static ReplyKeyboardMarkup basket2 = new(new[] {
            new KeyboardButton[] { "✅Замовити", "Очистити" },
            new KeyboardButton[] { "✅Додати iнформацiю про доставку" },
            new KeyboardButton[] { "Головне меню" }
        }) { ResizeKeyboard = true };
        public static ReplyKeyboardMarkup errorAddDeliveryInfo = new(new[] {
            new KeyboardButton[] { "⁉️Додати iнформацiю про доставку", "Головне меню" }
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
        public static ArrayList AddMenu()
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
    }
}