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
    class CreatorViewModel : INotifyPropertyChanged
    {

        private Reminder _newReminder;
        private RelayCommand _createCommand;
        private RelayCommand _closeCommand;

        public CreatorViewModel(Reminder newReminder)
        {
            this._newReminder = newReminder;
        }

        public RelayCommand CreateCommand
        {
            get
            {
                return _createCommand ?? (_createCommand = new RelayCommand(Create,
                           o => !String.IsNullOrEmpty(Date.ToString()) &&
                                !String.IsNullOrEmpty(Hours.ToString())&&
                                !String.IsNullOrEmpty(Minutes.ToString())&&
                                !String.IsNullOrEmpty(Text)));
            }
        }

        public String Text
        {
            get => _newReminder.Text;
            set
            {
                _newReminder.Text = value;
            }
        }
        

        public int Hours
        {
            get => _newReminder.ReactDate.Hour;
            set
            {
                _newReminder.ReactDate.AddHours(value);
            }
        }

        public int Minutes
        {
            get => _newReminder.ReactDate.Minute;
            set
            {
                _newReminder.ReactDate.AddMinutes(value);
            }
        }


        public DateTime Date
        {
            get => _newReminder.ReactDate.Date;
            set
            {
                _newReminder.ReactDate =value;
            }
        }


        private void Create(Object obj)
        {
            
            Date.AddHours(Hours);
            Date.AddMinutes(Minutes);
            if (DateTime.Now.CompareTo(Date)>0)
            {
                MessageBox.Show("You cannot set reminder or earlier date or time");
                return;
            }


            StationManager.CurrentUser.UsersReminders.Add(new Reminder(Date, Text));
            MessageBox.Show(Date + Text);

            CabinetWindow cabinetWindow = new CabinetWindow();
            cabinetWindow.ShowDialog();
            OnRequestClose(false);
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
