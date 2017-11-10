using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using BoSi_Reminder.Interface.Models;
using BoSi_Reminder.Tools;
using BoSi_Reminder.DBAdapter;
using BoSi_Reminder.Authentification;

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
        public DateTime? _date;
        //список для збереження всіх нагадувань в ListBox
        private List<Reminder> _usersReminders;
        private string _dateBlockContent;
        //змінна для відслідковування чи увімкнений режим "Показати все"
        bool isDisplayAll = false;
        public Reminder SelectedReminder { get; set; }

        //при завантаженні вікна вводиться ім'я поточного користувача та сьогоднішню дату
        public CabinetViewModel()
        {
            try
            {
                //звертаємось до бази, щоб вивести нагадування поточного користувача
                UsersReminders = EntityWraper.GetAllRemindsCurrUser(StationManager.CurrentUser)?.Where(r => r.ReactDate.Date == DateTime.Today).ToList();
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite("Exception in CabinetViewModel constructor while getting reminders", ex);
            }
            UsernameBlockText = StationManager.CurrentUser?.Name + " " + StationManager.CurrentUser?.Surname;
            Date = DateTime.Now.Date;
            DateBlockContent = DateTime.Now.ToString("dd/MM/yyyy");
        }


        public string DateBlockContent {
            get => _dateBlockContent;
            set
            {
                _dateBlockContent = value;
                OnPropertyChanged();
            }
        }
        public DateTime? Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        public List<Reminder> UsersReminders
        {
            get => _usersReminders;
            set
            {
                _usersReminders = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ListBoxSelectionChanged
        {
            get{ return _listBoxSelectionChanged ?? (_listBoxSelectionChanged = new RelayCommand(SelectionChangedItem)); }
        }

        public RelayCommand WindowLoaded
        {
            get { return _showLogInWindowCommand ?? (_showLogInWindowCommand = new RelayCommand(OnLoaded)); }
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

        public RelayCommand DisplayAllCommand
        {
            get { return _displayAllCommand ?? (_displayAllCommand = new RelayCommand(obj => DisplayAll(obj))); }
        }

        public RelayCommand SelectedDatesChangedCommand
        {
            get { return _selectedDatesChangedCommand ?? (_selectedDatesChangedCommand = new RelayCommand(DatesChanged)); }
        }

        //відкриття вікна створення нагадування
        private void Create(Object obj)
        {
            LogWriter.LogWrite("Open create window");
            OnRequestClose(false);
            CreatorWindow creatorWindow = new CreatorWindow();
            creatorWindow.ShowDialog();
        }

        //зміна дати в лейблі при зміні елементу ListBox
        private void SelectionChangedItem(Object obj)
        {
            if (isDisplayAll)
            {
                DateBlockContent = SelectedReminder?.ReactDate.Date.ToString("dd/MM/yyyy");
            }
        }

        //метод завантаження вікна
        private void OnLoaded(object obj)
        {
            try
            {
                OnRequestFillDates();
                TimeTracker.ShowPrevious();
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite("Exception in OnLoaded cabinet window method", ex);
            }
        }

        //відобразити всі нагадування користувача
        private void DisplayAll(Object obj)
        {
            Date = null;
            isDisplayAll = true;
            try
            {
                //звертаємось о бд, щоб показати всі нагадування почного користувача
                UsersReminders = EntityWraper.GetAllRemindsCurrUser(StationManager.CurrentUser);
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite("Exception in DisplayAll method while getting reminders", ex);
            }
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
                    UsersReminders.Remove(SelectedReminder);
                    //звертаємось до бд, щоб видалити обране нагадування
                    EntityWraper.Delete(SelectedReminder);

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

        //позначення нагадування як зробленого
        private void Done(Object obj)
        {
            try
            {
                if (SelectedReminder != null)
                {
                    //присвоєння властивості isDone значення true
                    SelectedReminder.IsDone = true;
                    //звертаємос до бд, щоб змінити поле IDone в обраного нагадуання
                    EntityWraper.Edit(SelectedReminder);

                    //оновлення ListBox 
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

        //вихід з персонального кабінету
        private void LogOut(object obj)
        {
            LogWriter.LogWrite("Log out");
            try
            {
                //видалення файлу з серіалізацією користувача
                File.Delete(SerializeManager.CreateAndGetPath(StationManager.CurrentUser.Filename));
            }
            catch(Exception e)
            {
                LogWriter.LogWrite("Exception in Logout method, deleting the user file",e);
            }

            StationManager.CurrentUser = null;
            OnRequestClose(false);
            //вікриття вікна логіну
            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }

        //виводить нагадування за обраною на календарі датою та відображає цю дату в текстовому блоці
        private void DatesChanged(Object obj)
        {
            try
            {
                if (Date != null)
                {
                    UsersReminders = EntityWraper.GetAllRemindsCurrUser(StationManager.CurrentUser).Where(r => r.ReactDate.Date == Date.Value).ToList();
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

        //метод для позначення дат на календарі, на які встановлено нагадування
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
