using System;
using System.Collections;
using System.Security.Cryptography;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot
{
    class Telegram
    {
        private static Random rnd = new Random();
        private TelegramBotClient _client;
        private ArrayList _list = new();
        private int _tempKey = rnd.Next();
        private bool _gameSession = false;

        public void Start()
        {
            Console.WriteLine($"[{DateTime.UtcNow}] System *START*");
            _client.StartReceiving(Update, ErrorMessage);
        }

        public Telegram()
        {
            string _token = "5692076142:AAEBGNUICt8FfzE3zP7eQv0-NNB5ZzPBFFw";
            _client = new TelegramBotClient(_token);

            Libs.Session game = new();
            _list = new ArrayList();
            game.State.Add("UserList", (object)_list);
            _list = new ArrayList();
            game.State.Add("RoleList", (object)_list);
            _list = new ArrayList();
            game.State.Add("ChatIdList", (object)_list);
            game.State.Add("adminId", (object)((long)0));
            game.State.Add("bool _ifHaveAdmin", (object)((bool)false));
            Libs.SessionRegistry.Sessions.Add(0, game);
        }

        private async Task VDefault(Message message)
        {
            if (message.Text == "/start")
            {
                string hello = "Hello";
                if (message.Chat.FirstName != null)
                {
                    hello += $", {message.Chat.FirstName}";
                    if (message.Chat.LastName != null)
                    {
                        hello += $" {message.Chat.LastName}";
                    }
                }
                else if (message.Chat.LastName != null)
                {
                    hello += $", {message.Chat.LastName}";
                }
                hello += "!";
                await _client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: hello);
                hello = "Kommands:\n" +
                    "/connect - для входа в игру\n" +
                    "/admin - залогинится админом\n" +
                    "/begin - (для админа) начало игры\n" +
                    "/list - (для админа) получить список пользователей и ролей\n" +
                    "/kill - (для админа) когда мафия, либо чИлАвеки убили игрока\n" +
                    "/heal - (для админа) когда доктор лечит игрока\n" +
                    "/check - (для админа) когда шериф проверяет игроков\n" +
                    "/end - (для админа) конец игры";
                await _client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: hello);
            }
            else if (message.Text == "/admin")
            {
                _tempKey = rnd.Next();
                Console.WriteLine($"[{DateTime.UtcNow}] Password it`s {_tempKey}");
                await _client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Please, enter denamic admin password.");
                Libs.SessionRegistry.Sessions[message.Chat.Id].State["_state"] = (object)1;
            }
            else if (message.Text == "/connect")
            {
                if (!(bool)Libs.SessionRegistry.Sessions[message.Chat.Id].State["bool _connect"])
                {

                    ((ArrayList)Libs.SessionRegistry.Sessions[0].State["UserList"]).Add($"//{message.Chat.FirstName} {message.Chat.LastName} {message.Chat.Username}//");
                    Libs.SessionRegistry.Sessions[message.Chat.Id].State["_gameUserId"] = (object)((ArrayList)Libs.SessionRegistry.Sessions[0].State["UserList"]).Count;
                    await _client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: $"You`re user id number #{((ArrayList)Libs.SessionRegistry.Sessions[0].State["UserList"]).Count}");
                    Libs.SessionRegistry.Sessions[message.Chat.Id].State["bool _connect"] = (object)((bool)true);
                    ((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"]).Add(message.Chat.Id);
                }
                else
                {
                    await _client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: $"Are you in Game.\nYou`re user id number #{(int)Libs.SessionRegistry.Sessions[message.Chat.Id].State["_gameUserId"]}");
                }
            }
            else if((bool)Libs.SessionRegistry.Sessions[0].State["bool _ifHaveAdmin"]) 
                await AdminPanel(message);
        }

        private async Task AuthorizationCheck(Message message)
        {
            if (message.Text == $"{_tempKey}")
            {
                Libs.SessionRegistry.Sessions[0].State["adminId"] = (object)((long)message.Chat.Id);
                Console.WriteLine($"[{DateTime.UtcNow}] New administrator {(long)Libs.SessionRegistry.Sessions[0].State["adminId"]} added successfully");
                await _client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "You are now an admin.");
                Libs.SessionRegistry.Sessions[message.Chat.Id].State["_state"] = (object)0;
                Libs.SessionRegistry.Sessions[0].State["bool _ifHaveAdmin"] = (object)((bool)true);
                _tempKey = rnd.Next();
            }
            else
            {
                _tempKey = rnd.Next();
                await _client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "You're don't right.");
                Libs.SessionRegistry.Sessions[message.Chat.Id].State["_state"] = (object)0;
            }
        }

        private async Task EndGame()
        {
            if (!_gameSession)
            {
                await _client.SendTextMessageAsync(
                    chatId: (long)Libs.SessionRegistry.Sessions[0].State["adminId"],
                    text: "Game not started.");
                return;
            }
            string listUser = "End of the game\n---------------\n";
            for (int i = 0; i < ((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"]).Count; i++)
            {
                listUser += $"#{i + 1} {(string)((ArrayList)Libs.SessionRegistry.Sessions[0].State["UserList"])[i]} {(string)Libs.SessionRegistry.Sessions[(long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[i]].State["_role"]} ";
                if ((bool)Libs.SessionRegistry.Sessions[(long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[i]].State["bool _alive"])
                    listUser += "[Alive].\n";
                else
                    listUser += "[Dead].\n";
            }
            for (int i = 0; i < ((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"]).Count; i++)
            {
                await _client.SendTextMessageAsync((long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[i], listUser);
                Libs.SessionRegistry.Sessions[(long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[i]].State["bool _alive"] = (object)((bool)false);
            }
            await _client.SendTextMessageAsync((long)Libs.SessionRegistry.Sessions[0].State["adminId"], listUser);
            ((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"]).Clear();
            ((ArrayList)Libs.SessionRegistry.Sessions[0].State["UserList"]).Clear();
            ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Clear();
        }

        private async Task StartGame()
        {
            foreach (long chatId in (ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])
                await _client.SendTextMessageAsync(chatId, "Civilian - Мирный \nDoctor - Доктор \nSheriff - Шериф \n Mafia - Мафия");
            _gameSession = true;
            if (((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"]).Count < 4)
            {
                foreach (long chatId in (ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])
                    await _client.SendTextMessageAsync(chatId, "Not enough players.");
                await _client.SendTextMessageAsync((long)Libs.SessionRegistry.Sessions[0].State["adminId"], "You don't have enough players.");
            }
            else
            {
                switch (((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"]).Count)
                {
                    case 4:
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Doctor");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Mafia");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian"); 
                        break;
                    case 5:
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Doctor");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Mafia");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian"); 
                        break;
                    case 6:
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Doctor");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Mafia");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Mafia");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        break;
                    case 7:
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Sheriff");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Doctor");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Mafia");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Mafia");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        break;
                    case 8:
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Sheriff");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Doctor");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Mafia");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Mafia");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Mafia");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        break;
                    case 9:
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Sheriff");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Doctor");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Mafia");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Mafia");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Mafia");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        break;
                    case 10:
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Sheriff");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Doctor");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Mafia");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Mafia");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Mafia");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Mafia");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Add("Civilian");
                        break;
                    default:
                        foreach (long chatId in (ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])
                            await _client.SendTextMessageAsync(chatId, "Too many players.");
                        await _client.SendTextMessageAsync((long)Libs.SessionRegistry.Sessions[0].State["adminId"], "You have too many players.");
                        return;
                }
                for (int i = ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"]).Count - 1; i >= 1; i--)
                {
                    int j = rnd.Next(i + 1);
                    var temp = ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"])[j];
                    ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"])[j] = ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"])[i];
                    ((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"])[i] = temp;
                }
                for (int i = 0; i < ((ArrayList)Libs.SessionRegistry.Sessions[0].State["UserList"]).Count; i++)
                {
                    Libs.SessionRegistry.Sessions[(long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[i]].State["_role"] = (object)((ArrayList)Libs.SessionRegistry.Sessions[0].State["RoleList"])[i];
                    Libs.SessionRegistry.Sessions[(long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[i]].State["bool _alive"] = (object)((bool)true);

                    await _client.SendTextMessageAsync(
                        chatId: (long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[i],
                        text: $"You:\n - user #{(int)Libs.SessionRegistry.Sessions[(long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[i]].State["_gameUserId"]} \n" +
                        $" - role /{(string)Libs.SessionRegistry.Sessions[(long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[i]].State["_role"]}/");
                }
            }
        }

        private int GetIntChatIdFromString(string strId)
        {
            if (strId == "1")
                return 1;
            else if (strId == "2")
                return 2;
            else if (strId == "3")
                return 3;
            else if (strId == "4")
                return 4;
            else if (strId == "5")
                return 5;
            else if (strId == "6")
                return 6;
            else if (strId == "7")
                return 7;
            else if (strId == "8")
                return 8;
            else if (strId == "9")
                return 9;
            else if (strId == "10")
                return 10;
            else return 0;
        }

        private async Task VTwo(Message message)
        {
            if (((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"]).Count >= GetIntChatIdFromString(message.Text))
            {
                if ((bool)Libs.SessionRegistry.Sessions[(long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[GetIntChatIdFromString(message.Text) - 1]].State["bool _alive"])
                {
                    Libs.SessionRegistry.Sessions[(long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[GetIntChatIdFromString(message.Text) - 1]].State["bool _alive"] = (object)((bool)false);
                    await _client.SendTextMessageAsync(message.Chat.Id, "The player is now dead.");
                    await _client.SendTextMessageAsync((long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[GetIntChatIdFromString(message.Text) - 1], "You are now dead.");
                }
                else
                    await _client.SendTextMessageAsync(message.Chat.Id, "The player has already died.");
            }
            else 
                await _client.SendTextMessageAsync(message.Chat.Id, "I did not find such a player.");
            Libs.SessionRegistry.Sessions[message.Chat.Id].State["_state"] = (object)0;
        }

        private async Task VThree(Message message)
        {
            if (((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"]).Count >= GetIntChatIdFromString(message.Text))
            {
                if (!(bool)Libs.SessionRegistry.Sessions[(long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[GetIntChatIdFromString(message.Text) - 1]].State["bool _alive"])
                {
                    Libs.SessionRegistry.Sessions[(long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[GetIntChatIdFromString(message.Text) - 1]].State["bool _alive"] = (object)((bool)true);
                    await _client.SendTextMessageAsync(message.Chat.Id, "This user is now alive.");
                    await _client.SendTextMessageAsync((long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[GetIntChatIdFromString(message.Text) - 1], "You are now alive.");
                }
                else
                    await _client.SendTextMessageAsync(message.Chat.Id, "The player has already alive.");
            }
            else
                await _client.SendTextMessageAsync(message.Chat.Id, "I did not find such a player.");
            Libs.SessionRegistry.Sessions[message.Chat.Id].State["_state"] = (object)0;
        }

        private async Task VFour(Message message)
        {
            if (((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"]).Count >= GetIntChatIdFromString(message.Text))
            {
                await _client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"It's player [{(string)(Libs.SessionRegistry.Sessions[(long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[GetIntChatIdFromString(message.Text) - 1]].State["_role"])}]");
            }
            else
                await _client.SendTextMessageAsync(message.Chat.Id, "I did not find such a player.");
            Libs.SessionRegistry.Sessions[message.Chat.Id].State["_state"] = (object)0;
        }

        private async Task AdminPanel(Message message)
        {
            if (message.Chat.Id == (long)Libs.SessionRegistry.Sessions[0].State["adminId"])
            {
                if (message.Text == "/list")
                {
                    string listUser = "You have:\n";
                    if (((ArrayList)Libs.SessionRegistry.Sessions[0].State["UserList"]).Count < 1)
                        listUser = "You have 0 users";
                    else
                    {   
                        for (int i = 0; i < ((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"]).Count; i++)
                        { 
                            listUser += $"#{i + 1} {((ArrayList)Libs.SessionRegistry.Sessions[0].State["UserList"])[i]} ";
                            if ((string)Libs.SessionRegistry.Sessions[(long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[i]].State["_role"] != " ")
                            {
                                listUser += $"{(string)Libs.SessionRegistry.Sessions[(long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[i]].State["_role"]}";
                                if ((bool)Libs.SessionRegistry.Sessions[(long)((ArrayList)Libs.SessionRegistry.Sessions[0].State["ChatIdList"])[i]].State["bool _alive"])
                                    listUser += "[Alive].\n";
                                else
                                    listUser += "[Dead].\n";
                            }
                            else
                                listUser += "\n";
                        }
                    }   
                    await _client.SendTextMessageAsync(
                        chatId: (long)Libs.SessionRegistry.Sessions[0].State["adminId"],
                        text: listUser);
                }
                else if (message.Text == "/begin")
                {
                    await StartGame();
                }
                else if (message.Text == "/kill")
                {
                    await _client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Enter player number.");
                    Libs.SessionRegistry.Sessions[message.Chat.Id].State["_state"] = (object)2;
                }
                else if (message.Text == "/heal")
                {
                    await _client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Enter player number.");
                    Libs.SessionRegistry.Sessions[message.Chat.Id].State["_state"] = (object)3;
                }
                else if (message.Text == "/check")
                {
                    await _client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Enter player number.");
                    Libs.SessionRegistry.Sessions[message.Chat.Id].State["_state"] = (object)4;
                }
                else if (message.Text == "/end")
                {
                    await EndGame();
                }
            }
            else
            {
                await _client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "You're not admin.");
            }
        }

        private async Task Update(ITelegramBotClient _client, Update update, CancellationToken _token)
        {
            var message = update.Message;
            if (message == null)
                return;
            if (message.Text == null)
                return;
            if (!Libs.SessionRegistry.Sessions.ContainsKey(message.Chat.Id))
            {
                Libs.Session session = new();
                session.State.Add("_state", (object)0);
                session.State.Add("bool _connect", (object)((bool)false));
                session.State.Add("_gameUserId", (object)0);
                session.State.Add("_role", (object)" ");
                session.State.Add("bool _alive", (object)((bool)false));
                
                Libs.SessionRegistry.Sessions.Add(message.Chat.Id, session);
            } 
            else switch ((int)Libs.SessionRegistry.Sessions[message.Chat.Id].State["_state"])
            {
                case 1:
                    await AuthorizationCheck(message);
                    break;
                case 2:
                    await VTwo(message);
                    break;
                case 3:
                    await VThree(message);
                    break;
                case 4:
                    await VFour(message);
                    break;
                default:
                    await VDefault(message);
                    break;
            }
        }

        private static Task ErrorMessage(ITelegramBotClient _client, Exception exception, CancellationToken _token)
        {
            Console.WriteLine($"[{DateTime.UtcNow}] {exception.Message}");
            return Task.CompletedTask;
        }
    }
}