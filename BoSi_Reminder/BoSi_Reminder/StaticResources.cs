using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoSi_Reminder
{
    class StaticResources
    {
        private static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        internal static readonly string DirPath = Path.Combine(AppData, "BoSi_Reminder");
        internal static readonly string DirPathLog = Path.Combine(DirPath, "Logs");
    }
}
