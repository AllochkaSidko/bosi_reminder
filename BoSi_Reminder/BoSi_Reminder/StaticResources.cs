using System;
using System.IO;


namespace BoSi_Reminder
{
   
    class StaticResources
    {
        //шляхи до папок, де зберігаються логи та серіалізація
        private static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        internal static readonly string DirPath = Path.Combine(AppData, "BoSi_Reminder");
        internal static readonly string DirPathLog = Path.Combine(DirPath, "Logs");
    }
}
