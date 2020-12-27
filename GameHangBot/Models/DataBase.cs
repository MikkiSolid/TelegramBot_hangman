using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using Dapper;

namespace GameHangBot.Models
{
    public class DataBase
    {
        string _connectionString = "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\hangbot.db;Version=3;";
        private SQLiteConnection connection;
        
        public DataBase()
        {
            connection = new SQLiteConnection(_connectionString);
        }

        public string GetWord()
        {
            Word word = null;

            var sql = "SELECT id,word FROM ( SELECT id,word FROM nouns WHERE wcase = \"им\" and length(word) <= 10";
            sql += ") as t ORDER BY random() LIMIT 1";

            word = connection.Query<Word>(sql).First();

            if (word != null && word.word.Contains('-'))
            {
                word.word = word.word.Replace("-", "");
            }

            return word.word;
        }

        public void NewUser(Int64 chatId, string currentWord = null, string secretWord = null)
        {
            var sql = "INSERT INTO users (chatId, currentWord, secretWord, life) VALUES (@chatId, @currentWord, @secretWord, @life)";
            connection.Query<User>(sql, new { chatId = chatId, currentWord = currentWord, secretWord = secretWord, life = 7 });
        }

        public void updateLife(Int64 chatId, int life = 7)
        {
            var sql = "UPDATE users SET life = @life WHERE chatId = @chatId";
            connection.Query<int>(sql, new { life = life, chatId = chatId }).FirstOrDefault();
        }

        public void updateCurrentWord(Int64 chatId, string currentWord = null)
        {
            var sql = "UPDATE users SET currentWord = @currentWord WHERE chatId = @chatId";
            connection.Query<int>(sql, new { currentWord = currentWord, chatId = chatId }).FirstOrDefault();
        }
        public void updateSecretWord(Int64 chatId, string secretWord = null)
        {
            var sql = "UPDATE users SET secretWord = @secretWord WHERE chatId = @chatId";
            connection.Query<int>(sql, new { secretWord = secretWord, chatId = chatId }).FirstOrDefault();
        }
        public void updateGameOver(Int64 chatId) // xz
        {
            var sql = "UPDATE users SET secretWord = NULL, currentWord = NULL, life = @life WHERE chatId = @chatId";
            connection.Query<int>(sql, new { chatId = chatId, life = 7 }).FirstOrDefault();
        }

        public User GetUser(Int64 chatId) // xz
        {
            var sql = "SELECT * FROM users WHERE chatId = @chatId";
            return connection.Query<User>(sql, new { chatId = chatId}).FirstOrDefault();
        }

        //public string GetSpesialWord(SettingsForSearch settings)
        //{

        //    var sql = "SELECT word FROM ( SELECT word FROM hangman.nouns WHERE wcase = \"им\"";
        //    sql += settings.GetAlifePartQuery(true);
        //    sql += settings.GetBeginEndPartQuery(true);
        //    sql += settings.GetGenderPartQuery(true);
        //    sql += settings.GetLengthPartQuery(true);
        //    sql += ") as t ORDER BY rand() LIMIT 1";

        //    var word = connection.Query<string>(sql).FirstOrDefault();

        //    if (word != null && word.Contains('-'))
        //    {
        //        word = word.Replace("-", "");
        //    }

        //    return word;
        //}

        //public List<User> GetRatingTop10()
        //{
        //    var sql = "SELECT Name,Score FROM hangman.users ORDER BY Score DESC, Score DESC LIMIT 10";
        //    var result = connection.Query<User>(sql).ToList();
        //    return result;
        //}


        //public void UdateScore(User u)
        //{
        //    var sql = "SELECT Score FROM hangman.users WHERE Name = @Name";
        //    var Score = connection.Query<int>(sql, new { Name = u.Name }).FirstOrDefault();

        //    if (Score < u.Score)
        //    {
        //        sql = "UPDATE hangman.users SET Score = @Score WHERE Name = @name";
        //        connection.Query<int>(sql, new { name = u.Name, Score = u.Score }).FirstOrDefault();
        //    }
        //}

    }
}