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
            try
            {

                if (!Directory.Exists(StaticResources.DirPath))
                    Directory.CreateDirectory(StaticResources.DirPath);

                var path = System.IO.Path.Combine(StaticResources.DirPath, User.FileName);
                if (!File.Exists(path))
                {
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.ShowDialog();
                }
                else
                {
                    var user = SerializeManager.Deserialize<User>(path);
                    if (DBAdapter.Users.Where(u => u.Login == user.Login) != null && String.IsNullOrEmpty(user.Login))
                    {
                        DBAdapter.Users.RemoveAll(u => u.Login == user.Login);
                        DBAdapter.Users.Add(user);
                    }
                    else
                    {
                        DBAdapter.Users.Add(user);
                    }

                    StationManager.CurrentUser = user;
                    CabinetWindow cabinetWindow = new CabinetWindow();
                    cabinetWindow.ShowDialog();

                }
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite("Initialize main window", ex);
            }
            InitializeComponent();


            //----------ВИПРАВИЛИ ПОМИЛКУ----------//
            Environment.Exit(0);

        }

    }
}