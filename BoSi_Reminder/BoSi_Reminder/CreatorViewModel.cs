using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BoSi_Reminder
{
    class CreatorViewModel : INotifyPropertyChanged
    {

        private Reminder _newReminder;
        private RelayCommand _createCommand;
        private RelayCommand _closeCommand;

        public CreatorViewModel(Reminder newReminder)
        {
            this._newReminder = newReminder;
        }


        public String Text
        {
            get => _newReminder.Text;
            set
            {
                _newReminder.Text = value;
            }
        }

        

        private void Create(Object obj)
        {
            _newReminder.Text = Text;
            StationManager.CurrentUser.UsersReminders.Add(_newReminder);

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
}
