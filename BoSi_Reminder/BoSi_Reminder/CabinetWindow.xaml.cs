﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BoSi_Reminder
{
    /// <summary>
    /// Interaction logic for CabinetWindow.xaml
    /// </summary>
    public partial class CabinetWindow : Window
    {
        public CabinetWindow()
        {
            InitializeComponent();
            CabinetViewModel = new CabinetViewModel();
            CabinetViewModel.RequestClose += Close;
            DataContext = CabinetViewModel;
            

        }

        private CabinetViewModel CabinetViewModel { get; set; }
        //змінна для відслідковування чи увімкнений режим "Показати все"
        bool isDisplayAll = false;
       

        private void Close(bool isQuitApp)
        {
            if (!isQuitApp)
                this.Close();
            else
            {
                Environment.Exit(0);
            }
        }

        //при завантаженні вікна вводиться ім'я поточного користувача та сьогоднішню дату
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.UsernameBlock.Text = StationManager.CurrentUser?.Name + " " + StationManager.CurrentUser?.Surname;
            Fill();
            this.DateBlock.Content = DateTime.Now.ToString("dd/MM/yyyy");
            ListBox.ItemsSource = StationManager.CurrentUser?.SortRemindList()?.Where(r => r.ReactDate.Date == DateTime.Now.Date);


        }


        //заповнення масиву для виділення дат, на які встановлено нагадування
        private void Fill()
        {
            foreach (var d in StationManager.CurrentUser?.UsersReminders)
                Calendar.SelectedDates.Add(d.ReactDate);
        }



        //виводить нагадування за обраною на календарі датою та відображає цю дату в текстовому блоці
        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Calendar.SelectedDate != null)
                {
                    ListBox.ItemsSource = StationManager.CurrentUser?.SortRemindList()?.Where(r => r.ReactDate.Date == Calendar.SelectedDate.Value);
                    this.DateBlock.Content = this.Calendar.SelectedDate.Value.ToString("dd/MM/yyyy");

                }
                //-----------ВИПРАВЛЕНО ПОМИЛКУ-----------//
                Fill();
            }
            catch (Exception ex )
            {
                LogWriter.LogWrite("SelectedDatesChanged",ex);
            }

            LogWriter.LogWrite("Select remind item");

        }

        //видалення обраного нагадування
        private void DeleteReminder_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ListBox.SelectedIndex;

            //-----------ВИПРАВЛЕНО ПОМИЛКУ-----------//
            //пошук обраного нагадування зі списку користувача
            try
            {
                var reminder = StationManager.CurrentUser.SortRemindList()?.ElementAtOrDefault(selectedIndex);
                //якщо нагадування існує то видаляємо
                //в іншому випадку виводимо повідомлення про помилку
                if (reminder != null)
                {
                    StationManager.CurrentUser.UsersReminders.Remove(reminder);
                    SerializeManager.Serialize<User>(StationManager.CurrentUser);
                    //якщо не обрано режим "Показати все" то відображаємо нагадування за обраною датою
                    //в іншому випадку виводимо всі
                    if (!isDisplayAll)
                        ListBox.ItemsSource = StationManager.CurrentUser.SortRemindList()?.Where(r => r.ReactDate.Date == Calendar.SelectedDate.Value);
                    else
                    {
                        ListBox.ItemsSource = StationManager.CurrentUser.SortRemindList();
                        this.DateBlock.Content = "";
                    }
                }
                else
                {
                    MessageBox.Show("Reminder not found!");
                }

            }
            catch (Exception ex)
            {
                LogWriter.LogWrite("DeleteReminder", ex);
            }

            LogWriter.LogWrite("Delete reminder");
        }

        //позначення нагадування як виконаного(аналогічно видаленню)
        private void IsDoneButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ListBox.SelectedIndex;

            var item = ListBox.SelectedItem;
            //-----------ВИПРАВЛЕНО ПОМИЛКУ-----------//
            try
            {
                var reminder = StationManager.CurrentUser.SortRemindList()?.ElementAtOrDefault(selectedIndex);
                if (reminder != null)
                {
                    //присвоєння властивості isDone значення true
                    StationManager.CurrentUser.UsersReminders.SingleOrDefault(r => r.Id == reminder.Id).IsDone = true;
                    SerializeManager.Serialize<User>(StationManager.CurrentUser);
                    if (!isDisplayAll)
                        ListBox.ItemsSource = StationManager.CurrentUser.SortRemindList()?.Where(r => r.ReactDate.Date == Calendar.SelectedDate.Value);
                    else
                        ListBox.ItemsSource = StationManager.CurrentUser.SortRemindList();

                }
                else
                {
                    MessageBox.Show("Reminder not found!");
                }
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite("Reminder isDone", ex);
            }
            LogWriter.LogWrite("Done reminder");
        }

        //відобразити всі нагадування користувача
        private void DisplayAll_Click(object sender, RoutedEventArgs e)
        {
            
            Calendar.SelectedDate = null;
            isDisplayAll = true;
            ListBox.ItemsSource = StationManager.CurrentUser?.SortRemindList();
            this.DateBlock.Content = "";
            LogWriter.LogWrite("Display all reminders");
        }


        //---------------ВИПРАВЛЕНА ПОМИЛКА---------------//
        //відображення дати нагадування при натисненні на ньому в списку, коли увімкнений режим "Показати все"
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = ListBox.SelectedIndex;
            var reminder = StationManager.CurrentUser?.SortRemindList()?.ElementAtOrDefault(selectedIndex);

            if (isDisplayAll)
            {
                DateBlock.Content = reminder?.ReactDate.Date.ToString("dd/MM/yyyy");
            }
        }
    }
}
