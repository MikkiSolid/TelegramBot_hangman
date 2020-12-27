using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GameHangBot.Models.Commands
{
    public class GameCommand : Command
    {
        public override string Name => "game";

        public override async void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            var str = GameEngine.initWord(chatId);

            await client.SendTextMessageAsync(chatId, str);
        }
    }
}