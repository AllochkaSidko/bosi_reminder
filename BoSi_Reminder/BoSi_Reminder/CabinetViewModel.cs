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
        private Reminder _selectedReminder;
        private RelayCommand _logOutCommand;
        private RelayCommand _createCommand;
        private RelayCommand _deleteCommand;
        private RelayCommand _doneCommand;
        private RelayCommand _remindCommand;

        public RelayCommand RemindCommand
        {
            get { return _remindCommand ?? (_remindCommand = new RelayCommand(obj => Remind(obj))); }
        }


        public RelayCommand LogoutCommand
        {
            get { return _logOutCommand ?? (_logOutCommand = new RelayCommand(obj => OnRequestClose(true))); }
        }

        public RelayCommand CreateCommand
        {
            get { return _createCommand ?? (_createCommand = new RelayCommand(obj => Create(obj))); }
        }

        public RelayCommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new RelayCommand(obj => Delete(obj))); }
        }

        public RelayCommand DoneCommand
        {
            get { return _doneCommand ?? (_doneCommand = new RelayCommand(obj => Done(obj))); }
        }


        private void Create(Object obj)
        {

            CreatorWindow creatorWindow = new CreatorWindow();
            creatorWindow.ShowDialog();
            
        }

        private void Delete(Object obj)
        {

            

        }

        private void Done(Object obj)
        {

           

        }

        public bool IsDone
        {
            get => _selectedReminder.IsDone;
            set
            {
                _selectedReminder.IsDone = value;
            }
        }

        private void Remind(Object obj)
        {

            Reminder first = StationManager.CurrentUser.SortRemindList().First<Reminder>();
           
            MessageBox.Show(first.ReactDate + first.Text);
           
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
