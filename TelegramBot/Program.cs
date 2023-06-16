//----------------Разработка телеграм бота---------------------------

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
//Добавляем наше созданное пространство имен
using VoiceTexterBot.Controllers;
using VoiceTexterBot.Services;
using VoiceTexterBot.Configuration;

namespace VoiceTexterBot
{
    
    internal class Bot : BackgroundService
    {

        //(Клиент к Telegram Bot API)интерфейс объекта TelegramBotClient, который как раз и предоставлен нам библиотекой Telegram.Bot. 
        private ITelegramBotClient _telegramClient;

        //Мои созданные контроллеры различных видов сообщений(создали обьекты наших классов)
        private TextMessageController _textMessageController;
        private VoiceMessageController _voiceMessageController;
        private DefaultMessageController _defaultMessageController;
        private InlineKeyboardController _inlineKeyboardController;

        //Конструктор класса, в котором происходит инициализация внутреннего поля ITelegramBotClient и моих классов 
        public Bot(ITelegramBotClient telegramClient, TextMessageController textMessageController, VoiceMessageController voiceMessageController, DefaultMessageController defaultMessageController, InlineKeyboardController inlineKeyboardController)
        {
            this._telegramClient = telegramClient;
            _textMessageController = textMessageController;
            _voiceMessageController = voiceMessageController;
            _defaultMessageController = defaultMessageController;
            _inlineKeyboardController = inlineKeyboardController;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Здесь выбираем, какие обновления хотим получать. В данном случае разрешены все
            _telegramClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, new ReceiverOptions() { AllowedUpdates = { } }, cancellationToken: stoppingToken);
            Console.WriteLine("Бот запущен !");
        }

        //HandleUpdateAsync - метод для обработки событий
        //async - асинхроность, Task - метод возвращает обьект Task 
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Обрабатываем нажатия на кнопки  из Telegram Bot API: https://core.telegram.org/bots/api#callbackquery
            if (update.Type == UpdateType.CallbackQuery)
            {
                //применяем метод из класса контроля нажатия кнопки
                await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }
            // Обрабатываем входящие сообщения из Telegram Bot API: https://core.telegram.org/bots/api#message
            if (update.Type == UpdateType.Message)
            {
                switch (update.Message!.Type)
                {
                    case MessageType.Voice:
                        await _voiceMessageController.Handle(update.Message, cancellationToken);
                        return;
                    case MessageType.Text:
                        await _textMessageController.Handle(update.Message, cancellationToken);
                        return;
                    default:
                        await _defaultMessageController.Handle(update.Message,cancellationToken);
                        return;
                }
            }
        }

        //HandleErrorAsync - метод для обработки ошибок
        //Задаем сообщение об ошибке в зависимости от того, какая именно ошибка произошла
        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Tlelegram API Error:\n [{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            //Выводим в консоль сообщение об ошибке
            Console.WriteLine(errorMessage);

            //Задержка перед повторным подключением
            Console.WriteLine("");
            Thread.Sleep(10000);

            return Task.CompletedTask;
        }
    }

    public class Program
    {
        //новый метод, который будет инициализировать конфигурацию:
        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                BotToken = "5729694398:AAEFmS9E5yblAbAxct5Z4HLf--RMB7m7neU"
            };
        }

        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            //Объект, отвечающий за постоянный жизненный цикл приложения 
            // => Задаем конфигурацию. Позволяет поддерживать приложение активным в консоли. Собираем
            var host = new HostBuilder().ConfigureServices((hostContext, services) => ConfigureServices(services)).UseConsoleLifetime().Build();

            Console.WriteLine("Сервис запущен.");

            //Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен.");
        }

        //Контейнер зависимостей
        static void ConfigureServices(IServiceCollection services)
        {
            //добавив инициализацию конфигурации в его начало
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(BuildAppSettings);
            //Подключаем хранилище
            services.AddSingleton<IStorage, MemoryStorage>();
            //Подключаем мои контроллеры сообщений и кнопок
            services.AddTransient<TextMessageController>();
            services.AddTransient<VoiceMessageController>();
            services.AddTransient<InlineKeyboardController>();
            services.AddTransient<DefaultMessageController> ();

            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("5729694398:AAEFmS9E5yblAbAxct5Z4HLf--RMB7m7neU"));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
        }
    }


}
