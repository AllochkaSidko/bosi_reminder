using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Threading.Tasks;
using Interface.Models;
using Interface;
using Tools;

namespace BoSi_Reminder.Authentification
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
        private async void SignIn(Object obj)
        {
            OnRequestLoader(true);
            //виконання методу в потоці
            var result = await Task.Run(() =>
            {
                User currentUser = null;
                //перевірка чи є існує такий користувач
                try
                {
                    currentUser = BoSiReminderService_Wrapper.GetUserByLogin(Login);
                }
                catch (Exception ex)
                {
                    LogWriter.LogWrite("Exception while trying to get user with such login as " + Login, ex);
                    return false;
                }

                if (currentUser == null)
                {
                    MessageBox.Show("Wrong Login");
                    return false;
                }

                if (currentUser.Password != User.Hash(Password))
                {
                    MessageBox.Show("Wrong password!");
                    return false;
                }

                try
                {
                    currentUser.PreviousLog = DateTime.Now;
                    BoSiReminderService_Wrapper.EditUser(currentUser);
                }
                catch (Exception ex)
                {
                    LogWriter.LogWrite("Exception while trying to edit user", ex);
                    return false;
                }
                //записуємопоточного користувача
                StationManager.CurrentUser = currentUser;
                //серіалізуємо поточного користувача
                SerializeManager.Serialize<User>(StationManager.CurrentUser);
                return true;
            });

            OnRequestLoader(false);

            if (result)
            {
                //записуємо в лог дії користувача
                LogWriter.LogWrite(StationManager.CurrentUser.Login + " entered to the system.");
                OnRequestClose(false); 
            }
        }

        //вікриваємо вікно Sign Up
        private void SignUp(Object obj)
        {
            OnRequestVisibilityChange(false);
            SignupWindow signupWindow = new SignupWindow();
            signupWindow.ShowDialog();
            OnRequestVisibilityChange(true);
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
        internal event LoaderHandler RequestLoader;
        internal delegate void LoaderHandler(bool isShow);

        internal virtual void OnRequestLoader(bool isShow)
        {
            RequestLoader?.Invoke(isShow);
        }

        internal event VisibilityHandler RequestVisibilityChange;
        internal delegate void VisibilityHandler(bool visibility);

        internal virtual void OnRequestVisibilityChange(bool visibility)
        {
            RequestVisibilityChange?.Invoke(visibility);
        }
    }
}
