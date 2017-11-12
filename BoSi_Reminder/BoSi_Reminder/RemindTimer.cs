using System;
using System.Linq;
using System.Windows.Threading;
using DBAdapter;
using Tools;

namespace BoSi_Reminder
{
    public class RemindTimer
    {
        static DispatcherTimer _timer;
        public static void Start()
        {
            try
            {
                var now = DateTime.UtcNow;
                //вирахування часу для спрацювання кожної хвилини, що настала
                var nextMinute = now.AddTicks(-(now.Ticks % TimeSpan.TicksPerMinute)).AddMinutes(1);
                _timer = new DispatcherTimer(DispatcherPriority.Normal);
                _timer.Interval = nextMinute - DateTime.UtcNow;
                //встановлення методу, який має виконуватись кожну хвилину, таймеру
                _timer.Tick += Func;
                //запуск таймера
                _timer.Start();
            }
            catch(Exception ex)
            {
                LogWriter.LogWrite("Exception in RemindTimer class, method Start", ex);
            }
        }

        private static void Func(object sender, EventArgs e)
        {
            var correctionNow = DateTime.UtcNow;
            //вирахування часу для спрацювання кожної хвилини, що настала
            var timeCorrection = correctionNow.AddTicks(-(correctionNow.Ticks % TimeSpan.TicksPerMinute)).AddMinutes(1);
            _timer.Interval = timeCorrection - DateTime.UtcNow;

            try
            {
                if (StationManager.CurrentUser != null)
                {
                    TimeTracker.TimerReact();
                }
                //зміна поточного списку нагадувань на сьогодні з наставанням 00:00 
                if (DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0)
                {
                    TimeTracker.TodayReminds = EntityWraper.GetAllRemindsCurrUser(StationManager.CurrentUser).Where(d => d.ReactDate.Date == DateTime.Today).ToList();  
                }
            }
            catch(Exception ex)
            {
                LogWriter.LogWrite("Exception in RemindTimer class, method Func", ex);
            }
        }

    }
}
