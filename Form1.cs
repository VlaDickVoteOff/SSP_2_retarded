using SSP_1.DAL;
using SSP_1.Services;
using System;
using System.IO;
using System.Windows.Forms;
using SSP_1.Exception;

namespace SSP_2
{
    public partial class Form1 : Form
    {
        private readonly TranslatorService translatorService;
        private readonly DbService _dbService;
        private readonly ResourceReader _resourceReader;

        public Form1()
        {
            try
            {
                InitializeComponent();
                _resourceReader = new ResourceReader();
                var dbPath = _resourceReader.GetResourcePath("dictionary.mdb");
                ConnectionProvider.CreateConnection(dbPath);

                var connection = ConnectionProvider.Instance;
                translatorService = new TranslatorService(connection,
                    new CommandCreator(connection));
                _dbService = new DbService(connection, new CommandCreator(connection));
            }
            catch (DatabaseException e)
            {
                MessageBox.Show("Не получилось подключиться к базе данных. " + e.Message);
                throw;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string wordToTranslate = textBox1.Text.ToLower();
            string translated;
            var dictionary = comboBox1.SelectedItem?.ToString();    


            if (string.IsNullOrEmpty(dictionary))
            {
                if (radioButton1.Checked)
                {
                    translated = translatorService.GetTranslations(wordToTranslate, "russian", "english");
                }
                else if (radioButton2.Checked)
                {
                    translated = translatorService.GetTranslations(wordToTranslate, "english", "russian");
                }
                else
                {
                    MessageBox.Show("Направление не выбрано");
                    return;
                }

                if (translated != "")
                {
                    label1.Text = translated;
                }
                else
                {
                    MessageBox.Show("No data found!");
                }
                DumpData1(textBox1.Text);
                DumpData(label1.Text);
                return;
            }

            string specifieddictionary = string.IsNullOrEmpty(dictionary) ? "" : dictionary;

            if (radioButton1.Checked)
            {
                translated =
                    translatorService.GetTranslationsSpecified(wordToTranslate, "russian", "english",
                        specifieddictionary);
            }
            else if (radioButton2.Checked)
            {
                translated =
                    translatorService.GetTranslationsSpecified(wordToTranslate, "english", "russian",
                        specifieddictionary);
            }
            else
            {
                MessageBox.Show("Направление не выбрано");
                return;
            }

            if (translated != "")
            {
                label1.Text = translated;
            }
            else
            {
                MessageBox.Show("No data found!");
            }



            DumpData1(textBox1.Text);
            DumpData(label1.Text);


        }
    


    
        


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = String.Empty;
            textBox1.Text = String.Empty;
            File.Delete(Environment.CurrentDirectory + "/TextDump.txt");
            File.Delete(Environment.CurrentDirectory + "/TextDump1.txt");

        }


       
        void DumpData(string content)
        {
            using (var writer = new StreamWriter(Environment.CurrentDirectory + "/TextDump.txt"))
            {
                writer.Write(content);
            }
        }

        void DumpData1(string content)
        {
            using (var writer = new StreamWriter(Environment.CurrentDirectory + "/TextDump1.txt"))
            {
                writer.Write(content);
            }
        }
        string ReadData()
        {
           
                if (File.Exists(Environment.CurrentDirectory + "/TextDump.txt"))
                {
                try
                {
                    using (var reader = new StreamReader(Environment.CurrentDirectory + "/TextDump.txt"))
                    {
                        label1.Text = reader.ReadToEnd();
                        return reader.ReadToEnd();

                    }
                }
                catch (Exception ex) { MessageBox.Show("Ошибка доступа к файлу!"); }
            }

                return string.Empty;
            
        }

        string ReadData1()
        {

           
                if (File.Exists(Environment.CurrentDirectory + "/TextDump1.txt"))
            {
                    try
                    {
                        using (var reader = new StreamReader(Environment.CurrentDirectory + "/TextDump1.txt"))
                {
                    textBox1.Text = reader.ReadToEnd();
                    return reader.ReadToEnd();

                }
                }
                catch (Exception ex) { MessageBox.Show("Ошибка доступа к файлу!"); }
            }

            return string.Empty;
            
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            ReadData();
            ReadData1();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dbService.InsertNewPair(textBox2.Text, textBox3.Text))
                {
                    MessageBox.Show("Успешно добавлено!");
                }
                else
                {
                    MessageBox.Show("Не получилось добавить");
                }
            }
            catch (DatabaseException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var selected = comboBox3.SelectedItem?.ToString();
        //        if (_dbService.InsertNewPairSpecified(textBox2.Text, textBox3.Text, selected))
        //        {
        //            MessageBox.Show("Успешно добавлено!");
        //        }
        //        else
        //        {
        //            MessageBox.Show("Не получилось добавить");
        //        }
        //    }
        //    catch (DatabaseException exception)
        //    {
        //        MessageBox.Show(exception.Message);
        //    }
        //}

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var comboBoxValue = comboBox2.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(comboBoxValue))
            {
                MessageBox.Show("Выберите язык");
                return;
            }

            string tableName = string.Empty;

            switch (comboBoxValue)
            {
                case "Русский":
                    tableName = "russian";
                    break;
                case "Английский":
                    tableName = "english";
                    break;
            }
            int rowsRemoved = _dbService.DeleteByKeyValue(tableName, textBox4.Text);

            MessageBox.Show($"Удалено {rowsRemoved} строк");
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    }
    

