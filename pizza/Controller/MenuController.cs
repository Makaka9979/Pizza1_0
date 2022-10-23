using Libs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libs;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Reflection;
using Telegram.Bot.Types.ReplyMarkups;
using Model;
using Telegram.Bot.Types.Enums;
using System.Collections;
using Microsoft.VisualBasic;

namespace Controller
{
    internal class MenuController : IController
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

        private ReplyKeyboardMarkup index = new(new[] {
            new KeyboardButton[] { "Меню", "Корзина" },
            new KeyboardButton[] { "Контакти" }
        }) { ResizeKeyboard = true };
        private ReplyKeyboardMarkup contact = new(new[] {
            new KeyboardButton[] { "Головне меню" }
        }) { ResizeKeyboard = true };

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
        
        public async void HandleContact(ITelegramBotClient _client, Update update)
        {
            await _client.SendContactAsync(
                    chatId: update.Message.Chat.Id,
                    phoneNumber: contactPhone,
                    firstName: contactFName,
                    vCard: vCardTg,
                    replyMarkup: contact);
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
            await _client.SendTextMessageAsync(update.Message.Chat.Id, "А я тебе вже зачекався 🌚");
            Thread.Sleep(250);
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Вибирай що хочеш зробити",
                replyMarkup: index);
        }
        public async void HandleIndex(ITelegramBotClient _client, Update update)
        {
            await _client.SendTextMessageAsync(
                        chatId: update.Message.Chat.Id,
                        text: "Вибирай що хочеш зробити",
                        replyMarkup: index);
        }
        public async void Run(ITelegramBotClient _client, Update update)
        {
            var message = update.Message.Text;
            if(message == "/start")
                HandleStart(_client, update);
            else if (message == "Головне меню")
                HandleIndex(_client, update);
        }
    }
}
