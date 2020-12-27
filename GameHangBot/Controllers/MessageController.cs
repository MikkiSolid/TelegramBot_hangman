using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Telegram.Bot.Types;
using GameHangBot.Models;
using System.Threading.Tasks;

namespace GameHangBot.Controllers
{
    public class MessageController : ApiController
    {
        [Route(@"api/message/update")] // webhook uri part
        public async Task<OkResult> Update([FromBody]Update update)
        {
            var commands = Bot.Commands;
            var message  = update.Message;
            var client = await Bot.Get();

            foreach(var command in commands)
            {
                if (command.Contains(message.Text))
                {
                    await Task.Run(() => command.Execute(message, client));
                    return Ok();
                }
            }

            if(message.Text.Length == 1)
                await Task.Run(() => commands.ElementAt(0).Execute(message, client));
            else
            {
                await Task.Run(() => commands.ElementAt(1).Execute(message, client));
            }


            return Ok();
        }
    }
}
