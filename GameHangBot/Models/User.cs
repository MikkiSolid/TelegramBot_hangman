using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameHangBot.Models
{
    public class User
    {
        public int id { get; set; }
        public int chatId { get; set; }
        public string currentWord { get; set; }
        public string secretWord { get; set; }
        public int life { get; set; }
    }
}