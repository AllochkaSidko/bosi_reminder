﻿using System;
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

        public RelayCommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(Close)); }
        }
        public String Text
        {
            get => _newReminder.Text;
            set
            {
                _newReminder.Text = value;
            }
        }

        int _hours;
        public int Hours
        {
            get => _hours;
            set
            {
                _hours = Convert.ToInt32(value);
            }
        }
        int _minutes;
        public int Minutes
        {
            get => _minutes;
            set
            {
                _minutes = Convert.ToInt32(value);
            }
        }


        public DateTime Date
        {
            get => _newReminder.ReactDate.Date;
            set
            {
                _newReminder.ReactDate = value;
            }
        }


        private void Create(Object obj)
        {
            var date = new DateTime(Date.Year, Date.Month, Date.Day, Hours, Minutes, 0);
            if (DateTime.Now.CompareTo(date)>0)
            {
                MessageBox.Show("You cannot set reminder or earlier date or time");
                return;
            }
            
            StationManager.CurrentUser.UsersReminders.Add(new Reminder(date, Text));
            
            OnRequestClose(false);
            CabinetWindow cabinetWindow = new CabinetWindow();
            cabinetWindow.ShowDialog();
        }

        private void Close(Object obj)
        {
            OnRequestClose(false);
            CabinetWindow cabinetWindow = new CabinetWindow();
            cabinetWindow.ShowDialog();

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
