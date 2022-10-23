using System;
using System.IO;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections;
using Telegram.Bot.Types.Enums;
using Libs;

namespace Bot
{
    internal class Telegram
    {
        private TelegramBotClient _client;

        //private static long admin_id = 562489554;
        public Telegram()
        {
            string _token = "5529174269:AAFdFoselL-cnp7wt4EveCQ-cyMXxKNHJro";
            _client = new TelegramBotClient(_token); ;
        }
        public void Run() 
        {
            _client.StartReceiving(Update, ErrorMessage);
        }

        /*private async Task PlaceAnOrder(Message message)
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
                addOrder.Clear();
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
        */
        private async Task Update(ITelegramBotClient botClient, Update update, CancellationToken botToken)
        {
            var message = update.Message;
            if (message == null)
                return;
            if (message.Text == null)
                return;
            if (!SessionRegistry.Sessions.ContainsKey(message.Chat.Id))
            {
                Session session = new();
                ArrayList orders = new ArrayList();
                session.State.Add("orders", (object)orders);
                session.State.Add("id", (object)message.Chat.Id);
                session.State.Add("currentPage", (object)0);
                SessionRegistry.Sessions.Add(message.Chat.Id, session);
                
            }
            Thread.Sleep(250);
            ControllerRegistry.Get(message.Text)?.Run(_client, update);
        }

        private static Task ErrorMessage(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            string msg = $"[{DateTime.Now}] {exception.Message}";

            LoggerRegistry.GetLogger("cli").Warning(msg);
            LoggerRegistry.GetLogger("file").Warning(msg);

            return Task.CompletedTask;
        }
    }
}
