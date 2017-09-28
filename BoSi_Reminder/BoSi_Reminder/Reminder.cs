using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoSi_Reminder
{
   public class Reminder
    {
        public int Id { get; set; }
        public static int FreeId { get; set; }
        public DateTime ReactDate { get; set; }
        public bool IsDone { get; set; }
        public string Text { get; set; }

        public Reminder(DateTime reactDate, string text)
        {
            this.ReactDate = reactDate;
            this.Text = text;
            this.Id = ++FreeId;
            this.IsDone = false;
        }

    }
}
