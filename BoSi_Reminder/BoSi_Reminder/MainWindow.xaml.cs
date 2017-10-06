using System;
using System.Collections.Generic;
using System.IO;
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

namespace BoSi_Reminder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            

            if (!Directory.Exists(SerializeManager.DirPath))
                Directory.CreateDirectory(SerializeManager.DirPath);

            var path = System.IO.Path.Combine(SerializeManager.DirPath, User.FileName);
            if (!File.Exists(path))
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
            }
            else
            {
                StationManager.CurrentUser = SerializeManager.Deserialize<User>(path);
                CabinetWindow cabinetWindow = new CabinetWindow();
                cabinetWindow.ShowDialog();
  
            }
            InitializeComponent();


            //----------ВИПРАВИЛИ ПОМИЛКУ----------//
            Environment.Exit(0);

        }

    }
}