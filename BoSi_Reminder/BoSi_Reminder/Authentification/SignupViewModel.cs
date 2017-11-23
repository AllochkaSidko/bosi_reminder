using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DBAdapter;
using Interface;
using Interface.Models;
using Tools;

namespace BoSi_Reminder.Authentification
{
    public class SignupViewModel : INotifyPropertyChanged
    {
        private User _userCandidate;
        private RelayCommand _signUpCommand;
        private RelayCommand _closeCommand;
        public RelayCommand _closeSignUpCommand;
      

        public SignupViewModel(User userCandidate)
        {
            this._userCandidate = userCandidate;
        }

        public RelayCommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(obj => OnRequestClose(true))); }
        }

        public RelayCommand CloseSignUpCommand
        {
            get { return _closeSignUpCommand ?? (_closeSignUpCommand = new RelayCommand(obj=> CloseWin(obj))); }
        }

        public void CloseWin(Object obj)
        {
            OnRequestClose(false);
            
        }

        //первірка чи всі поля заповнені
        public RelayCommand SignUpCommand
        {
            get
            {
                if (_signUpCommand == null)
                {
                    _signUpCommand = new RelayCommand(SignUp, o => !String.IsNullOrEmpty(Login) &&
                                                                   !String.IsNullOrEmpty(Password) &&
                                                                   !String.IsNullOrEmpty(Name) &&
                                                                   !String.IsNullOrEmpty(Surname) &&
                                                                   !String.IsNullOrEmpty(Email));
                }
                return _signUpCommand;
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

        public String Name
        {
            get => _userCandidate.Name;
            set
            {
                _userCandidate.Name = value;
                
            }
        }

        public String Surname
        {
            get => _userCandidate.Surname;
            set
            {
                _userCandidate.Surname = value;
               
            }
        }

        public String Email
        {
            get => _userCandidate.Email;
            set
            {
                _userCandidate.Email = value;
                OnPropertyChanged();
            }
        }

        //метод створення нового акаунту
        private async void SignUp(Object obj)
        {
            OnRequestLoader(true);
            //виконання методу в потоці
            var result = await Task.Run(() =>
            {
                User newuser = null;

            try
            {
                if(!new EmailAddressAttribute().IsValid(Email))
                {
                    MessageBox.Show("Invalid email");
                    return false;
                }

                if(BoSiReminderService_Wrapper.UserExist(Login))
                {
                    MessageBox.Show("User with such login aleady exists");
                    return false;
                }
            }
            catch(Exception ex)
            {
                LogWriter.LogWrite("Exception while checking email and login in SignUp method", ex);
                return false;
            }

            try
            {
                //створення нового користувача
                newuser = new User(Login, Password, Name, Surname, Email);
                BoSiReminderService_Wrapper.AddUser(newuser);
            }
            catch(Exception ex)
            {
                LogWriter.LogWrite("Exception while creating new user", ex);
            }
            StationManager.CurrentUser = newuser;

            //серіалізуємо поточного користувача
            SerializeManager.Serialize<User>(StationManager.CurrentUser);

            return true;
            });
            OnRequestLoader(false);

            if (result)
            {
                LogWriter.LogWrite("Sign up request \n" + StationManager.CurrentUser.Login + " entered to the system.");
                OnRequestClose(false);
            }
            return;
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

    
    }
}
