using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoSi_Reminder
{

    [Serializable]
    public class Reminder
    {
        public Guid Id { get; set; }
        public DateTime ReactDate { get; set; }
        public bool IsDone { get; set; }
        public string Text { get; set; }
        public bool Status { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Reminder(DateTime reactDate, string text)
        {
           // User = StationManager.CurrentUser;
            UserId = StationManager.CurrentUser.Id;
            Id = Guid.NewGuid();
            this.ReactDate = reactDate;
            this.Text = text;
            this.IsDone = false;
            this.Status = false;
        }

        public Reminder()
        { }

        public class ReminderEntityConfiguration : EntityTypeConfiguration<Reminder>
        {
            public ReminderEntityConfiguration()
            {
                ToTable("Reminder");

                HasKey(s => s.Id);

                Property(p => p.ReactDate)
                    .HasColumnName("ReactDate")
                    .IsRequired();

                Property(p => p.IsDone)
                    .HasColumnName("IsDone")
                    .IsRequired();

                Property(p => p.Text)
                    .HasColumnName("Text")
                    .IsRequired();

                Property(p => p.Status)
                    .HasColumnName("Status")
                    .IsRequired();
            }
        }
    }
}
