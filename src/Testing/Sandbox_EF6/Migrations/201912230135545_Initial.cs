namespace Sandbox
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BaseClasses",
                c => new
                    {
                        Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Details",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        StringMax = c.String(),
                        Bob = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaseClasses", t => t.Id)
                .ForeignKey("dbo.Masters", t => t.Bob)
                .Index(t => t.Id)
                .Index(t => t.Bob);
            
            CreateTable(
                "dbo.Masters",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        StringMax = c.String(),
                        String100 = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaseClasses", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.StringMax);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Masters", "Id", "dbo.BaseClasses");
            DropForeignKey("dbo.Details", "Bob", "dbo.Masters");
            DropForeignKey("dbo.Details", "Id", "dbo.BaseClasses");
            DropIndex("dbo.Masters", new[] { "StringMax" });
            DropIndex("dbo.Masters", new[] { "Id" });
            DropIndex("dbo.Details", new[] { "Bob" });
            DropIndex("dbo.Details", new[] { "Id" });
            DropTable("dbo.Masters");
            DropTable("dbo.Details");
            DropTable("dbo.BaseClasses");
        }
    }
}
