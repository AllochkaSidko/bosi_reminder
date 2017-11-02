using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoSi_Reminder
{
    public class SignupViewModel : INotifyPropertyChanged
    {
        private User _userCandidate;
        private RelayCommand _signUpCommand;
        private RelayCommand _closeCommand;
      

        public SignupViewModel(User userCandidate)
        {
            this._userCandidate = userCandidate;
        }

        public RelayCommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(obj => OnRequestClose(true))); }
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
        private void SignUp(Object obj)
        {
            User newuser = null;

            try
            {
                if(!new EmailAddressAttribute().IsValid(Email))
                {
                    MessageBox.Show("Invalid email");
                    return;
                }

                if(EntityWraper.UserExist(Login))
                {
                    MessageBox.Show("User with such login aleady exists");
                    return;
                }
            }
            catch(Exception ex)
            {
                LogWriter.LogWrite("Exception while checking email and login in SignUp method", ex);
                return;
            }

            try
            {
                //створення нового користувача
                newuser = new User(Login, Password, Name, Surname, Email);
                EntityWraper.AddUser(newuser);
            }
            catch(Exception ex)
            {
                LogWriter.LogWrite("Exception while creating new user", ex);
            }
            StationManager.CurrentUser = newuser;

            //серіалізуємо поточного користувача
            SerializeManager.Serialize<User>(StationManager.CurrentUser);

            LogWriter.LogWrite("Sign up request");

            //перехід на вікно Кабінету
            OnRequestClose(false);
            CabinetWindow cabinetWindow = new CabinetWindow();
            cabinetWindow.ShowDialog();
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
    }
}
