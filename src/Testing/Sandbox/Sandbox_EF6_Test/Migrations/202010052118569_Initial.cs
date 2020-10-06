namespace Migrations
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
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Entity2",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FK = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Entity1", t => t.FK)
                .Index(t => t.FK);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Entity2", "FK", "dbo.Entity1");
            DropIndex("dbo.Entity2", new[] { "FK" });
            DropTable("dbo.Entity2");
            DropTable("dbo.Entity1");
        }
    }
}
