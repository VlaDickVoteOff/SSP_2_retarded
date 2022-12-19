using System;
using System.Data.OleDb;
using System.Security.Cryptography;
using SSP_1.DAL;
using SSP_1.Exception;

namespace SSP_1.Services
{
    public class DbService
    {
        private readonly OleDbConnection _connection;
        private readonly CommandCreator _commandCreator;

        public DbService(OleDbConnection connection, CommandCreator commandCreator)
        {
            _connection = connection;
            _commandCreator = commandCreator;
        }

        public bool InsertNewPair(string russianWord, string englishWord)
        {
            var selectAllCommand = _commandCreator.SelectAllFromTable("words");
            var insertCommand = _commandCreator.InsertTwoKeyValues("words", "russian", russianWord, "english", englishWord);
            _connection.Open();

            var reader = selectAllCommand.ExecuteReader();

            while (reader.Read())
            {
                var englishDictionaryWord = reader.GetString(reader.GetOrdinal("english"));
                var russianDictionaryWord = reader.GetString(reader.GetOrdinal("russian"));

                if (englishDictionaryWord == englishWord && russianDictionaryWord == russianWord)
                {
                    _connection.Close();
                    throw new DatabaseException("Уже существует");
                }
            }



            var result = insertCommand.ExecuteNonQuery();

            _connection.Close();

            return result != 0;
        }

        public int DeleteByKeyValue(string language, string value)
        {
            _connection.Open();
            var deleteCommand = _commandCreator.DeleteByKeyValue("words", language, value);
            

            int number =  deleteCommand.ExecuteNonQuery();
            _connection.Close();
            return number;
        }

        public bool InsertNewPairSpecified(string russianWord, string englishWord, string specified)
        {
            var selectAllCommand = _commandCreator.SelectAllFromTable("words");
            var insertCommand = _commandCreator.InsertThreeKeyValues("words", "russian", russianWord, "english", englishWord, "dictionarytype", specified);
            _connection.Open();

            var reader = selectAllCommand.ExecuteReader();

            while (reader.Read())
            {
                var englishDictionaryWord = reader.GetString(reader.GetOrdinal("english"));
                var russianDictionaryWord = reader.GetString(reader.GetOrdinal("russian"));

                if (englishDictionaryWord == englishWord && russianDictionaryWord == russianWord)
                {
                    _connection.Close();
                    throw new DatabaseException("Уже существует");
                }
            }



            var result = insertCommand.ExecuteNonQuery();

            _connection.Close();

            return result != 0;
        }
    }
}
