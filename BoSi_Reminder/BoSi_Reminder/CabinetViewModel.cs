using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoSi_Reminder
{
    class CabinetViewModel : INotifyPropertyChanged
    {
        
        private RelayCommand _logOutCommand;
        private RelayCommand _createCommand;
        private RelayCommand _deleteCommand;
        private RelayCommand _remindCommand;


        //список для збереження всіх нагадувань в ListBox
        public List<Reminder> UsersReminders { get; set; }

        public CabinetViewModel()
        {
            UsersReminders = StationManager.CurrentUser?.SortRemindList();
        }

        public RelayCommand RemindCommand
        {
            get { return _remindCommand ?? (_remindCommand = new RelayCommand(obj => Remind(obj))); }
        }


        public RelayCommand LogOutCommand
        {
            get { return _logOutCommand ?? (_logOutCommand = new RelayCommand(LogOut)); }
        }

        public RelayCommand CreateCommand
        {
            get { return _createCommand ?? (_createCommand = new RelayCommand(obj => Create(obj))); }
        }

        public RelayCommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new RelayCommand(obj => Delete(obj))); }
        }



        //вікриття вікна створення нагадування
        private void Create(Object obj)
        {
            LogWriter.LogWrite("Open create window");
            OnRequestClose(false);
            CreatorWindow creatorWindow = new CreatorWindow();
            creatorWindow.ShowDialog();
            
        }

        private void Delete(Object obj)
        {

            //не реалізовано MVVM

        }

        private void Done(Object obj)
        {

            //не реалізовано MVVM

        }


        //вікриття початкового вікна
        private void LogOut(object obj)
        {
            LogWriter.LogWrite("Log out");
            try
            {
                File.Delete(SerializeManager.CreateAndGetPath(StationManager.CurrentUser.Filename));
            }
            catch(Exception e)
            {
                LogWriter.LogWrite("Logout method, deleting the user file");
                LogWriter.LogWrite(e.Message);
            }
            OnRequestClose(false);
            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }


        //імітація спрацьовування нагадувань
        private void Remind(Object obj)
        {
            //берем найближче нагадування
            Reminder first = StationManager.CurrentUser.SortRemindList()?.First();
            //якщо список пустий то надходить відповідне повідомлення 
            //в іншому випадку виводиться текст та дата нагадування
            if (first == null)
            {
                MessageBox.Show("You haven't got any reminders");
            }
            else
            {
               
                MessageBox.Show(first.Text, first.ReactDate.ToString());
            }

            LogWriter.LogWrite("Reminder immitation");
        }

        //метод для закриття вікна
        protected virtual void OnRequestClose(bool isquitapp)
        {
            RequestClose?.Invoke(isquitapp);
        }

        internal event CloseHandler RequestClose;
        public delegate void CloseHandler(bool isQuitApp);
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }




    }
}
