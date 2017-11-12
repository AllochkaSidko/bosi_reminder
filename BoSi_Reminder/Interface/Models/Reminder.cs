﻿using System;
using System.Data.Entity.ModelConfiguration;

namespace Interface.Models
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

        public Reminder(DateTime reactDate, string text, User user)
        {
            UserId = user.Id;
            Id = Guid.NewGuid();
            ReactDate = reactDate;
            Text = text;
            IsDone = false;
            Status = false;
        }

        public Reminder()
        { }


        //визначаємо конфігурацію для полів користувача
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
