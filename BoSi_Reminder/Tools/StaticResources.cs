using System;
using System.IO;


namespace BoSi_Reminder.Tools
{
   
    public class StaticResources
    {
        //шляхи до папок, де зберігаються логи та серіалізація
        private static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static readonly string DirPath = Path.Combine(AppData, "BoSi_Reminder");
        public static readonly string DirPathLog = Path.Combine(DirPath, "Logs");
    }
}
