using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoSi_Reminder
{
    class LoginViewModel : INotifyPropertyChanged
    {

        private User _userCandidate;
        private RelayCommand _signInCommand;
        private RelayCommand _closeCommand;
        private RelayCommand _toSignUpCommand;



        public LoginViewModel(User userCandidate)
        {
            this._userCandidate = userCandidate;
        }


        public RelayCommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(obj => OnRequestClose(true))); }
        }

        //перевірка чи заповнені поля для логіна і паролю
        public RelayCommand SignInCommand
        {
            get
            {
                return _signInCommand ?? (_signInCommand = new RelayCommand(SignIn,
                           o => !String.IsNullOrEmpty(Login) &&
                                !String.IsNullOrEmpty(Password)));
            }
        }

        //викликається метод SignUp
        public RelayCommand SignUpCommand
        {
            get
            {
                return _toSignUpCommand ?? (_toSignUpCommand = new RelayCommand(obj => SignUp(obj)));
            }
        }

        
        internal String Password
        {
            get => _userCandidate.Password;
            set
            {
                _userCandidate.Password = value;
            }
        }
        public String Login
        {
            get => _userCandidate.Login;
            set
            {
                _userCandidate.Login = value;
                OnPropertyChanged();
            }
        }


        //метод для входу користувача в систему
        private void SignIn(Object obj)
        {
            //перевірка чи є існує такий користувач
            var currentUser = DBAdapter.Users.FirstOrDefault(user => user.Login == Login &&
                                                                     user.Password == User.Hash(Password));
            if (currentUser == null)
            {
                MessageBox.Show("Wrong Username or Password");
                return;
            }

            //записуємопоточного користувача
            StationManager.CurrentUser = currentUser;

            SerializeManager.Serialize<User>(StationManager.CurrentUser);

            OnRequestClose(false);

            //переходимо на вікно Кабінету 
            CabinetWindow cabinetWindow = new CabinetWindow();
            cabinetWindow.ShowDialog();
        }

        //вікриваємо вікно Sign Up
        private void SignUp(Object obj)
        {
            OnRequestClose(false);
            SignupWindow signupWindow = new SignupWindow();
            signupWindow.ShowDialog();
            
        }


        internal event CloseHandler RequestClose;
        public delegate void CloseHandler(bool isQuitApp);


        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnRequestClose(bool isquitapp)
        {
            RequestClose?.Invoke(isquitapp);
        }
    }
}
