using Interface.Models;
using System.ServiceModel;
using System.Collections.Generic;
using System;

namespace Interface
{
    [ServiceContract]
        public interface IBoSi_ReminderService
        {
            [OperationContract]
            bool UserExist(string login);
            [OperationContract]
            User GetUserByLogin(string login);
            [OperationContract]
            void AddUser(User user);
            [OperationContract]
            void AddReminder(Reminder reminder);
            [OperationContract]
            List<Reminder> GetAllRemindsCurrUser(User user);
            [OperationContract]
            Reminder GetReminder(Guid id);
            [OperationContract]
            User GetUser(Guid id);
            [OperationContract]
            void Delete(Reminder item);
            [OperationContract]
            Reminder Edit(Reminder item);
            [OperationContract]
            User EditUser(User item);
        }
}
