using System;
using System.IO;
using System.Windows;
using BoSi_Reminder.Authentification;
using DBAdapter;
using Interface;
using Interface.Models;
using Tools;

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
                var user = SerializeManager.Deserialize<User>(User.FileName);

                //якщо не існує файлу з серіалізованим користувачем, то ми відкриваємо вікно Логіну
                if (user==null)
                {
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.ShowDialog();
                }
                //в іншому випадку користувача десеріалізують
                else
                {
                    var curUser = BoSiReminderService_Wrapper.GetUser(user.Id);
                    if (curUser == null)
                    {
                        LoginWindow loginWindow = new LoginWindow();
                        loginWindow.ShowDialog();
                    }
                    else
                    {
                        curUser.PreviousLog = DateTime.Now;
                        BoSiReminderService_Wrapper.EditUser(curUser);

                        //встановлюємо поточного користувача та відриваємо вікно Кабінету
                        StationManager.CurrentUser = curUser;
                        CabinetWindow cabinetWindow = new CabinetWindow();
                        cabinetWindow.ShowDialog();
                    }

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