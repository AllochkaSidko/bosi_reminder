﻿using System.Data.Entity;
using BoSi_Reminder.Interface.Models;
using BoSi_Reminder.DBAdapter.Migrations;


namespace BoSi_Reminder.DBAdapter
{
    public class ReminderContext : DbContext
    {
        public ReminderContext() : base("UI")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ReminderContext, Configuration>("UI"));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Reminder> Reminders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new User.UserEntityConfiguration());
            modelBuilder.Configurations.Add(new Reminder.ReminderEntityConfiguration());
        }
    }
}