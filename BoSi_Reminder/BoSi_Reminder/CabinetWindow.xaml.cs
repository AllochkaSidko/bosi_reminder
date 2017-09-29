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



            StationManager.CurrentUser.UsersReminders.Add(new Reminder(new DateTime(2017,10,13),"Alla"));
            StationManager.CurrentUser.UsersReminders.Add(new Reminder(new DateTime(2017, 11, 17), "Alla2"));
            foreach (var k in StationManager.CurrentUser.UsersReminders)
            {
                Grid grid = new Grid();

                Label label = new Label();
                label.Content = k.ReactDate;
                label.Margin = new Thickness(10, 18, 0, 0);
                label.FontSize = 17;

                TextBox textbox = new TextBox();
                textbox.Text = k.Text;
                textbox.Margin = new Thickness(100, 20, 0, 0);
                textbox.Height = 60;
                textbox.Width = 400;

                grid.Children.Add(label);
                grid.Children.Add(textbox);
                ListBox.Items.Add(grid);


            }
           
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DateBlock.Content = this.Calendar.SelectedDate.Value.ToString("dd/MM/yyyy");
        }
    }
}
