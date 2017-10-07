using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoSi_Reminder
{
  public static class LogWriter
    {
        private static readonly string FilePath = Path.Combine(StaticResources.DirPathLog,
            "log" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt");

        private static Mutex MutexObj = new Mutex(true, FilePath.Replace(Path.DirectorySeparatorChar, '_'));

        private static void CheckingCreateFile()
        {
            if (!Directory.Exists(StaticResources.DirPathLog))
                Directory.CreateDirectory(StaticResources.DirPathLog);
            
            if (!File.Exists(FilePath))
                File.Create(FilePath).Close();
        }

        public static void LogWrite(string logMessage, Exception ex = null)
        {
            CheckingCreateFile();

            try
            {
                using (StreamWriter w = File.AppendText(FilePath))
                {
                    if (ex != null)
                    {
                        AppendLog(logMessage, w);
                        var realException = ex;
                        while (realException != null)
                        {
                            AppendLog(realException.Message, w);
                            AppendLog(realException.StackTrace, w);
                            realException = realException.InnerException;
                        }
                    }
                    else
                    {
                        AppendLog(logMessage, w);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private static void AppendLog(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write(StationManager.CurrentUser.Login+ "  --- ");
                txtWriter.Write("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
                txtWriter.Flush();
                txtWriter.Close();
                txtWriter=null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            MutexObj.ReleaseMutex();
        }

         
    }
}
