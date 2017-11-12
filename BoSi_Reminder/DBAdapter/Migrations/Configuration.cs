
using System.Data.Entity.Migrations;

namespace DBAdapter.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ReminderContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationsEnabled = false;
            ContextKey = "ReminderContext";
        }

    protected override void Seed(ReminderContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
