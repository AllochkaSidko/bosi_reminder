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
        //private RelayCommand _deleteCommand;


       

        public RelayCommand LogoutCommand
        {
            get { return _logOutCommand ?? (_logOutCommand = new RelayCommand(obj => OnRequestClose(true))); }
        }

        public RelayCommand CreateCommand
        {
            get { return _createCommand ?? (_createCommand = new RelayCommand(obj => Create(obj))); }
        }


        private void Create(Object obj)
        {

            CreatorWindow creatorWindow = new CreatorWindow();
            creatorWindow.ShowDialog();
            
        }

        public bool IsDone
        {
            get => _selectedReminder.IsDone;
            set
            {
                _selectedReminder.IsDone = value;
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
