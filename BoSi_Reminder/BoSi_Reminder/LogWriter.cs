using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BoSi_Reminder
{
  public static class LogWriter
    {
        private static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static readonly string DirPath = Path.Combine(AppData, "BoSi_Reminder");
    
        public static void LogWrite(string logMessage)
        {

            if (!File.Exists(DirPath + "\\" + "log.txt"))
                File.Create(DirPath + "\\" + "log.txt").Dispose();

            try
            {
                using (StreamWriter w = File.AppendText(DirPath + "\\" + "log.txt"))
                    AppendLog(logMessage, w);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private static void AppendLog(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write(StationManager.CurrentUser.Login+ "  --- ");
                txtWriter.Write("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.Write("  :");
                txtWriter.Write("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
                txtWriter.Flush();
                txtWriter.Close();
            }
            catch (Exception ex)
            {
            }
           
        }
    }
}
