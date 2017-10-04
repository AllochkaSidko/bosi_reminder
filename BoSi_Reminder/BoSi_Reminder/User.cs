using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoSi_Reminder
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public DateTime PreviousLog { get; set; }
        public static int FreeId = 0;
        public List<Reminder> UsersReminders { get; set; }

        public User(string login, string password, string name, string surname, string email)
        {
            this.Password = Hash(password);
            this.Login = login;
            this.Name = name;
            this.Surname = surname;
            this.Email = email;
            this.Id = ++FreeId;
            this.PreviousLog = DateTime.Now;
            UsersReminders = new List<Reminder>();
           
        }

        public User(){}

        //сортування списку нагадувань за датою
        public List<Reminder> SortRemindList()
        {
            if (UsersReminders == null || UsersReminders.Count == 0)
                return null;
            return StationManager.CurrentUser.UsersReminders?.OrderBy(o => o.ReactDate)?.ToList();
        }

        //метод хешування паролю
        public static string Hash(string password)
        {
            var bytes = new UTF8Encoding().GetBytes(password);
            var hashBytes = System.Security.Cryptography.MD5.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
