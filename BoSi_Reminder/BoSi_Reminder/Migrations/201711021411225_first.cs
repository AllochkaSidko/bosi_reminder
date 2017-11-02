namespace BoSi_Reminder.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reminder",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ReactDate = c.DateTime(nullable: false),
                        IsDone = c.Boolean(nullable: false),
                        Text = c.String(nullable: false),
                        Status = c.Boolean(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        Login = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        PreviousLog = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reminder", "UserId", "dbo.Users");
            DropIndex("dbo.Reminder", new[] { "UserId" });
            DropTable("dbo.Users");
            DropTable("dbo.Reminder");
        }
    }
}
