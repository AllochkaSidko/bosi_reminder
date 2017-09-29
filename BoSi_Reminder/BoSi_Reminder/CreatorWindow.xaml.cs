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
    /// Interaction logic for CreatorWindow.xaml
    /// </summary>
    public partial class CreatorWindow : Window
    {
        public CreatorWindow()
        {
            InitializeComponent();
            CreatorViewModel = new CreatorViewModel(new Reminder());
            CreatorViewModel.RequestClose += Close;
            DataContext = CreatorViewModel;
        }

        private CreatorViewModel CreatorViewModel { get; set; }

        private void Close(bool isQuitApp)
        {
            if (!isQuitApp)
                this.Close();
            else
            {
                Environment.Exit(0);
            }
        }

        private void Hours_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            string value = comboBox.SelectedItem as string;
            this.Title = "Selected: " + value;
        }

        private void Minutes_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> data = new List<string>();

            for (int i = 0; i < 10; i++)
                data.Add("0"+i.ToString());
            for (int i = 10; i<60; i++)
            data.Add(i.ToString());

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

            // ... Make the first item selected.
            comboBox.SelectedIndex = 0;
        }

        private void Minutes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            string value = comboBox.SelectedItem as string;
            this.Title = "Selected: " + value;
        }

        private void Hours_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> data = new List<string>();

            for (int i = 0; i < 24; i++)
                data.Add(i.ToString());

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

            // ... Make the first item selected.
            comboBox.SelectedIndex = 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DatePicker.SelectedDate = DateTime.Now;
        }
    }
}
