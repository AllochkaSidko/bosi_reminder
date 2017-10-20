using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoSi_Reminder
{
    public static class DBAdapter
    {
        public static List<User> Users { get; set; }


        //заповнення початковими даними
        static DBAdapter()
        {
            Users = new List<User>
            {
                new User("sasha", "password", "Sasha", "Bosa", "shuraka665@gmail.com"),
                new User("alla", "password2", "Alla", "Sidko", "allochka.sidko@gmail.com")
            };
            Users[0].Reminders.Add(new Reminder(new DateTime(2017, 10, 20, 20, 9, 0), "Do c#"));
            Users[1].Reminders.Add(new Reminder(new DateTime(2017, 09, 17,20,23,0), "Do hometask"));

        }

        
    }
}
