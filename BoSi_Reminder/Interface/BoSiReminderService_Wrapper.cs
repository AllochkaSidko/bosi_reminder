using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Interface.Models;

namespace Interface
{
    public class BoSiReminderService_Wrapper
    {
        //перевірка чи існує в бд користувач з таким логіном
        public static bool UserExist(string login)
        {
            using (var myChannelFactory = new ChannelFactory<IBoSi_ReminderService>("Server"))
            {
                IBoSi_ReminderService client = myChannelFactory.CreateChannel();
                return client.UserExist(login);
            }
        }

        //витягаємо до користувача через логін
        public static User GetUserByLogin(string login)
        {
            using (var myChannelFactory = new ChannelFactory<IBoSi_ReminderService>("Server"))
            {
                IBoSi_ReminderService client = myChannelFactory.CreateChannel();
                return client.GetUserByLogin(login);
            }
        }

        //додаємо нового користувача
        public static void AddUser(User user)
        {
            using (var myChannelFactory = new ChannelFactory<IBoSi_ReminderService>("Server"))
            {
                IBoSi_ReminderService client = myChannelFactory.CreateChannel();
                client.AddUser(user);
            }
        }

        //додаємо нове нагадування
        public static void AddReminder(Reminder reminder)
        {
            using (var myChannelFactory = new ChannelFactory<IBoSi_ReminderService>("Server"))
            {
                IBoSi_ReminderService client = myChannelFactory.CreateChannel();
                client.AddReminder(reminder);
            }
        }

        //дістаємо всі нагадування поточного користвача
        public static List<Reminder> GetAllRemindsCurrUser(User user)
        {
            using (var myChannelFactory = new ChannelFactory<IBoSi_ReminderService>("Server"))
            {
                IBoSi_ReminderService client = myChannelFactory.CreateChannel();
                return client.GetAllRemindsCurrUser(user);
            }
        }

        //дістаємо нагадування за ід
        public static Reminder GetReminder(Guid id)
        {
            using (var myChannelFactory = new ChannelFactory<IBoSi_ReminderService>("Server"))
            {
                IBoSi_ReminderService client = myChannelFactory.CreateChannel();
                return client.GetReminder(id);
            }
        }

        //дістаємо користувача за ід
        public static User GetUser(Guid id)
        {
            using (var myChannelFactory = new ChannelFactory<IBoSi_ReminderService>("Server"))
            {
                IBoSi_ReminderService client = myChannelFactory.CreateChannel();
                return client.GetUser(id);
            }
        }

        //видаляємо нагадування
        public static void Delete(Reminder item)
        {
            using (var myChannelFactory = new ChannelFactory<IBoSi_ReminderService>("Server"))
            {
                IBoSi_ReminderService client = myChannelFactory.CreateChannel();
                client.Delete(item);
            }
        }

        //зберігаємо змінити нагадування
        public static Reminder Edit(Reminder item)
        {
            using (var myChannelFactory = new ChannelFactory<IBoSi_ReminderService>("Server"))
            {
                IBoSi_ReminderService client = myChannelFactory.CreateChannel();
                client.Edit(item);
            }
            return item;
        }

        //зберігаємо зміни корстувача 
        public static User EditUser(User item)
        {
            using (var myChannelFactory = new ChannelFactory<IBoSi_ReminderService>("Server"))
            {
                IBoSi_ReminderService client = myChannelFactory.CreateChannel();
                client.EditUser(item);
            }
            return item;
        }
    }
}
