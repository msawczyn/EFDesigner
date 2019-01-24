namespace ClassLibrary_Framework_EF6
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
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
                        Sequence = c.Long(
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "DatabaseGeneratedOption",
                                    new AnnotationValues(oldValue: null, newValue: "Identity")
                                },
                            }),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Entity1", new[] { "Id" });
            DropTable("dbo.Entity1",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "Sequence",
                        new Dictionary<string, object>
                        {
                            { "DatabaseGeneratedOption", "Identity" },
                        }
                    },
                });
        }
    }
}
