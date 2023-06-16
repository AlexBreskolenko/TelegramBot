using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using VoiceTexterBot.Services;


//----------------------------------Контроллер обработчик нажатий на кнопки----------------------------------------------
namespace VoiceTexterBot.Controllers
{
    public class InlineKeyboardController
    {
        //интерфейс объекта TelegramBotClient, который как раз и предоставлен нам библиотекой Telegram.Bot. 
        private readonly ITelegramBotClient _telegramClient;
        //Создаем интерфейс
        private readonly IStorage _memoryStorage;

        public InlineKeyboardController(ITelegramBotClient telegramClient, IStorage memoryStorage)
        {
            //Конструктор класса, в котором происходит инициализация внутреннего поля ITelegramBotClient
            _telegramClient = telegramClient;
            _memoryStorage = memoryStorage;
        }

        //Метод обработки нажатия на кнопку
        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if(callbackQuery?.Data == null)
            {
                return;
            }

            //Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).LanguageCode = callbackQuery.Data;

            //Генерируем информационное сообщение
            string languageText = callbackQuery.Data switch
            {
                "ru" => "Русский",
                "en" => "Англиский",
                _ => String.Empty
            };

            //Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"<b>Язык аудио - {languageText}.{Environment.NewLine}</b>" + $"{Environment.NewLine}" +
                $"Можно поменять в главном меню.", cancellationToken : ct, parseMode : Telegram.Bot.Types.Enums.ParseMode.Html);
        }
    }
}
