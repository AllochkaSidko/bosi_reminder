using System;
using System.Windows;
using BoSi_Reminder.Interface.Models;


namespace BoSi_Reminder.Authentification
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            LoginViewModel = new LoginViewModel(new User());
            LoginViewModel.RequestClose += Close;
            DataContext = LoginViewModel;
        }


        private LoginViewModel LoginViewModel { get; set; }


        private void Password_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            LoginViewModel.Password = Password.Password;
        }



        private void Close(bool isQuitApp)
        {
            if (!isQuitApp)
                this.Close();
            else
            {
                Environment.Exit(0);
            }
        }


    }
}
