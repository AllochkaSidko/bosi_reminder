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

        public RelayCommand SignInCommand
        {
            get
            {
                return _signInCommand ?? (_signInCommand = new RelayCommand(SignIn,
                           o => !String.IsNullOrEmpty(Login) &&
                                !String.IsNullOrEmpty(Password)));
            }
        }

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

        

        private void SignIn(Object obj)
        {

            var currentUser = DBAdapter.Users.FirstOrDefault(user => user.Login == Login &&
                                                                     user.Password == Password);
            if (currentUser == null)
            {
                MessageBox.Show("Wrong Username or Password");
                return;
            }

            StationManager.CurrentUser = currentUser;
            CabinetWindow cabinetWindow = new CabinetWindow();
            cabinetWindow.ShowDialog();
            OnRequestClose(false);
        }

        private void SignUp(Object obj)
        {
            
            SignupWindow signupWindow = new SignupWindow();
            signupWindow.ShowDialog();
            OnRequestClose(false);
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
