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
            this.DateBlock.Content = DateTime.Now.ToString("dd/MM/yyyy");
            this.UsernameBlock.Text = StationManager.CurrentUser.Name + " " + StationManager.CurrentUser.Surname;
            Fill();
           
        }

        private void Fill()
        {
            foreach (var k in StationManager.CurrentUser.SortRemindList())
            {
                Grid grid = new Grid();

                Label label = new Label();
                label.Content = k.ReactDate;
                label.Margin = new Thickness(10, 18, 0, 0);
                label.FontSize = 17;
                label.Foreground = System.Windows.Media.Brushes.Orange;

                TextBox textbox = new TextBox();
                textbox.Text = k.Text;
                textbox.Margin = new Thickness(110, 20, 0, 0);
                textbox.Height = 60;
                textbox.Width = 330;
                textbox.Foreground = System.Windows.Media.Brushes.DodgerBlue;
                textbox.BorderBrush = System.Windows.Media.Brushes.SlateBlue;

                grid.Children.Add(label);
                grid.Children.Add(textbox);
                ListBox.Items.Add(grid);


            }



        }


       

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DateBlock.Content = this.Calendar.SelectedDate.Value.ToString("dd/MM/yyyy");
        }

        private void DeleteReminder_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ListBox.SelectedIndex;
            var reminder = StationManager.CurrentUser.SortRemindList().ElementAtOrDefault(selectedIndex);
            if (reminder != null)
            {
                StationManager.CurrentUser.UsersReminders.Remove(reminder);
                ListBox.Items.RemoveAt(selectedIndex);
            }
            else
            {
                MessageBox.Show("Reminder not found!");
            }
           
            
        }

        private void IsDoneButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ListBox.SelectedIndex;
            var reminder = StationManager.CurrentUser.SortRemindList().ElementAtOrDefault(selectedIndex);
            if (reminder != null)
            {
                StationManager.CurrentUser.UsersReminders.SingleOrDefault(r => r.Id == reminder.Id).IsDone = true;
                var current = ListBox.Items[selectedIndex];
                ListBox.Items[selectedIndex] = "DONE!";
            }
            else
            {
                MessageBox.Show("Reminder not found!");
            }

        }
    }
}
