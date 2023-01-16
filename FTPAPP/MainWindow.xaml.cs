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
        private string _tempPath;
        public MainWindow()
        {
            InitializeComponent();
            string temp = "ftp://komphort.ru/";
            _Client = new FTP_Client("ftp://ftp.equation.com", 30000, false);
            if(_Client.CreateRequest())
                Data.ItemsSource = _Client.ListDirectory();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_Client.CreateRequest())
            {
                DataFile file = (DataFile)Data.SelectedItem;
                _Client.DownloadFile("D:\\", file.file_size, file.file_name);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

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
                Data.ItemsSource = _Client.ListDirectory();
        }

        private void Data_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataFile file = (DataFile)Data.SelectedItem;
            if(file.file_type == "dir")
                Data.ItemsSource = _Client.ChangeDirectory(file.file_name);
        }
    }
}
