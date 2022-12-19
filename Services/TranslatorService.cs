using MySql.Data.MySqlClient;
using SSP_1.DAL;
using SSP_2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SSP_1.Services
{
    public class TranslatorService
    {
        private readonly OleDbConnection _connection;
        private readonly CommandCreator _commandCreator;


        public TranslatorService(OleDbConnection connection, CommandCreator commandCreator)
        {
            _connection = connection;
            _commandCreator = commandCreator;
        }



        public string GetTranslations(string word, string targetLanguage, string sourceLanguage)
        {
            _connection.Open();

            var correctedWords = new List<string>();
            var dataSet = FillDataSet();


            var targetLanguageWords = new string[dataSet.Tables[0].Rows.Count];
            for (var i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                targetLanguageWords[i] = dataSet.Tables[0].Rows[i][sourceLanguage].ToString();
            }

            foreach (var targetLanguageWord in targetLanguageWords)
            {
                var cost = LevenshteinDistance.Compute(word, targetLanguageWord);
                if (cost <= 1 && !correctedWords.Contains(targetLanguageWord))
                {
                    correctedWords.Add(targetLanguageWord);
                }
            }

            var result = string.Empty;

            _connection.Open();

            foreach (var correctedWord in correctedWords)
            {
                var command = _commandCreator.SelectValuesByKeyFromTable("words", sourceLanguage, correctedWord);
                var mdr = command.ExecuteReader();
                while (mdr.Read())
                {
                    result = string.Concat(result,
                        string.Concat(correctedWord, " - ", mdr.GetString(mdr.GetOrdinal(targetLanguage)), "\n"));
                }

                mdr.Close();
            }

            _connection.Close();

            return result;
        }


        public string GetTranslationsSpecified(string word, string targetLanguage, string sourceLanguage, string specifiedDictionaryType)
        {


            var correctedWords = new List<string>();

            var specifieddictionary = GetDictionaryId(specifiedDictionaryType);
            _connection.Open();
            var dataSet = FillSpecifiedDataSet(specifieddictionary);


            var targetLanguageWords = new string[dataSet.Tables[0].Rows.Count];
            for (var i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                targetLanguageWords[i] = dataSet.Tables[0].Rows[i][sourceLanguage].ToString();
            }

            foreach (var targetLanguageWord in targetLanguageWords)
            {
                var cost = LevenshteinDistance.Compute(word, targetLanguageWord);
                if (cost <= 1 && !correctedWords.Contains(targetLanguageWord))
                {
                    correctedWords.Add(targetLanguageWord);
                }
            }

            var result = string.Empty;

            _connection.Open();

            foreach (var correctedWord in correctedWords)
            {
                var command = _commandCreator.SelectValuesByKeyFromTableSpecified("words", sourceLanguage, correctedWord, specifieddictionary);
                var mdr = command.ExecuteReader();
                while (mdr.Read())
                {
                    result = string.Concat(result,
                        string.Concat(correctedWord, " - ", mdr.GetString(mdr.GetOrdinal(targetLanguage)), "\n"));
                }

                mdr.Close();
            }

            _connection.Close();

            return result;
        }

        private string GetDictionaryId(string specifiedDictionaryType)
        {
            string result = string.Empty;
            _connection.Open();

            var command = _commandCreator.SelectDictionaryType(specifiedDictionaryType);

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                result =  reader.GetInt32(reader.GetOrdinal("dictionaryid")).ToString();
            }
            _connection.Close();
            return result;
        }
        
        private DataSet FillDataSet()
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = _commandCreator.SelectAllFromTable("words");
            DataSet ds = new DataSet();

            string dataTableName = "words";
            adapter.Fill(ds, dataTableName);

            _connection.Close();

            return ds;
        }

        private DataSet FillSpecifiedDataSet(string specified)
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = _commandCreator.SelectAllSpecifiedFromTable("words", specified);
            DataSet ds = new DataSet();

            string dataTableName = "words";
            adapter.Fill(ds, dataTableName);

            _connection.Close();

            return ds;
        }










       
    }
}







