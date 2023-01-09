using Telegram.Bot.Types;
using Telegram.Bot;

namespace Controller
{
    internal class MenuController : Libs.IController
    {
        private string contactFName = "Пiцца Харкiв";
        private static string contactPhone = "+111111111111";
        private static string contact_email = "info@ct-college.net";
        private string vCardTg = $"BEGIN:VCARD\n VERSION:3.0\nN:Харків;Пiцца\nORG:Пiццерiя\n" +
            $"TEL;TYPE=voice,work,pref:{contactPhone}\nEMAIL:{contact_email}\nEND:VCARD";
        private double gpsLatitude = 49.999366f;
        private double gpsLongitude = 36.243200f;
        private string gpsTitle = "ВСП «ХКТФК НТУ «ХПI»";
        private string gpsAddress = "вулиця Манiзера, 4, Харкiв, Харкiвська область, Украина, 61000";

        public async Task HandleContact(ITelegramBotClient _client, Update update)
        {
            await _client.SendContactAsync(
                chatId: update.Message.Chat.Id,
                phoneNumber: contactPhone,
                firstName: contactFName,
                vCard: vCardTg,
                replyMarkup: Bot.Keyboard.other);
            Thread.Sleep(250);
            await _client.SendVenueAsync(
                chatId: update.Message.Chat.Id,
                latitude: gpsLatitude,
                longitude: gpsLongitude,
                title: gpsTitle,
                address: gpsAddress);
        }
        
        public string GetHelloUserString(Update update)
        {
            string hello = "Привiт";
            if (update.Message.Chat.FirstName != null)
            {
                hello += $", {update.Message.Chat.FirstName}";

                if (update.Message.Chat.LastName != null)
                {
                    hello += $" {update.Message.Chat.LastName}";
                }
            }
            else if (update.Message.Chat.LastName != null)
            {
                hello += $", {update.Message.Chat.LastName}";
            }
            return $"{hello}!";
        }
        public async Task HandleStart(ITelegramBotClient _client, Update update)
        {
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: $"{GetHelloUserString(update)}\n" +
                    $"А я тебе вже зачекався 🌚");
            Thread.Sleep(250);
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Вибирай що хочеш зробити",
                replyMarkup: Bot.Keyboard.index);
            Libs.LoggerRegistry.GetLogger("file").Info($"{Bot.Keyboard.NewUserMsg(update.Message)}");
        }
        public async Task HandleIndex(ITelegramBotClient _client, Update update)
        {
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Вибирай що хочеш зробити",
                replyMarkup: Bot.Keyboard.index);
        }
        public async void Run(ITelegramBotClient _client, Update update)
        {
            string message = update.Message.Text;
            if (message == "/start")
                await HandleStart(_client, update);
            else if (message == "Головне меню")
                await HandleIndex(_client, update);
            else if (message == "Контакти")
                await HandleContact(_client, update);
        }
    }
}
