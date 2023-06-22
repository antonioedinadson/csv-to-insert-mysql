using CSV_MYSQL_ImportDB.Config;
using System.Windows.Input;
using System.IO;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Threading;

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

        #region HostName
        private string _hostName;
        public string HostName
        {
            get => _hostName;
            set
            {
                _hostName = value;
                OnPropertyChanged(nameof(HostName));
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

        #region Pregress
        private int _progress;
        public int Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }
        #endregion

        #region TotalRegisters
        private int _totalRegisters;
        public int TotalRegisters
        {
            get => _totalRegisters;
            set
            {
                _totalRegisters = value;
                OnPropertyChanged(nameof(TotalRegisters));
            }
        }
        #endregion

        public ICommand StartDBCommand { get; set; }

        private DBConnection _connection;

        public MainWindowVM()
        {
            _progress = 0;
            StartDBCommand = new RelayCommand(p => ExecuteCsvExtractData());
        }

        public void ExecuteCsvExtractData()
        {
            _connection = new DBConnection(_hostName, _databaseName, _userName, _password ?? "");
            new Thread(p => Exec()).Start();
        }

        private void Exec()
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

                    int processedLines = 0;
                    TotalRegisters = File.ReadLines(_filePath).Count();


                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(',');

                        for (int i = 0; i < values.Length; i++)
                        {                            
                            if (string.IsNullOrWhiteSpace(values[i]))
                            {
                                values[i] = "null";
                            }
                        }

                        _cmd.CommandText = $"INSERT INTO {_tableName} ({string.Join(", ", columnNames.Select(column => column.Trim('\"')))}) VALUES ({string.Join(", ", values)})";
                        _cmd.ExecuteNonQuery();

                        processedLines++;
                        Progress = processedLines;
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
    }
}
