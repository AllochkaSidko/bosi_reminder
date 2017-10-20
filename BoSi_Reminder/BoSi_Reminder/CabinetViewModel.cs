using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private RelayCommand _isDoneCommand;
        private RelayCommand _showLogInWindowCommand;
        private RelayCommand _displayAllCommand;
        private RelayCommand _selectedDatesChangedCommand;
        private RelayCommand _listBoxSelectionChanged;

        public string UsernameBlockText { get; set; }
        public DateTime? Date { get; set; }

        private string _dateBlockContent;
        public string DateBlockContent {
            get => _dateBlockContent;
            set
            {
                _dateBlockContent = value;
                OnPropertyChanged();
            }
        }
        //змінна для відслідковування чи увімкнений режим "Показати все"
        bool isDisplayAll = false;
        public Reminder SelectedReminder { get; set; }
        //список для збереження всіх нагадувань в ListBox

        private List<Reminder> _usersReminders;
        public List<Reminder> UsersReminders
        {
            get => _usersReminders;
            set
            {
                _usersReminders = value;
                OnPropertyChanged();
            }
        }

        //при завантаженні вікна вводиться ім'я поточного користувача та сьогоднішню дату
        public CabinetViewModel()
        {
            UsersReminders = StationManager.CurrentUser?.Reminders?.Where(r => r.ReactDate.Date == DateTime.Today).ToList();
            UsernameBlockText = StationManager.CurrentUser?.Name + " " + StationManager.CurrentUser?.Surname;
            Date = DateTime.Now.Date;
            DateBlockContent = DateTime.Now.ToString("dd/MM/yyyy");
        }


        /*
         //відображення дати нагадування при натисненні на ньому в списку, коли увімкнений режим "Показати все"
         private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             Reminder reminder = (Reminder)ListBox.SelectedItem;

             // if (isDisplayAll)
             // {
             //   DateBlock.Content = reminder?.ReactDate.Date.ToString("dd/MM/yyyy");
             //}
         }*/

        public RelayCommand ListBoxSelectionChanged
        {
            get
            {
                if (_listBoxSelectionChanged == null)
                {
                    _listBoxSelectionChanged = new RelayCommand(SelectionChangedItem);
                }
                return _listBoxSelectionChanged;
            }
        }

        private void SelectionChangedItem(Object obj)
        {
            if (isDisplayAll)
            {
                DateBlockContent = SelectedReminder?.ReactDate.Date.ToString("dd/MM/yyyy");
            }
        }


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
            OnRequestFillDates();
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

        public RelayCommand IsDoneCommand
        {
            get { return _isDoneCommand ?? (_isDoneCommand = new RelayCommand(obj => Done(obj))); }
        }
        
        //відкриття вікна створення нагадування
        private void Create(Object obj)
        {
            LogWriter.LogWrite("Open create window");
            OnRequestClose(false);
            CreatorWindow creatorWindow = new CreatorWindow();
            creatorWindow.ShowDialog();
        }
        
        public RelayCommand DisplayAllCommand
        {
            get { return _displayAllCommand ?? (_displayAllCommand = new RelayCommand(obj => DisplayAll(obj))); }
        }

        //відобразити всі нагадування користувача
        private void DisplayAll(Object obj)
        {
            Date = null;
            isDisplayAll = true;
            UsersReminders = StationManager.CurrentUser?.Reminders;
            OnRequestUpdateList();
            DateBlockContent = "";
            LogWriter.LogWrite("Display all reminders");
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
                    UsersReminders.Remove(SelectedReminder);
                    SerializeManager.Serialize<User>(StationManager.CurrentUser);

                    OnRequestUpdateList();
                    if (isDisplayAll)
                        DateBlockContent = "";
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
            try
            {
                if (SelectedReminder != null)
                {
                    //присвоєння властивості isDone значення true
                    SelectedReminder.IsDone = true;
                    SerializeManager.Serialize<User>(StationManager.CurrentUser);
                    OnRequestUpdateList();
                }
                else
                {
                    MessageBox.Show("Reminder not found!");
                }
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite("Exception in isDone method", ex);
            }
            LogWriter.LogWrite("Done reminder");
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



        public RelayCommand SelectedDatesChangedCommand
        {
            get
            {
                if (_selectedDatesChangedCommand == null)
                {
                    _selectedDatesChangedCommand = new RelayCommand(DatesChanged);
                }
                return _selectedDatesChangedCommand;
            }
        }

        //виводить нагадування за обраною на календарі датою та відображає цю дату в текстовому блоці
        private void DatesChanged(Object obj)
        {
            try
            {
                if (Date != null)
                {
                    UsersReminders = StationManager.CurrentUser?.Reminders?.Where(r => r.ReactDate.Date == Date.Value).ToList();
                    OnRequestUpdateList();
                    DateBlockContent = Date.Value.ToString("dd/MM/yyyy");
                }

                OnRequestFillDates();
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite("Exception in SelectedDatesChanged method", ex);
            }

            LogWriter.LogWrite("Select remind item");
        }

     

        internal event UpdateListHandler UpdateList;
        public delegate void UpdateListHandler();

        //метод для оновлення списку
        protected virtual void OnRequestUpdateList()
        {
            UpdateList?.Invoke();
        }


        internal event FillDatesHandler FillDates;
        public delegate void FillDatesHandler();

        //метод для оновлення списку
        protected virtual void OnRequestFillDates()
        {
            FillDates?.Invoke();
        }

        internal event CloseHandler RequestClose;
        public delegate void CloseHandler(bool isQuitApp);

        //метод для закриття вікна
        protected virtual void OnRequestClose(bool isquitapp)
        {
            RequestClose?.Invoke(isquitapp);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
