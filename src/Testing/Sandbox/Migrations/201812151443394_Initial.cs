namespace Sandbox
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Entity3",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Property1 = c.Int(),
                        Property2 = c.String(maxLength: 2, unicode: false),
                        Property3 = c.String(maxLength: 9),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Entity3", new[] { "Id" });
            DropTable("dbo.Entity3");
        }
    }
}
