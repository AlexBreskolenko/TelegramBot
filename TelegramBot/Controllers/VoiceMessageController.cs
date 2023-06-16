using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;


//----------------------------------Контроллер аудио сообщений----------------------------------------------
namespace VoiceTexterBot.Controllers
{
    public class VoiceMessageController
    {
        //интерфейс объекта TelegramBotClient, который как раз и предоставлен нам библиотекой Telegram.Bot. 
        private readonly ITelegramBotClient _telegramClient;

        public VoiceMessageController(ITelegramBotClient telegramClient)
        {
            //Конструктор класса, в котором происходит инициализация внутреннего поля ITelegramBotClient
            _telegramClient = telegramClient;
        }

        //Метод обработки звуковых сообщений
        public async Task Handle(Message message, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Получено голосовое сообщение", cancellationToken: ct);
        }
    }
}
