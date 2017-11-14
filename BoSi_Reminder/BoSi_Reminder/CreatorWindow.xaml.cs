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
        private CreatorViewModel CreatorViewModel { get; }

        public CreatorWindow()
        {
            InitializeComponent();
            CreatorViewModel = new CreatorViewModel(new Reminder());
            CreatorViewModel.RequestClose += Close;
            DataContext = CreatorViewModel;
        }

       
    }
}
