namespace Sandbox
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Entity1",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Entity1_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Entity2", t => t.Entity1_Id)
                .Index(t => t.Id)
                .Index(t => t.Entity1_Id);
            
            CreateTable(
                "dbo.Entity2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Entity1", "Entity1_Id", "dbo.Entity2");
            DropIndex("dbo.Entity2", new[] { "Id" });
            DropIndex("dbo.Entity1", new[] { "Entity1_Id" });
            DropIndex("dbo.Entity1", new[] { "Id" });
            DropTable("dbo.Entity2");
            DropTable("dbo.Entity1");
        }
    }
}
