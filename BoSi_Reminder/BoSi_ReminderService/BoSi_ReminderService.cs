using Interface;
using System.Collections.Generic;
using DBAdapter;
using Interface.Models;
using System;

namespace BoSi_ReminderService
{
    class BoSi_ReminderService : IBoSi_ReminderService
        {

        public bool UserExist(string login)
        {
            return EntityWraper.UserExist(login);
        }

        //витягаємо до користувача через логін
        public User GetUserByLogin(string login)
        {
            return EntityWraper.GetUserByLogin(login);
        }

        //додаємо нового користувача
        public void AddUser(User user)
        {
            EntityWraper.AddUser(user);
        }

        //додаємо нове нагадування
        public void AddReminder(Reminder reminder)
        {
            EntityWraper.AddReminder(reminder);
        }

        //дістаємо всі нагадування поточного користвача
        public  List<Reminder> GetAllRemindsCurrUser(User user)
        {
            return EntityWraper.GetAllRemindsCurrUser(user);
        }

        //дістаємо нагадування за ід
        public  Reminder GetReminder(Guid id)
        {
            return EntityWraper.GetReminder(id);
        }

        //дістаємо користувача за ід
        public User GetUser(Guid id)
        {
            return EntityWraper.GetUser(id);
        }

        //видаляємо нагадування
        public  void Delete(Reminder item)
        {
            EntityWraper.Delete(item);
            //return true;
        }

        //зберігаємо змінити нагадування
        public  Reminder Edit(Reminder item)
        {
            return EntityWraper.Edit(item);
        }

        //зберігаємо зміни корстувача 
        public User EditUser(User item)
        {
            return EntityWraper.EditUser(item);
        }
    }
}

