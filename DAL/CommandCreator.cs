using System.Data.OleDb;

namespace SSP_1.DAL
{
    public class CommandCreator
    {
        private readonly OleDbConnection _connection;

        public CommandCreator(OleDbConnection connection)
        {
            _connection = connection;
        }

        public OleDbCommand SelectAllFromTable(string tableName)
        {
            OleDbCommand command = _connection.CreateCommand();

            command.CommandText = $"SELECT * FROM {tableName}";

            return command;
        }

        public OleDbCommand SelectDictionaryType(string disctinaryType)
        {
            OleDbCommand command = _connection.CreateCommand();

            command.CommandText = $"SELECT * FROM dictionaries WHERE dictionarytype='{disctinaryType}'";

            return command;
        }

        public OleDbCommand SelectValuesByKeyFromTable(string tableName, string key, string value)
        {
            var query = $"SELECT * FROM {tableName} WHERE {key}='{value}'";

            return new OleDbCommand(query, _connection); ;
        }



        public OleDbCommand SelectAllSpecifiedFromTable(string tableName, string specifieddictionary)
        {
            OleDbCommand command = _connection.CreateCommand();

            command.CommandText = $"SELECT * FROM {tableName} WHERE dictionaryid='{specifieddictionary}'";

            return command;
        }

        public OleDbCommand InsertTwoKeyValues(string tableName, string key1, string value1, string key2, string value2)
        {
            var query = $"INSERT INTO {tableName} ({key1},{key2}) VALUES ('{value1}', '{value2}')";

            return new OleDbCommand(query, _connection); ;
        }

        public OleDbCommand InsertThreeKeyValues(string tableName, string key1, string value1, string key2, string value2, string key3, string value3)
        {
            var query = $"INSERT INTO {tableName} ({key1},{key2},{key3}) VALUES ('{value1}', '{value2}', '{value3}')";

            return new OleDbCommand(query, _connection); ;
        }

        public OleDbCommand DeleteByKeyValue(string tableName, string key, string value)
        {
            var query = $"DELETE FROM {tableName} WHERE {key} = '{value}'";

            return new OleDbCommand(query, _connection); ;
        }

        public OleDbCommand SelectValuesByKeyFromTableSpecified(string taleName, string sourceLanguage, string correctedWord, string specifieddictionary)
        {
            var query = $"SELECT * FROM {taleName} WHERE {sourceLanguage}='{correctedWord}' AND dictionaryid='{specifieddictionary}'";

            return new OleDbCommand(query, _connection); ;
        }
    }
}
