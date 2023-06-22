using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;

namespace CSV_MYSQL_ImportDB
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            };
        }

        private void ButtonBaseFile_Click(object sender, RoutedEventArgs e)
        {
            txtFileBase.Text = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();

            _ = openFileDialog.ShowDialog();

            if (string.IsNullOrEmpty(openFileDialog.FileName))
            {
                return;
            }

            txtFileBase.Text = openFileDialog.FileName;
        }
    }
}
