using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace BoSi_Reminder
{
    class CabinetViewModel : INotifyPropertyChanged
    {
        
        private RelayCommand _logOutCommand;
        private RelayCommand _createCommand;
        private RelayCommand _deleteCommand;
        private RelayCommand _showLogInWindowCommand;
        public string UsernameBlockText { get; set; }
        public DateTime Date { get; set; }
        public string DateBlockContent { get; set; }
        //змінна для відслідковування чи увімкнений режим "Показати все"
        bool isDisplayAll = false;
        public Reminder SelectedReminder { get; set; }

        //при завантаженні вікна вводиться ім'я поточного користувача та сьогоднішню дату

        //список для збереження всіх нагадувань в ListBox
        public List<Reminder> _usersReminders;

        public CabinetViewModel()
        {
            UsersReminders = StationManager.CurrentUser?.Reminders?.Where(r => r.ReactDate.Date == Date).ToList();
            UsernameBlockText = StationManager.CurrentUser?.Name + " " + StationManager.CurrentUser?.Surname;
            Date = DateTime.Now.Date;
            DateBlockContent = DateTime.Now.ToString("dd/MM/yyyy");
        }

        /*
        //заповнення масиву для виділення дат, на які встановлено нагадування
        private void Fill()
        {
            foreach (var d in StationManager.CurrentUser?.Reminders)
                Calendar.SelectedDates.Add(d.ReactDate);
        }
        */

        public RelayCommand WindowLoaded
        {
            get
            {
                if (_showLogInWindowCommand == null)
                {
                    _showLogInWindowCommand = new RelayCommand(OnLoaded);
                }
                return _showLogInWindowCommand;
            }
        }

        private void OnLoaded(object obj)
        {
            TimeTracker.ShowPrevious();
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


        //відкриття вікна створення нагадування
        private void Create(Object obj)
        {
            LogWriter.LogWrite("Open create window");
            OnRequestClose(false);
            CreatorWindow creatorWindow = new CreatorWindow();
            creatorWindow.ShowDialog();
            
        }

        public List<Reminder> UsersReminders
        {
            get => _usersReminders;
            set
            {
                _usersReminders = value;
                OnPropertyChanged("_userReminders");
            }
        }
        //видалення обраного нагадування
        private void Delete(Object obj)
        {
            
            //пошук обраного нагадування зі списку користувача
            try
                {
               
                    //якщо нагадування існує то видаляємо
                    //в іншому випадку виводимо повідомлення про помилку
                    if (SelectedReminder != null)
                    {
                        StationManager.CurrentUser.Reminders.Remove(SelectedReminder);
                        SerializeManager.Serialize<User>(StationManager.CurrentUser);
                        //якщо не обрано режим "Показати все" то відображаємо нагадування за обраною датою
                        //в іншому випадку виводимо всі
                       // if (!isDisplayAll)
                         //   UsersReminders = StationManager.CurrentUser.Reminders?.Where(r => r.ReactDate.Date == Date).ToList();
                       // else
                        //{
                            UsersReminders = StationManager.CurrentUser.Reminders;
                            DateBlockContent = "";
                        //}
                    }
                    else
                    {
                        MessageBox.Show("Reminder not found!");
                    }

                }
                catch (Exception ex)
                {
                    LogWriter.LogWrite("Exception in DeleteReminder method", ex);
                }

                LogWriter.LogWrite("Delete reminder");
            
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
                LogWriter.LogWrite("Exception in Logout method, deleting the user file",e);
            }

            StationManager.CurrentUser = null;
            OnRequestClose(false);
            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
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

        /*
        RelayCommand _selectionCommand;
        public RelayCommand SelectionCommand
        {
            get
            {
                if (_selectionCommand == null)
                {
                    _selectionCommand = new RelayCommand(a =>
                    {
                        SelectedDatesCollection dates = a as SelectedDatesCollection;
                        dates.ToList();
                        foreach (var d in StationManager.CurrentUser?.Reminders)
                            dates.Add(d.ReactDate);
                    });

                }
                return _selectionCommand;
            }
        }
        */

    }
}
