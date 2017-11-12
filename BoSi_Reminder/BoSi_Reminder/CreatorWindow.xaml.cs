using System;
using System.Windows;
using Interface.Models;

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

        private CreatorViewModel CreatorViewModel { get; }

        private void Close(bool isQuitApp)
        {
            if (!isQuitApp)
                Close();
            else
            {
                Environment.Exit(0);
            }
        }

        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           

        }
    }
}
