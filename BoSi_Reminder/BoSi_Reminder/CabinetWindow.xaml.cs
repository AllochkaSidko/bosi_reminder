using System;
using System.Windows;
using BoSi_Reminder.Authentification;
using DBAdapter;
using Interface;
using Interface.Models;
using Tools;


namespace BoSi_Reminder
{
    /// <summary>
    /// Interaction logic for CabinetWindow.xaml
    /// </summary>
    public partial class CabinetWindow : Window
    {

        private CabinetViewModel CabinetViewModel { get; set; }

        public CabinetWindow()
        {
            InitializeComponent();

            try
            {
                var user = SerializeManager.Deserialize<User>(User.FileName);

                //якщо не існує файлу з серіалізованим користувачем, то ми відкриваємо вікно Логіну
                if (user == null)
                {
                    OpenLoginWindow();
                }
                //в іншому випадку користувача десеріалізують
                else
                {
                    var curUser = BoSiReminderService_Wrapper.GetUser(user.Id);
                    if (curUser == null)
                    {
                        OpenLoginWindow();
                    }
                    else
                    {
                        curUser.PreviousLog = DateTime.Now;
                        BoSiReminderService_Wrapper.EditUser(curUser);

                        //встановлюємо поточного користувача та відриваємо вікно Кабінету
                        StationManager.CurrentUser = curUser;
                        InitializeCabinet();
                    }

                }
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite("Exception while Initializing main window", ex);
            }
        }

        
        private void OpenLoginWindow()
        {

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Closed += (sender, args) => InitializeCabinet();
            loginWindow.ShowDialog();

        }

        private void InitializeCabinet()
        {
            if (StationManager.CurrentUser == null)
                Environment.Exit(0);

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CabinetViewModel = new CabinetViewModel();
            CabinetViewModel.RequestClose += Close;
            CabinetViewModel.FillDates += Fill;
            CabinetViewModel.RequestVisibilityChange += (x) => Visibility = x;
            CabinetViewModel.OpenLogin += OpenLoginWindow;

            DataContext = CabinetViewModel;
        }
        private void Close(bool isQuitApp)
        {
            if (!isQuitApp)
                Close();
            else
            {
                Environment.Exit(0);
            }
        }
        
        //заповнення масиву для виділення дат, на які встановлено нагадування
        private void Fill()
        {
            try
            {
                foreach (var d in BoSiReminderService_Wrapper.GetAllRemindsCurrUser(StationManager.CurrentUser))
                    Calendar.SelectedDates.Add(d.ReactDate);
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite("Exception in Fill() method while getting reminders", ex);
            }
        }
    }
}
