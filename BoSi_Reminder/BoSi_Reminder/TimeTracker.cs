using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DBAdapter;
using Interface.Models;
using Tools;

namespace BoSi_Reminder
{
    public class TimeTracker
    {
       
        public static void TimerReact()
        {
            try
            {
                //список нагадувань, які мають спрацювати в поточний час
                var reminder = EntityWraper.GetAllRemindsCurrUser(StationManager.CurrentUser).Where(d => DateTime.Now.CompareTo(d.ReactDate)>0).ToList();

                //вивід цих нагадувань
                foreach (var r in reminder)
                    ShowReminder(r);
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite("Exception in TimerReact constructor while getting reminders", ex);
            }
          
        }

        public static void ShowReminder(Reminder reminder)
        {
            try
            {
                //вивід нагадування, якщо він раніше не був виведений
                if (!reminder.Status&&!reminder.IsDone)
                {
                    MessageBox.Show(reminder.Text, reminder.ReactDate.ToString(), MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
                    //зміна статусу
                    reminder.Status = true;
                    EntityWraper.Edit(reminder);
                }
                LogWriter.LogWrite("Show Riminder:" + reminder.Text);

            }
            catch (Exception e)
            {
                LogWriter.LogWrite("Exception in ShowReminder method", e);
            }
        }

        public static void ShowPrevious()
        {
            //виведення нагадування, час спрацювання яких вже минув
            try
            {
                foreach (var r in EntityWraper.GetAllRemindsCurrUser(StationManager.CurrentUser))
                    if (DateTime.Now.CompareTo(r.ReactDate) > 0)
                        ShowReminder(r);
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite("Exception in ShowPrevious method while getting reminders", ex);
            }
        }
    }
}
