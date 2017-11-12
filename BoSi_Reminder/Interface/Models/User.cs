using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Tools;

namespace Interface.Models
{
    [Serializable]
    public class User : ISerializable
    {
        public static string FileName = "user";
        public string Name { get; set; }
        public string Password { get; set; }
        public Guid Id { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public DateTime PreviousLog { get; set; }
        public List<Reminder> Reminders { get; set; }

        public string Filename
        {
            get
            {
                return FileName;
            }
        }

        public User(string login, string password, string name, string surname, string email):this()
        {
            Id = Guid.NewGuid();
            Password = Hash(password);
            Login = login;
            Name = name;
            Surname = surname;
            Email = email;
            PreviousLog = DateTime.Now;    
        }

        //private?
        public User()
        {
            Reminders = new List<Reminder>();
        }

        //метод хешування паролю
        public static string Hash(string password)
        {
            var bytes = new UTF8Encoding().GetBytes(password);
            var hashBytes = System.Security.Cryptography.MD5.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }

        //визначаємо конфігурацію для таблиці нагадувань
        public class UserEntityConfiguration : EntityTypeConfiguration<User>
        {
            public UserEntityConfiguration()
            {
                ToTable("Users");

                HasKey(s => s.Id);

                Property(p => p.Name)
                    .HasColumnName("Name")
                    .IsRequired();

                Property(p => p.Surname)
                    .HasColumnName("Surname")
                    .IsRequired();

                Property(p => p.Email)
                    .HasColumnName("Email")
                    .IsRequired();

                Property(p => p.Password)
                    .HasColumnName("Password")
                    .IsRequired();

                Property(p => p.Login)
                    .HasColumnName("Login")
                    .IsRequired();

                //властивість, що користувач може мати багато нагадувань
                HasMany(s => s.Reminders)
                    .WithRequired(w => w.User)
                    .HasForeignKey(w => w.UserId)
                    .WillCascadeOnDelete(true);
            }
        }
    }
}
