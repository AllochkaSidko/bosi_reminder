using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace BoSi_Reminder
{
    public class RemindTimer
    {
        static DispatcherTimer timer;
        public static void Start()
        {
            var now = DateTime.UtcNow;
            var nextMinute = now.AddTicks(-(now.Ticks % TimeSpan.TicksPerMinute)).AddMinutes(1);
            timer = new DispatcherTimer(DispatcherPriority.Normal);
            timer.Interval = nextMinute - DateTime.UtcNow;
            timer.Tick += Func;
            timer.Start();


        }
        private static void Func(object sender, EventArgs e)
        {
            var correctionNow = DateTime.UtcNow;
            var timeCorrection = correctionNow.AddTicks(-(correctionNow.Ticks % TimeSpan.TicksPerMinute)).AddMinutes(1);
            timer.Interval = timeCorrection - DateTime.UtcNow;

            if (StationManager.CurrentUser != null)
            {
                TimeTracker.TimerReact();
            }

            if (DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0)
            {
                TimeTracker.TodayReminds = StationManager.CurrentUser.Reminders.Where(d => d.ReactDate.Date == DateTime.Today).ToList();
            }
        }

    }
}
