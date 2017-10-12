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

        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //встановлення поточної дати за замовчуванням
            this.DatePicker.SelectedDate = DateTime.Now;

            //заповнення спадного списку для обирання годин
            List<string> data = new List<string>();
            for (int i = 0; i < 24; i++)
                data.Add(i.ToString("00"));
            Hours.ItemsSource = data;
            Hours.SelectedIndex = 0;

            //заповнення спадного списку для обирання хвилин
            List<string> data2 = new List<string>();
            for (int i = 0; i < 60; i++)
                data2.Add(i.ToString("00"));
            Minutes.ItemsSource = data2;
            Minutes.SelectedIndex = 0;

        }
    }
}
