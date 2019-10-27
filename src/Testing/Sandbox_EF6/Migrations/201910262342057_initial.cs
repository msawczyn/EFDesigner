namespace Sandbox_EF6
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BaseClasses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DetailBaseClasses_Id = c.Long(name: "Detail.BaseClasses_Id", nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Details", t => t.DetailBaseClasses_Id)
                .Index(t => t.DetailBaseClasses_Id);
            
            CreateTable(
                "dbo.Details",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        MasterDetails_Id = c.Long(name: "Master.Details_Id"),
                        StringMax = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaseClasses", t => t.Id)
                .ForeignKey("dbo.Masters", t => t.MasterDetails_Id)
                .Index(t => t.Id)
                .Index(t => t.MasterDetails_Id);
            
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
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Masters", "Id", "dbo.BaseClasses");
            DropForeignKey("dbo.Details", "Master.Details_Id", "dbo.Masters");
            DropForeignKey("dbo.Details", "Id", "dbo.BaseClasses");
            DropForeignKey("dbo.BaseClasses", "Detail.BaseClasses_Id", "dbo.Details");
            DropIndex("dbo.Masters", new[] { "Id" });
            DropIndex("dbo.Details", new[] { "Master.Details_Id" });
            DropIndex("dbo.Details", new[] { "Id" });
            DropIndex("dbo.BaseClasses", new[] { "Detail.BaseClasses_Id" });
            DropTable("dbo.Masters");
            DropTable("dbo.Details");
            DropTable("dbo.BaseClasses");
        }
    }
}
