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
        public static List<Reminder> TodayReminds = StationManager.CurrentUser.Reminders.Where(d => d.ReactDate.Date == DateTime.Now.Date).ToList();
        public static void TimerReact()
        {
            var reminder = StationManager.CurrentUser.Reminders.Where(d => d.ReactDate.Hour == DateTime.Now.Hour && d.ReactDate.Minute == DateTime.Now.Minute).ToList();
            foreach (var r in reminder)
             ShowReminder(r);
        }

        public static void ShowReminder(Reminder reminder)
        {
            try
            {
                if (!reminder.Status)
                {
                    MessageBox.Show(reminder.Text, reminder.ReactDate.ToString());
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
            foreach (var r in StationManager.CurrentUser.Reminders)
                if (DateTime.Now.CompareTo(r.ReactDate) > 0)
                    ShowReminder(r);
        }
    }
}
