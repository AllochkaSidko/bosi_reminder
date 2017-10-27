using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoSi_Reminder
{
    public class TimeTracker
    {
        //список нагадувань на сьогдні поточного користувача
        public static List<Reminder> TodayReminds = StationManager.CurrentUser.Reminders.Where(d => d.ReactDate.Date == DateTime.Now.Date).ToList();

        public static void TimerReact()
        {
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
                    SerializeManager.Serialize(StationManager.CurrentUser);
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
            foreach (var r in StationManager.CurrentUser.Reminders)
                if (DateTime.Now.CompareTo(r.ReactDate) > 0)
                    ShowReminder(r);
        }
    }
}
