using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Libs;
using static System.Net.Mime.MediaTypeNames;

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

        public async void HandleContact(ITelegramBotClient _client, Update update)
        {
            await _client.SendContactAsync(
                chatId: update.Message.Chat.Id,
                phoneNumber: contactPhone,
                firstName: contactFName,
                vCard: vCardTg,
                replyMarkup: Keyboard.other);
            Thread.Sleep(250);
            await _client.SendVenueAsync(
                chatId: update.Message.Chat.Id,
                latitude: gpsLatitude,
                longitude: gpsLongitude,
                title: gpsTitle,
                address: gpsAddress);
        }
        public async void HandleStart(ITelegramBotClient _client, Update update)
        {
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "А я тебе вже зачекався 🌚");
            Thread.Sleep(250);
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Вибирай що хочеш зробити",
                replyMarkup: Keyboard.index);
            LoggerRegistry.GetLogger("file").Info($"{Keyboard.NewUserMsg(update.Message)}");
        }
        public async void HandleIndex(ITelegramBotClient _client, Update update)
        {
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Вибирай що хочеш зробити",
                replyMarkup: Keyboard.index);
        }
        public async void Run(ITelegramBotClient _client, Update update)
        {
            string message = update.Message.Text;
            if (message == "/start")
                HandleStart(_client, update);
            else if (message == "Головне меню")
                HandleIndex(_client, update);
            else if (message == "Контакти")
                HandleContact(_client, update);
        }
    }
}
