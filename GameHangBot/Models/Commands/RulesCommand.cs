using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GameHangBot.Models.Commands
{
    public class RulesCommand : Command
    {
        public override string Name => "rules";

        public override async void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            var str = GameEngine.getRules();

            await client.SendTextMessageAsync(chatId, str);
        }
    }
}