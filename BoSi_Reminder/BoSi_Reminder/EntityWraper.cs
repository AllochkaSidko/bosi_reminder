using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoSi_Reminder
{
    public static class EntityWraper
    {
        public static bool UserExist(string login)
        {
            using (var context = new ReminderContext())
            {
                return context.Users.Any(u => u.Login == login);
            }
        }

        public static User GetUserByLogin(string login)
        {
            using (var context = new ReminderContext())
            {
                return context.Users.FirstOrDefault(u => u.Login == login);
            }
        }

        public static void AddUser(User user)
        {
            using (var context = new ReminderContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public static void AddReminder(Reminder reminder)
        {
            using (var context = new ReminderContext())
            {
                context.Reminders.Add(reminder);
                context.SaveChanges();
            }
        }

        public static List<Reminder> GetAll()
        {
            using (var context = new ReminderContext())
            {
                return context.Reminders.AsNoTracking().ToList();    
            }
        }

        public static Reminder Get(Guid id)
        {
            using (var context = new ReminderContext())
            {
               return context.Reminders.AsNoTracking().SingleOrDefault(item => (Guid)item.GetType().GetProperty("Id").GetValue(item) == id);
            }
        }
        

        public static bool Delete(Reminder item)
        {
            using (var context = new ReminderContext())
            {
                context.Reminders.Remove(item);
                context.SaveChanges();
                context.Entry(item).State = EntityState.Detached;
            }
            return true;
        }

        public static Reminder Edit(Reminder item)
        {
            using (var context = new ReminderContext())
            {
                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
            }
            return item;
        }
    }
}
