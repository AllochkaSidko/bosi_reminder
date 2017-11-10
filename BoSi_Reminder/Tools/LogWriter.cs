using System;
using System.IO;
using System.Threading;

namespace BoSi_Reminder.Tools
{
  public class LogWriter
    {

        //визначаємо шлях і формат назви лог-фалів
        private static readonly string FilePath = Path.Combine(StaticResources.DirPathLog,
            "log" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt");

        private static Mutex MutexObj = new Mutex(true, FilePath.Replace(Path.DirectorySeparatorChar, '_'));

        //перевірка чи існує такий файл, якщо не існує, то його створюють
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
                //якщо отримано ексепшн, то інформацію про нього записують у повідомленя
                //якщо ні, то просто відправляють повідомлення
                
                    AppendLog(logMessage);
                    if (ex != null)
                    {
                        var realException = ex;
                        while (realException != null)
                        {
                            AppendLog(realException.Message);
                            AppendLog(realException.StackTrace);
                            realException = realException.InnerException;
                        }
                    }
                   
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private static void AppendLog(string logMessage)
        {
            MutexObj.WaitOne();
            //створюємо потік для дописування нових логів у файл
            //записуємо лог
            StreamWriter txtWriter = null;
            try
            {
                txtWriter = File.AppendText(FilePath);
                txtWriter.Write("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                txtWriter.Flush();
                txtWriter?.Close();
                txtWriter = null;
            }
            MutexObj.ReleaseMutex();
        }

         
    }
}
