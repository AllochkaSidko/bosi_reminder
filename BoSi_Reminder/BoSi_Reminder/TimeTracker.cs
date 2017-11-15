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
                var reminder = EntityWraper.GetAllRemindsCurrUser(StationManager.CurrentUser).Where(d => DateTime.Now.CompareTo(d.ReactDate)>0&&!d.Status && !d.IsDone).ToList();

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
                ShowMessageBoxAsync(reminder.Text, reminder.ReactDate.ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                //зміна статусу
                reminder.Status = true;
                EntityWraper.Edit(reminder);
   
                LogWriter.LogWrite("Show Riminder:" + reminder.Text);
            }
            catch (Exception e)
            {
                LogWriter.LogWrite("Exception in ShowReminder method", e);
            }
        }

        private delegate void ShowMessageBoxDelegate(string strMessage, string strCaption, MessageBoxButton enmButton, MessageBoxImage enmImage);
        // Method invoked on a separate thread that shows the message box.
        private static void ShowMessageBox(string strMessage, string strCaption, MessageBoxButton enmButton, MessageBoxImage enmImage)
        {
            MessageBox.Show(strMessage, strCaption, enmButton, enmImage);
        }
        // Shows a message box from a separate worker thread.
        public static void ShowMessageBoxAsync(string strMessage, string strCaption, MessageBoxButton enmButton, MessageBoxImage enmImage)
        {
            ShowMessageBoxDelegate caller = new ShowMessageBoxDelegate(ShowMessageBox);
            caller.BeginInvoke(strMessage, strCaption, enmButton, enmImage, null, null);
        }
    }
}
