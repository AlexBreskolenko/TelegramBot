using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceTexterBot.Models;

namespace VoiceTexterBot.Services
{
    //Сервис(интерфейс) который будет отвечать за обработку файлов
    public interface IFileHandler
    {
        Task Download(string fileId, CancellationToken ct);
        string Process(string param);
    }
}


