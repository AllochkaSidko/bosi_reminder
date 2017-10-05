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
            //перевірка на валідність пошти
            if (!ValidatorExtensions.IsValidEmailAddress(Email))
            {
                MessageBox.Show("Invalid email!");
                return;
            }

            //перевірка на унікальність імені користувача
            if (DBAdapter.Users.Any(user => user.Login == Login))
            {
                MessageBox.Show("User with this username already exists");
                return;
            }
            //створення нового користувача
            User newuser = new User(Login, Password, Name, Surname, Email);
            DBAdapter.Users.Add(newuser);
            //запис поточного користувача
            StationManager.CurrentUser = newuser;

            SerializeManager.Serialize<User>(StationManager.CurrentUser);

            //перехід на вікно Кабінету
            OnRequestClose(false);
            CabinetWindow cabinetWindow = new CabinetWindow();
            cabinetWindow.ShowDialog();
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
