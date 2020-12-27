using GameHangBot.Models.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;

namespace GameHangBot.Models
{
    public static class Bot
    {
        private static TelegramBotClient client;

        private static List<Command> commandsList;
        public static IReadOnlyList<Command> Commands { get => commandsList.AsReadOnly(); }

        public static async Task<TelegramBotClient> Get()
        {
            if (client != null)
            {
                return client;
            }

            commandsList = new List<Command>();
            commandsList.Add(new GetWordCommand());
            commandsList.Add(new RulesCommand());
            commandsList.Add(new GameCommand());
            // add more command

            client = new TelegramBotClient(AppSettings.Key);
            var hook = string.Format(AppSettings.Url, "api/message/update");

            await client.SetWebhookAsync(hook);

            return client;
        }



    }
}