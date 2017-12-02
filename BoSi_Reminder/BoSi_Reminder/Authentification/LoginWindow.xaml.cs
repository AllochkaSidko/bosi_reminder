using System;
using System.Windows;
using System.Windows.Controls;
using FontAwesome.WPF;
using Interface.Models;


namespace BoSi_Reminder.Authentification
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginViewModel LoginViewModel { get; set; }
        private ImageAwesome _loader;

        public LoginWindow()
        {
            InitializeComponent();
            LoginViewModel = new LoginViewModel(new User());
            LoginViewModel.RequestClose += Close;
            LoginViewModel.RequestLoader += OnRequestLoader;
            //LoginViewModel.RequestVisibilityChange += (x) => Visibility = x;
            LoginViewModel.RequestVisibilityChange += VisibilityLogin;
            DataContext = LoginViewModel;
        }


        private void VisibilityLogin(bool v)
        {
            if (!v)
                this.Hide();
            else
            {
                if (StationManager.CurrentUser != null)
                    this.Close();
                else
                    this.ShowDialog();
            }
                
        }
        //поява лоардера
        private void OnRequestLoader(bool isShow)
        {
            //встановлення його параметрів
            if (isShow && _loader == null)
            {
                _loader = new ImageAwesome();
                MainGrid.Children.Add(_loader);
                _loader.Icon = FontAwesomeIcon.Spinner;
                _loader.Spin = true;
                _loader.Width = 35;
                _loader.Height = 35;
                Grid.SetRow(_loader, 0);
                Grid.SetColumn(_loader, 0);
                Grid.SetColumnSpan(_loader, 3);
                Grid.SetRowSpan(_loader, 3);
                _loader.HorizontalAlignment = HorizontalAlignment.Center;
                _loader.VerticalAlignment = VerticalAlignment.Center;
                IsEnabled = false;
            }
            else if (_loader != null)
            {
                MainGrid.Children.Remove(_loader);
                _loader = null;
                IsEnabled = true;
            }
        }

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
