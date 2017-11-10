using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BoSi_Reminder.Interface.Models;


namespace BoSi_Reminder.DBAdapter
{
    public static class EntityWraper
    {
        //перевірка чи існує в бд користувач з таким логіном
        public static bool UserExist(string login)
        {
            using (var context = new ReminderContext())
            {
                return context.Users.Any(u => u.Login == login);
            }
        }

        //витягаємо до користувача через логін
        public static User GetUserByLogin(string login)
        {
            using (var context = new ReminderContext())
            {
                return context.Users.FirstOrDefault(u => u.Login == login);
            }
        }

        //додаємо нового користувача
        public static void AddUser(User user)
        {
            using (var context = new ReminderContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        //додаємо нове нагадування
        public static void AddReminder(Reminder reminder)
        {
            using (var context = new ReminderContext())
            {
                context.Reminders.Add(reminder);
                context.SaveChanges();
            }
        }

        //дістаємо всі нагадування поточного користвача
        public static List<Reminder> GetAllRemindsCurrUser(User user)
        {
            using (var context = new ReminderContext())
            {
                return context.Reminders.Where(r=>r.UserId == user.Id).ToList();    
            }
        }

        //дістаємо нагадування за ід
        public static Reminder GetReminder(Guid id)
        {
            using (var context = new ReminderContext())
            {
               return context.Reminders.FirstOrDefault(item => item.Id == id);
            }
        }

        //дістаємо користувача за ід
        public static User GetUser(Guid id)
        {
            using (var context = new ReminderContext())
            {
                return context.Users.FirstOrDefault(item => item.Id == id);
            }
        }

        //видаляємо нагадування
        public static bool Delete(Reminder item)
        {
            using (var context = new ReminderContext())
            {
                context.Entry(item).State = EntityState.Deleted;
                context.SaveChanges();
            }
            return true;
        }

        //зберігаємо змінити нагадування
        public static Reminder Edit(Reminder item)
        {
            using (var context = new ReminderContext())
            {
                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
            }
            return item;
        }

        //зберігаємо зміни корстувача 
        public static User EditUser(User item)
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
