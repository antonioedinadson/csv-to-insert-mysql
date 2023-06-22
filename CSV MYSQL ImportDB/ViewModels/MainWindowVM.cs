using CSV_MYSQL_ImportDB.Config;
using System.Windows.Input;
using System.IO;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

namespace CSV_MYSQL_ImportDB.ViewModels
{
    internal class MainWindowVM : ViewModelBase
    {
        #region FilePath
        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }
        #endregion

        #region DataBaseName
        private string _databaseName;
        public string DatabaseName
        {
            get => _databaseName;
            set
            {
                _databaseName = value;
                OnPropertyChanged(nameof(DatabaseName));
            }
        }
        #endregion

        #region TableName
        private string _tableName;
        public string TableName
        {
            get => _tableName;
            set
            {
                _tableName = value;
                OnPropertyChanged(nameof(TableName));
            }
        }
        #endregion

        #region UserName
        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
        #endregion

        #region Password
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        #endregion

        public ICommand StartDBCommand { get; set; }

        private DBConnection _connection;

        public MainWindowVM()
        {
            StartDBCommand = new RelayCommand(ExecuteCsvExtractData, CanExecuteCsvExtractData);
        }

        public void ExecuteCsvExtractData(object obj)
        {

            try
            {
                MySqlCommand _cmd = new MySqlCommand();

                using (StreamReader reader = new StreamReader(_filePath))
                {
                    string headerLine = reader.ReadLine();
                    string[] columnNames = headerLine.Split(',');

                    _connection.OpenConnetion();
                    _cmd.Connection = _connection.GetConnection();

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(',');

                        for (int i = 0; i < values.Length; i++)
                        {
                            // Verifica se o valor é vazio ou em branco
                            if (string.IsNullOrWhiteSpace(values[i]))
                            {
                                values[i] = "null";
                            }
                        }

                        _cmd.CommandText = $"INSERT INTO {_tableName} ({string.Join(", ", columnNames.Select(column => column.Trim('\"')))}) VALUES ({string.Join(", ", values)})";
                        _cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.CloseConnetion();
            }
        }

        private bool CanExecuteCsvExtractData(object obj)
        {
            _connection = new DBConnection("127.0.0.1", "test", "root", "");
            return true;
        }
    }
}
