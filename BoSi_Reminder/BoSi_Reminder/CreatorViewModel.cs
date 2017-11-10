using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using BoSi_Reminder.Interface.Models;
using BoSi_Reminder.Tools;
using BoSi_Reminder.DBAdapter;

namespace BoSi_Reminder
{
    class CreatorViewModel : INotifyPropertyChanged
    {

        private Reminder _newReminder;
        private RelayCommand _createCommand;
        private RelayCommand _closeCommand;
        public List<string> HoursList { get; set; }
        public List<string> MinutesList { get; set; }

        public CreatorViewModel(Reminder newReminder)
        {
            this._newReminder = newReminder;
            HoursList = new List<string>();
            MinutesList = new List<string>();
            //заповнення спадного списку для обирання годин
            for (int i = 0; i < 24; i++)
                HoursList.Add(i.ToString("00"));
            //заповнення спадного списку для обирання хвилин
            for (int i = 0; i < 60; i++)
               MinutesList.Add(i.ToString("00"));
            Date = DateTime.Now;
        }

        //перевірка чи заповнені всі поля
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

        //метод створення нагадування
        private void Create(Object obj)
        {
            var date = new DateTime(Date.Year, Date.Month, Date.Day, Hours, Minutes, 0);
            //перевірка чи встановлена дата нагадування не раніше поточної
            if (DateTime.Now.CompareTo(date)>0)
            {
                MessageBox.Show("You cannot set reminder on earlier date or time");
                return;
            }
            //to do: split in two trys, problem in log writer no exception message
            try
            {
                var reminder = new Reminder(date, Text, StationManager.CurrentUser);
                //додвання новостворенного нагадування до списку нагадувань користувача на сьогодні
                if (date.Date == DateTime.Now.Date)
                    TimeTracker.TodayReminds.Add(reminder);
                //додаємо нове нагадування користувачу
                EntityWraper.AddReminder(reminder);
            }
            catch(Exception e)
            {
                LogWriter.LogWrite("Exception in Create reminder method, adding reminder to user", e);
            }

            LogWriter.LogWrite("Created reminder");
            //перехід назад на вікно Кабінету
            OnRequestClose(false);
            CabinetWindow cabinetWindow = new CabinetWindow();
            cabinetWindow.ShowDialog();
        }

        private void Close(Object obj)
        {
            LogWriter.LogWrite("Close creator window");
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
