using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace GameHangBot.Models.Commands
{
    public class GetWordCommand : Command
    {
        public override string Name => "getWord";

        public override async void Execute(Message message, TelegramBotClient client)
        {
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;
            char ch = message.Text.ToUpper()[0];

            var str = GameEngine.openChar(chatId, ch);

            if (str.Contains("Победа!") || str.Contains("Поражение!"))
            {
                str += "\n А это слово разгадаешь?";
                await client.SendTextMessageAsync(chatId, str);
                str = GameEngine.initWord(chatId);
            }

            await client.SendTextMessageAsync(chatId, str);
        }
    }
}