using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceTexterBot.Models;


namespace VoiceTexterBot.Services
{
    //-------------------------- класс MemoryStorage, который будет реализовать интерфейс IStorage.---------------------
    public class MemoryStorage : IStorage
    {
        ///<summary>
        ///Хранилище сессий
        ///</summary>
        private readonly ConcurrentDictionary<long, Session> _sessions;

        public MemoryStorage()
        {
            //Создаем колекцию _session хранилище типа данных ConcurrentDictionary
            _sessions = new ConcurrentDictionary<long, Session>();
        }

        //Метод GetSession(...) работает с хранилищем сессий, позволяя нам при подключении клиента создать новую сессию или обновить существующую.
        public Session GetSession(long chatId)
        {
            //Возвращаем сессию по ключу
            if (_sessions.ContainsKey(chatId))
            {
                return _sessions[chatId];
            }

            //Создаем и возвращаем новую, если такой небыло
            var newSession = new Session() { LanguageCode = "ru" };
            _sessions.TryAdd(chatId, newSession);
            return newSession;
        }

    }
}
