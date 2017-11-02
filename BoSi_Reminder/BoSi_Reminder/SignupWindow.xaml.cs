using System;
using System.Windows;


namespace BoSi_Reminder
{
    /// <summary>
    /// Interaction logic for SignupWindow.xaml
    /// </summary>
    public partial class SignupWindow : Window
    {
        public SignupWindow()
        {
            InitializeComponent();
            SignupViewModel = new SignupViewModel(new User());
            SignupViewModel.RequestClose += Close;
            DataContext = SignupViewModel;
        }


        private SignupViewModel SignupViewModel { get; set; }

        private void Password_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            SignupViewModel.Password = Password.Password;
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
