using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotIntim
{
    internal class Program
    {
        private static string token = "6015763347:AAE6Q63p1i7yYeikfZGDM5lP6Jm6iX_Z1A0";
        public static string language = "";
        private static TelegramBotClient client;
        static void Main(string[] args)
        {

            client = new TelegramBotClient(token);
            client.StartReceiving();
            client.OnMessage += Client_OnMessage;
            Console.ReadLine();
            client.StopReceiving();
        }

        private static async void Client_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var msg = e.Message;
            if(msg.Text != null)
            {
                                Console.WriteLine("Message: " + msg.Text);
                await client.SendTextMessageAsync(msg.Chat.Id, msg.Text,replyMarkup:GetLanguageButtons());
                switch (msg.Text)
                {
                    case "🇷🇺Russian":
                        language = "Russian";
                        await client.SendTextMessageAsync(msg.Chat.Id, "Выберите действие:", replyMarkup:GetActionButtons());
                        switch (msg.Text)
                        {
                            case ""
                        }
                        break;
                    case "🇬🇧Engilish":
                        language = "English";
                        await client.SendTextMessageAsync(msg.Chat.Id, "Select action:", replyMarkup: GetActionButtons());
                        break;
                }
            }
        }

        private static IReplyMarkup GetLanguageButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>()
                {
                    new List<KeyboardButton>() { new KeyboardButton { Text = "🇬🇧Engilish" } },
                         new List<KeyboardButton>() { new KeyboardButton { Text = "🇷🇺Russian" } },
                }
            };
        }
        private static IReplyMarkup GetActionButtons()
        {
            if (language == "Russian")
            {
                return new ReplyKeyboardMarkup
                {
                    Keyboard = new List<List<KeyboardButton>>()
                    {
                        new List<KeyboardButton>() { new KeyboardButton { Text = "🔎Найти сливы🔍" } },
                        new List<KeyboardButton>() { new KeyboardButton { Text = "🕵️‍Тех.поддержка🕵️‍" } },
                        new List<KeyboardButton>() { new KeyboardButton { Text = "🤑Реферальная система🤑" } },
                    }
                };
            }
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>()
                {
                    new List<KeyboardButton>() { new KeyboardButton { Text = "🔎Find pulms🔍" } },
                         new List<KeyboardButton>() { new KeyboardButton { Text = "🕵️‍Tech.support🕵️‍" } },
                           new List<KeyboardButton>() { new KeyboardButton { Text = "🤑Referall system🤑" } },
                }
            };
        }
    }
}
