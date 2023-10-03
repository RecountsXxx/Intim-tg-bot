using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotIntim
{
    class Program
    {
        private static TelegramBotClient _botClient;
        private static TimerCallback tm = new TimerCallback(send30MinuteMessage);
        private static Timer timer;
        private static List<long> users_id_in_bot = new List<long>();
        static void Main(string[] args)
        {
            //перед запуском создать в папке bin текстовый файл users_in_bot.txt
            //в нём храняться id для отправки всем пользователям сообщение

            _botClient = new TelegramBotClient("6083014604:AAGI5ByYjcMpRpE9DrfRLEB6G1B_NOCh8-U");
            using(StreamReader reader = new StreamReader("../../users_in_bot.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    users_id_in_bot.Add(long.Parse(line));
                }
            }
            _botClient.OnMessage += OnMessageHandler;
            _botClient.OnCallbackQuery += OnCallbackQueryHandler;
            timer = new Timer(tm, null, 0, 300000);
      
            _botClient.StartReceiving();
            Console.WriteLine("Bot started...");

            Thread.Sleep(Timeout.Infinite);
        }
        private static async void send30MinuteMessage(object obj)
        {
            for(int i =0; i < users_id_in_bot.Count; i++)
            {
                using (var stream = System.IO.File.Open("../../photo_spam.jpg", FileMode.Open))
                {
                    var fileToSend = new InputOnlineFile(stream);
                    var message = await _botClient.SendPhotoAsync(users_id_in_bot[i], fileToSend, caption: "А ты точно уверен в своей дeвушкe ?😍\r\n\r\n\U0001f98bМожет всё - таки yбедимcя ? Мне кажется всё плохо.. ❌\r\n\r\nПровeрить / start / start / start");

                }
            }
        }
        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            if (message == null || message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return;

            var chatId = message.Chat.Id;
            var text = message.Text;

            if (text == "/start")
            {
                if (!users_id_in_bot.Contains(chatId))
                {
                    users_id_in_bot.Add(chatId);
                    using(StreamWriter writer = new StreamWriter("../../users_in_bot.txt"))
                    {
                        writer.WriteLine(chatId);
                    }
                }
                var mainMenuKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    new List<KeyboardButton>() { new KeyboardButton { Text = "🔎Найти сливы🔍" } },
                        new List<KeyboardButton>() { new KeyboardButton { Text = "🕵️‍Тех.поддержка🕵️‍" } },
                });

                await _botClient.SendTextMessageAsync(chatId, "Выберите пункт меню:", replyMarkup: mainMenuKeyboard);
            }
            else if (text == "🔎Найти сливы🔍")
            {
                var submenuKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("❤️Instagram❤️", "sub_insta"),
                    },
                    new[]
                    {
                       InlineKeyboardButton.WithCallbackData("💙Telegram💙", "sub_tg"),
                    },
                     new[]
                    {
                       InlineKeyboardButton.WithCallbackData("💙VK💙", "sub_vk"),
                    },
                      new[]
                    {
                      InlineKeyboardButton.WithCallbackData("💙Facebook💙", "sub_facebook"),
                    },
                });

                await _botClient.SendTextMessageAsync(chatId, "Выберите нужную вам соц.сеть 👇", replyMarkup: submenuKeyboard);
            }
            else if (text == "🕵️‍Тех.поддержка🕵️‍")
            {
                await _botClient.SendTextMessageAsync(chatId, "🤑Тг админа - https://t.me/thmamiris🤑");
            }
            else if (text.StartsWith("https://"))
            {
                await _botClient.SendTextMessageAsync(chatId, "🤑НАЙДЕНО🤑");
            }
            else
            {
                await _botClient.SendTextMessageAsync(chatId, "Неверная команда или ссылка. Попробуйте снова.");
            }
        }

        private static async void OnCallbackQueryHandler(object sender, CallbackQueryEventArgs e)
        {
            var callbackQuery = e.CallbackQuery;
            var chatId = callbackQuery.Message.Chat.Id;
            var data = callbackQuery.Data;

            if (data == "sub_insta")
            {
                await _botClient.SendTextMessageAsync(chatId, "🔗 Отправьте боту ссылку на страницу Instagram аккаунта\r\n\r\nПример 👉 https://www.instagram.com/bobriha");
            }
            else if (data == "sub_tg")
            {
                await _botClient.SendTextMessageAsync(chatId, "🔗 Отправьте боту ссылку на страницу Telegram аккаунта или номер телефона \r\n\r\nПример 👉 Пример:\r\n├ https://t.me/el_primo \r\n└ +11932141245");
            }
            else if (data == "sub_vk")
            {
                await _botClient.SendTextMessageAsync(chatId, "🔗 Отправьте боту ссылку на страницу VK аккаунта\r\n\r\nПример 👉 https://www.vk.com/el_primka_7");
            }
            else if (data == "sub_facebook")
            {
                await _botClient.SendTextMessageAsync(chatId, "🔗 Отправьте боту ссылку на страницу Facebook аккаунта\r\n\r\nПример 👉 https://www.facebook.com/bobriha");
            }
        }
    }
}
