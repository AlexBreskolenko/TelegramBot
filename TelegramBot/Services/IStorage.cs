using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceTexterBot.Models;

namespace VoiceTexterBot.Services
{
    //-------------------------------------создаём сервис хранения пользовательских сессий----------------------
    public interface IStorage
    {
        ///<summary>
        ///Получение сессиии пользователя по идентификатору
        ///</summary>
        Session GetSession(long chatId);
    }

}
