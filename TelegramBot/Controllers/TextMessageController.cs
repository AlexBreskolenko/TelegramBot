using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;


//----------------------------------Контроллер текстовых сообщений----------------------------------------------
namespace VoiceTexterBot.Controllers
{
    public class TextMessageController
    {
        //интерфейс объекта TelegramBotClient, который как раз и предоставлен нам библиотекой Telegram.Bot. 
        private readonly ITelegramBotClient _telegramClient;

        public TextMessageController(ITelegramBotClient telegramClient)
        {
            //Конструктор класса, в котором происходит инициализация внутреннего поля ITelegramBotClient
            _telegramClient = telegramClient;
        }

        //----------Создаем кнопки
        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":
                    //Обьект , предоставляющий кнопки (параметр ReplyMarkup)
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[] { InlineKeyboardButton.WithCallbackData($"Русский", $"ru"), InlineKeyboardButton.WithCallbackData($"English", $"en") });

                    //Передаем кнопки вместе с сообщением
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b> Наш бот превращает аудио в текст. </b> {Environment.NewLine}" + $"{Environment.NewLine}" +
                        $"Можно записать сообщение и переслать другу, если лень печатать.{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html,
                        replyMarkup: new InlineKeyboardMarkup(buttons));

                    break;
                default:
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Отправте аудио для превращение в текст.", cancellationToken: ct);
                    break;
            }
        }
    }
}
