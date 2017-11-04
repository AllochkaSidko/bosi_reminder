using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BoSi_Reminder.Interface.Models;
using BoSi_Reminder.DBAdapter;
using BoSi_Reminder.Tools;

namespace BoSi_Reminder
{
    public class TimeTracker
    {
        //список нагадувань на сьогдні поточного користувача
        public static List<Reminder> TodayReminds; 

        public static void TimerReact()
        {
            try
            {
                TodayReminds = EntityWraper.GetAllRemindsCurrUser(StationManager.CurrentUser).Where(d => d.ReactDate.Date == DateTime.Now.Date).ToList();
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite("Exception in TimerReact constructor while getting reminders", ex);
            }
            //список нагадувань, які мають спрацювати в поточний час
            var reminder = TodayReminds.Where(d => d.ReactDate.Hour == DateTime.Now.Hour && d.ReactDate.Minute == DateTime.Now.Minute).ToList();
            //вивід цих нагадувань
            foreach (var r in reminder)
             ShowReminder(r);
        }

        public static void ShowReminder(Reminder reminder)
        {
            try
            {
                //вивід нагадування, якщо він раніше не був виведений
                if (!reminder.Status&&!reminder.IsDone)
                {
                    MessageBox.Show(reminder.Text, reminder.ReactDate.ToString());
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
