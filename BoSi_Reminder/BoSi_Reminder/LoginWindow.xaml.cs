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
