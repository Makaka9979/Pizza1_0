using System;
using System.Collections;
using System.Diagnostics;
using Telegram.Bot;

interface ILogger : IDisposable
{
    void Debug(string description);
    void Info(string description);
    void Warning(string description);
    void Error(string description);
}


class Program
{
    static ArrayList Menu()
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
        for (int i = 0; i < _price.Length; i++) { 
            menu.Add(new Model.Menu(_link[i], _name[i], _price[i], _text[i]));
        }
        return menu;
    }

    async static Task Main(string[] args) {
        string _token = "5529174269:AAFdFoselL-cnp7wt4EveCQ-cyMXxKNHJro";
        TelegramBotClient _client = new TelegramBotClient(_token);
        string fileAddress = "E:\\Program Files\\Visual Studio 2022\\pizza\\pizza\\View\\mylog.log";

        using LoggerPool lpool = new LoggerPool();
        lpool.AddLogger("cli", new CLILogger());
        lpool.AddLogger("file", new FileLogger(fileAddress));
        
        string temp = "Logger OK";
        lpool.GetLogger("cli").Debug(temp);
        lpool.GetLogger("cli").Info(temp);
        lpool.GetLogger("cli").Warning(temp);
        lpool.GetLogger("cli").Error(temp);

        ArrayList menu = Menu();
        Controller.Client user = new();
        View.Telegram bot = new View.Telegram(_client, _token, lpool, menu, user);
        
        string start = $"\n\tSTART [{DateTime.Now}]\n";
        lpool.GetLogger("cli").Info(start);
        lpool.GetLogger("file").Info(start);
        Console.ReadLine();
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}