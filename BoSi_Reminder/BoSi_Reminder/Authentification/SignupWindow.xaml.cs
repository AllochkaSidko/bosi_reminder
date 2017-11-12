using System;
using System.Windows;
using System.Windows.Controls;
using BoSi_Reminder.Interface.Models;
using FontAwesome.WPF;

namespace BoSi_Reminder.Authentification
{
    /// <summary>
    /// Interaction logic for SignupWindow.xaml
    /// </summary>
    public partial class SignupWindow : Window
    {
        private ImageAwesome _loader;
        private SignupViewModel SignupViewModel { get; set; }

        public SignupWindow()
        {
            InitializeComponent();
            SignupViewModel = new SignupViewModel(new User());
            SignupViewModel.RequestClose += Close;
            SignupViewModel.RequestLoader += OnRequestLoader;
            DataContext = SignupViewModel;
        }

        //поява лоардера
        private void OnRequestLoader(bool isShow)
        {
            //встановлення параметрів лоадера
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
