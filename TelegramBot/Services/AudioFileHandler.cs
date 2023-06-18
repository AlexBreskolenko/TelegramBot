using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceTexterBot.Models;
using VoiceTexterBot.Configuration;
using Telegram.Bot;
using VoiceTexterBot.Configuration;

namespace VoiceTexterBot.Services
{
    public class AudioFileHandler : IFileHandler
    {
        private readonly AppSettings _appSettings;
        private readonly ITelegramBotClient _telegramBotClient;

        public AudioFileHandler(ITelegramBotClient telegramBotClient, AppSettings appSettings)
        {
            _appSettings = appSettings;
            _telegramBotClient = telegramBotClient;
        }

        public async Task Download(string fileId, CancellationToken ct)
        {
            //генерируем полный путь файла из конфигурации
            string inputAudioFilePath = Path.Combine(_appSettings.DownloadFolder, $"{_appSettings.AudioFileName}.{_appSettings.InputAudioFormat}");

            using (FileStream destionationStream = File.Create(inputAudioFilePath))
            {
                //Загружаем информацию о файле
                var file = await _telegramBotClient .GetFileAsync(fileId,ct);
                if (file == null)
                    return;
               //Скачиваем файл
                await _telegramBotClient.DownloadFileAsync(file.FilePath, destionationStream, ct);
            }
        }
        public string Process (string languageCode)
        {
            //Метод пока не реализован
            throw new NotImplementedException();
        }
    }

}
