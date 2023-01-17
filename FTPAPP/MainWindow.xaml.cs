using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FTPAPP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FTP_Client _Client;
        public MainWindow()
        {
            Window1 passwordWindow = new Window1();
 
            if(passwordWindow.ShowDialog()==true)
            {
                _Client = new FTP_Client(passwordWindow.Server, 30000, false, passwordWindow.Login, passwordWindow.Password);
                if(_Client.CreateRequest() && _Client.Authorization())
                {
                    InitializeComponent();
                    MessageBox.Show("Авторизация пройдена");
                    Data.ItemsSource = _Client.ListDirectory();
                }
                else
                    MessageBox.Show("Неверный пароль");
            }
            else
            {
                MessageBox.Show("Авторизация не пройдена");
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (_Client.CreateRequest())
            {
                string path = "";
                DataFile file = (DataFile)Data.SelectedItem;

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                {
                    string fileName = saveFileDialog.FileName;
                    path = fileName;
                }

                _Client.DownloadFile(path + '.' + file.file_type, file.file_size, file.file_name);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (_Client.CreateRequest())
                Data.ItemsSource = _Client.BackDirectory();
        }

        private void Data_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataFile file = (DataFile)Data.SelectedItem;
            if(file.file_type == "dir")
                Data.ItemsSource = _Client.ChangeDirectory(file.file_name);
        }
    }
}
