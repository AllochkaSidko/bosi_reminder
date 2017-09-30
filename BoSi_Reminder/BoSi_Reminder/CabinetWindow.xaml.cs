using System;
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

       

        private void Close(bool isQuitApp)
        {
            if (!isQuitApp)
                this.Close();
            else
            {
                Environment.Exit(0);
            }
        }
        
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.UsernameBlock.Text = StationManager.CurrentUser.Name + " " + StationManager.CurrentUser.Surname;
            Fill();
            this.DateBlock.Content = DateTime.Now.ToString("dd/MM/yyyy");


        }

        private void Fill()
        {
            foreach (var d in StationManager.CurrentUser.UsersReminders)
                Calendar.SelectedDates.Add(d.ReactDate);
        }


       

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            
            ListBox.ItemsSource = StationManager.CurrentUser.SortRemindList()?.Where(r=>r.ReactDate.Date==Calendar.SelectedDate.Value);
            Fill();
            this.DateBlock.Content = this.Calendar.SelectedDate.Value.ToString("dd/MM/yyyy");

        }

        private void DeleteReminder_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ListBox.SelectedIndex;
            var reminder = StationManager.CurrentUser.SortRemindList().ElementAtOrDefault(selectedIndex);
            if (reminder != null)
            {
                StationManager.CurrentUser.UsersReminders.Remove(reminder);
                ListBox.ItemsSource = StationManager.CurrentUser.SortRemindList()?.Where(r => r.ReactDate.Date == Calendar.SelectedDate.Value);
            }
            else
            {
                MessageBox.Show("Reminder not found!");
            }
           
            
        }

        private void IsDoneButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ListBox.SelectedIndex;

           
            var item = ListBox.SelectedItem;
            var reminder = StationManager.CurrentUser.SortRemindList().ElementAtOrDefault(selectedIndex);
            if (reminder != null)
            {
                StationManager.CurrentUser.UsersReminders.SingleOrDefault(r => r.Id == reminder.Id).IsDone = true;
                ListBox.ItemsSource = StationManager.CurrentUser.SortRemindList()?.Where(r => r.ReactDate.Date == Calendar.SelectedDate.Value);
            }
            else
            {
                MessageBox.Show("Reminder not found!");
            }

        }
    }
}
