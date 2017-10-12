using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoSi_Reminder
{
    [Serializable]
    public class User : ISerializable
    {
        public static string FileName = "user";
        public string Name { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public DateTime PreviousLog { get; set; }
        public static int FreeId = 0;
        private List<Reminder> _reminders;
        public List<Reminder> Reminders {
            //сортування списку нагадувань за датою 
            get
            {
                _reminders = _reminders?.OrderBy(o => o.ReactDate)?.ToList();
                return _reminders;
            }
            set => _reminders = value; 
        }

        public string Filename
        {
            get
            {
                return FileName;
            }
        }

        public User(string login, string password, string name, string surname, string email)
        {
            this.Password = Hash(password);
            this.Login = login;
            this.Name = name;
            this.Surname = surname;
            this.Email = email;
            this.Id = ++FreeId;
            this.PreviousLog = DateTime.Now;
            Reminders = new List<Reminder>();      
        }

        
        public User(){}

        //метод хешування паролю
        public static string Hash(string password)
        {
            var bytes = new UTF8Encoding().GetBytes(password);
            var hashBytes = System.Security.Cryptography.MD5.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
