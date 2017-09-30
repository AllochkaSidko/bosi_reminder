using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public List<Reminder> UsersReminders { get; set; }

        public CabinetViewModel()
        {
            UsersReminders = StationManager.CurrentUser.SortRemindList();
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

       


        private void Create(Object obj)
        {
            OnRequestClose(false);
            CreatorWindow creatorWindow = new CreatorWindow();
            creatorWindow.ShowDialog();
            
        }

        private void Delete(Object obj)
        {

            

        }

        private void Done(Object obj)
        {

           

        }

        private void LogOut(object obj)
        {
            OnRequestClose(false);
            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }

      

        private void Remind(Object obj)
        {

            Reminder first = StationManager.CurrentUser.SortRemindList()?.First();
            if (first == null)
            {
                MessageBox.Show("You haven't any reminders");
            }
            else
            {
               
                MessageBox.Show(first.Text, first.ReactDate.ToString());
            }
        }
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
