using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace CleanBackup
{
    public class Telegram
    {
        private static string _token; 
        private static string _telegramChatId;
        Settings _settings;
        public Telegram(Settings settings)
        {
            _settings = settings;
            _token = settings.TelegramToken;
            _telegramChatId = settings.TelegramChtid;

        }

        public async Task SendMessageAsync(string telegramSendMessange)
        {
            if (_settings.TelegramIsNeed == true && !String.IsNullOrEmpty(_settings.TelegramToken) && !String.IsNullOrEmpty(_settings.TelegramChtid))
            {

                TelegramBotClient botClient = new TelegramBotClient(_token);
                var me = botClient.GetMeAsync().Result;
                Console.WriteLine(me.Username);
                var t = await botClient.SendTextMessageAsync(_telegramChatId, telegramSendMessange);
            }

        }

    }
}
