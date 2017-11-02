namespace BoSi_Reminder.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BoSi_Reminder.ReminderContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
                AutomaticMigrationsEnabled = false;
                ContextKey = "ReminderContext";
        }

protected override void Seed(BoSi_Reminder.ReminderContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
