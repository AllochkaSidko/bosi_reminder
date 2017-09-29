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

        static DBAdapter()
        {
            Users = new List<User>
            {
                new User("sasha", "password", "Sasha", "Bosa", "shuraka665@gmail.com"),
                new User("alla", "password2", "Alla", "Sidko", "allochka.sidko@gmail.com")
            };



        }

        
    }
}
