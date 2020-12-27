using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameHangBot.Models
{
    public static class GameEngine
    {
        public static string getRules()
        {
            var str = "ПРАВИЛА:\n";
            str += "• Присылайте мне только по одной букве русского алфавита.\n";
            str += "• В игре все слова в именительном падеже.\n";
            str += "• Слова взяты из словаря, поэтому могут попадаться довольно странные слова.\n";

            str += "МАЛЕНЬКИЕ ХИТРОСТИ:\n";
            str += "• Слово легче отгадывать, когда известны согласные, входящие в него.\n";
            str += "• Чем длиннее слово, тем проще его угадать.\n";
            str += "• Начинать отгадывать имеет смысл с наиболее чаще встречающихся букв: о, е, и, а,...\n";
            return str;
        }

        public static string initWord(Int64 chatId)
        {
            DataBase db = new DataBase();
            var wordFromBD = db.GetWord();

            var secretWord = wordFromBD.ToUpper();
            var currentWordChar = new char[secretWord.Length * 2];
            for (int i = 0; i < secretWord.Length * 2; i++)
            {
                currentWordChar[i] = '\uFF0D'; // 2014
                i++;
                currentWordChar[i] = ' ';
            }
            var CurrentWord = new string(currentWordChar);

            var user = db.GetUser(chatId);
            if (user == null)
            {
                db.NewUser(chatId, CurrentWord, secretWord);
            }
            else
            {
                db.updateSecretWord(chatId, secretWord);
                db.updateCurrentWord(chatId, CurrentWord);
                db.updateLife(chatId);
            }
            return message(CurrentWord);
        }

        public static string openChar(Int64 chatId, char ch)
        {
            DataBase db = new DataBase();

            var user = db.GetUser(chatId);

            if (user == null)
                return "Выберите команду /game";

            var currentWordChar = user.currentWord.ToCharArray();
            if (user.secretWord.Contains(ch))
            {
                for (int i = 0; i < user.secretWord.Length; i++)
                {
                    if (user.secretWord[i] == ch)
                    {
                        currentWordChar[i * 2] = ch;
                    }
                }
                user.currentWord = new string(currentWordChar);
                db.updateCurrentWord(user.chatId, user.currentWord);
            }
            else
            {
                user.life--;
                db.updateLife(user.chatId, user.life);
            }

            return rules(db, user);
        }

        private static string rules(DataBase db, User user)
        {
            if (user.secretWord.Equals(user.currentWord.ToString().Replace(" ", "")))
            {
                return "Победа!\nТы разгадал(а) слово: " + user.secretWord;
            }

            if (user.life == 0)
            {
                db.updateGameOver(user.chatId);
                return "Поражение!\nЗагаданное слово было: " + user.secretWord;
            }

            return message(user.currentWord, user.life);
        }


        private static string message(string currentWord, int lifes = 7)
        {
            string life = "";
            for (int i = 0; i < lifes; i++)
                life += "\u2764";
            var str = "";
            str += "Слово: " + currentWord + "\n";
            str += "Жизни: " + life;
            return str;
        }

    }
}