using System;
using System.ComponentModel;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Interface.Models
{
    [DataContract]
    public class Reminder : INotifyPropertyChanged
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public DateTime ReactDate { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public bool Status { get; set; }
        [DataMember]
        public Guid UserId { get; set; }
        [DataMember]
        public User User { get; set; }
        
        private bool _isDone;
        [DataMember]
        public bool IsDone
        {
            get { return _isDone; }
            set
            {
                if (_isDone != value)
                {
                    _isDone = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
