using System;
using System.IO;
using System.Windows;
using BoSi_Reminder.Interface.Models;
using BoSi_Reminder.Tools;
using BoSi_Reminder.DBAdapter;
using BoSi_Reminder.Authentification;

namespace BoSi_Reminder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            try
            {
                RemindTimer.Start();

                if (!Directory.Exists(StaticResources.DirPath))
                    Directory.CreateDirectory(StaticResources.DirPath);

                var path = Path.Combine(StaticResources.DirPath, User.FileName);
                //якщо не існує файлу з серіалізованим користувачем, то ми відкриваємо вікно Логіну
                if (!File.Exists(path))
                {
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.ShowDialog();
                }
                //в іншому випадку користувача десеріалізують
                else
                {
                    var user = SerializeManager.Deserialize<User>(path);
                    var curUser = EntityWraper.GetUser(user.Id);
                    curUser.PreviousLog = DateTime.Now;
                    EntityWraper.EditUser(curUser);
                    //встановлюємо поточного користувача та відриваємо вікно Кабінету
                    StationManager.CurrentUser = curUser;
                    CabinetWindow cabinetWindow = new CabinetWindow();
                    cabinetWindow.ShowDialog();

                }
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite("Exception while Initializing main window", ex);
            }
           
            Environment.Exit(0);

        }

       
    }
}