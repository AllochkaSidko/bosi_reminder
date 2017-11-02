using System;
using System.IO;
using System.Linq;
using System.Windows;


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

                var path = System.IO.Path.Combine(StaticResources.DirPath, User.FileName);
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
                    //якщо користувач існує в ДБАдаптері, то ми видаляємо цього користувача та записуємо десеріалізованого
                    //щоб не було конфліктів з доданими нагадуваннями
                   // if (DBAdapter.Users.Where(u => u.Login == user.Login) != null && String.IsNullOrEmpty(user.Login))
                  //  {
                  //      DBAdapter.Users.RemoveAll(u => u.Login == user.Login);
                   //     DBAdapter.Users.Add(user);
                   // }
                    //в іншому випадку додаємо користувача до ДБАдаптера
                    //else
                   // {
                   //     DBAdapter.Users.Add(user);
                   // }
                    //встановлюємо поточного користувача та відриваємо вікно Кабінету
                    StationManager.CurrentUser = user;
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