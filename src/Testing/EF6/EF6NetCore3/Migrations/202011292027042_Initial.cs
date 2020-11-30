namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BaseClassWithRequiredProperties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Property0 = c.String(nullable: false),
                        Property1 = c.String(),
                        PropertyInChild = c.String(),
                        Property11 = c.String(),
                        Property12 = c.String(),
                        PropertyInChild1 = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AllPropertyTypesOptionals",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        BinaryAttr = c.Binary(),
                        BooleanAttr = c.Boolean(),
                        ByteAttr = c.Byte(),
                        DateTimeAttr = c.DateTime(),
                        DateTimeOffsetAttr = c.DateTimeOffset(precision: 7),
                        DecimalAttr = c.Decimal(precision: 18, scale: 2),
                        DoubleAttr = c.Double(),
                        GuidAttr = c.Guid(),
                        Int16Attr = c.Short(),
                        Int32Attr = c.Int(),
                        Int64Attr = c.Long(),
                        SingleAttr = c.Single(),
                        TimeAttr = c.Time(precision: 7),
                        StringAttr = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AllPropertyTypesRequireds",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        BinaryAttr = c.Binary(nullable: false),
                        BooleanAttr = c.Boolean(nullable: false),
                        ByteAttr = c.Byte(nullable: false),
                        DateTimeAttr = c.DateTime(nullable: false),
                        DateTimeOffsetAttr = c.DateTimeOffset(nullable: false, precision: 7),
                        DecimalAttr = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DoubleAttr = c.Double(nullable: false),
                        GuidAttr = c.Guid(nullable: false),
                        Int16Attr = c.Short(nullable: false),
                        Int32Attr = c.Int(nullable: false),
                        Int64Attr = c.Long(nullable: false),
                        SingleAttr = c.Single(nullable: false),
                        TimeAttr = c.Time(nullable: false, precision: 7),
                        StringAttr = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BChilds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BParentOptional_1Id = c.Int(),
                        BParentRequired_2Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BParentOptionals", t => t.BParentOptional_1Id)
                .ForeignKey("dbo.BParentRequireds", t => t.Id)
                .ForeignKey("dbo.BParentRequireds", t => t.BParentRequired_2Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.BParentOptional_1Id)
                .Index(t => t.BParentRequired_2Id);
            
            CreateTable(
                "dbo.BParentCollections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BChildRequiredId = c.Int(nullable: false),
                        BChildOptionalId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BChilds", t => t.BChildRequiredId, cascadeDelete: true)
                .ForeignKey("dbo.BChilds", t => t.BChildOptionalId)
                .Index(t => t.BChildRequiredId)
                .Index(t => t.BChildOptionalId);
            
            CreateTable(
                "dbo.BParentOptionals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BChildOptional_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BChilds", t => t.Id)
                .ForeignKey("dbo.BChilds", t => t.BChildOptional_Id)
                .Index(t => t.Id)
                .Index(t => t.BChildOptional_Id);
            
            CreateTable(
                "dbo.BParentRequireds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BChilds", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Children",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(nullable: false),
                        Master_Children_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Children", t => t.ParentId)
                .ForeignKey("dbo.Masters", t => t.Master_Children_Id, cascadeDelete: true)
                .Index(t => t.ParentId)
                .Index(t => t.Master_Children_Id);
            
            CreateTable(
                "dbo.HiddenEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PropertyInChild = c.String(),
                        UChildOptional_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UChilds", t => t.UChildOptional_Id)
                .ForeignKey("dbo.UChilds", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.UChildOptional_Id);
            
            CreateTable(
                "dbo.UChilds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UParentOptional_UChildCollection_Id = c.Int(),
                        UParentRequired_UChildCollection_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HiddenEntities", t => t.UParentOptional_UChildCollection_Id)
                .ForeignKey("dbo.UParentRequireds", t => t.UParentRequired_UChildCollection_Id, cascadeDelete: true)
                .ForeignKey("dbo.UParentRequireds", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.UParentOptional_UChildCollection_Id)
                .Index(t => t.UParentRequired_UChildCollection_Id);
            
            CreateTable(
                "dbo.Masters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParserTests",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        foo = c.Long(nullable: false),
                        name1 = c.String(),
                        name2 = c.String(),
                        name3 = c.Int(),
                        name4 = c.Int(),
                        name5 = c.Int(),
                        name6 = c.Int(),
                        name7 = c.String(),
                        name8 = c.String(),
                        name9 = c.String(),
                        name = c.String(),
                        name11 = c.Int(),
                        name12 = c.Int(),
                        name13 = c.Int(),
                        name14 = c.Int(),
                        name15 = c.String(),
                        name16 = c.String(),
                        name17 = c.String(),
                        name18 = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RenamedColumns",
                c => new
                    {
                        Foo = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Foo);
            
            CreateTable(
                "dbo.SpatialProperties",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AttrGeography = c.Geography(nullable: false),
                        AttrGeographyCollection = c.Geography(nullable: false),
                        AttrGeographyLineString = c.Geography(nullable: false),
                        AttrGeographyMultiLineString = c.Geography(nullable: false),
                        AttrGeographyMultiPoint = c.Geography(nullable: false),
                        AttrGeographyMultiPolygon = c.Geography(nullable: false),
                        AttrGeographyPoint = c.Geography(nullable: false),
                        AttrGeographyPolygon = c.Geography(nullable: false),
                        AttrGeometry = c.Geometry(nullable: false),
                        AttrGeometryCollection = c.Geometry(nullable: false),
                        AttrGeometryLineString = c.Geometry(nullable: false),
                        AttrGeometryMultiLineString = c.Geometry(nullable: false),
                        AttrGeometryMultiPoint = c.Geometry(nullable: false),
                        AttrGeometryMultiPolygon = c.Geometry(nullable: false),
                        AttrGeometryPoint = c.Geometry(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UParentCollections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UChildOptionalId = c.Int(),
                        UChildRequiredId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UChilds", t => t.UChildOptionalId)
                .ForeignKey("dbo.UChilds", t => t.UChildRequiredId, cascadeDelete: true)
                .Index(t => t.UChildOptionalId)
                .Index(t => t.UChildRequiredId);
            
            CreateTable(
                "dbo.UParentRequireds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UChilds", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.BParentCollection_1_x_BChildCollection",
                c => new
                    {
                        BChild_Id = c.Int(nullable: false),
                        BParentCollection_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BChild_Id, t.BParentCollection_Id })
                .ForeignKey("dbo.BChilds", t => t.BChild_Id, cascadeDelete: true)
                .ForeignKey("dbo.BParentCollections", t => t.BParentCollection_Id, cascadeDelete: true)
                .Index(t => t.BChild_Id)
                .Index(t => t.BParentCollection_Id);
            
            CreateTable(
                "dbo.UParentCollection_x_UChildCollection",
                c => new
                    {
                        UParentCollection_Id = c.Int(nullable: false),
                        UChild_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UParentCollection_Id, t.UChild_Id })
                .ForeignKey("dbo.UParentCollections", t => t.UParentCollection_Id, cascadeDelete: true)
                .ForeignKey("dbo.UChilds", t => t.UChild_Id, cascadeDelete: true)
                .Index(t => t.UParentCollection_Id)
                .Index(t => t.UChild_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UParentRequireds", "Id", "dbo.UChilds");
            DropForeignKey("dbo.UChilds", "Id", "dbo.UParentRequireds");
            DropForeignKey("dbo.UChilds", "UParentRequired_UChildCollection_Id", "dbo.UParentRequireds");
            DropForeignKey("dbo.UParentCollections", "UChildRequiredId", "dbo.UChilds");
            DropForeignKey("dbo.UParentCollections", "UChildOptionalId", "dbo.UChilds");
            DropForeignKey("dbo.UParentCollection_x_UChildCollection", "UChild_Id", "dbo.UChilds");
            DropForeignKey("dbo.UParentCollection_x_UChildCollection", "UParentCollection_Id", "dbo.UParentCollections");
            DropForeignKey("dbo.Children", "Master_Children_Id", "dbo.Masters");
            DropForeignKey("dbo.HiddenEntities", "Id", "dbo.UChilds");
            DropForeignKey("dbo.HiddenEntities", "UChildOptional_Id", "dbo.UChilds");
            DropForeignKey("dbo.UChilds", "UParentOptional_UChildCollection_Id", "dbo.HiddenEntities");
            DropForeignKey("dbo.Children", "ParentId", "dbo.Children");
            DropForeignKey("dbo.BChilds", "BParentRequired_2Id", "dbo.BParentRequireds");
            DropForeignKey("dbo.BParentRequireds", "Id", "dbo.BChilds");
            DropForeignKey("dbo.BChilds", "Id", "dbo.BParentRequireds");
            DropForeignKey("dbo.BParentOptionals", "BChildOptional_Id", "dbo.BChilds");
            DropForeignKey("dbo.BChilds", "BParentOptional_1Id", "dbo.BParentOptionals");
            DropForeignKey("dbo.BParentOptionals", "Id", "dbo.BChilds");
            DropForeignKey("dbo.BParentCollections", "BChildOptionalId", "dbo.BChilds");
            DropForeignKey("dbo.BParentCollection_1_x_BChildCollection", "BParentCollection_Id", "dbo.BParentCollections");
            DropForeignKey("dbo.BParentCollection_1_x_BChildCollection", "BChild_Id", "dbo.BChilds");
            DropForeignKey("dbo.BParentCollections", "BChildRequiredId", "dbo.BChilds");
            DropIndex("dbo.UParentCollection_x_UChildCollection", new[] { "UChild_Id" });
            DropIndex("dbo.UParentCollection_x_UChildCollection", new[] { "UParentCollection_Id" });
            DropIndex("dbo.BParentCollection_1_x_BChildCollection", new[] { "BParentCollection_Id" });
            DropIndex("dbo.BParentCollection_1_x_BChildCollection", new[] { "BChild_Id" });
            DropIndex("dbo.UParentRequireds", new[] { "Id" });
            DropIndex("dbo.UParentCollections", new[] { "UChildRequiredId" });
            DropIndex("dbo.UParentCollections", new[] { "UChildOptionalId" });
            DropIndex("dbo.UChilds", new[] { "UParentRequired_UChildCollection_Id" });
            DropIndex("dbo.UChilds", new[] { "UParentOptional_UChildCollection_Id" });
            DropIndex("dbo.UChilds", new[] { "Id" });
            DropIndex("dbo.HiddenEntities", new[] { "UChildOptional_Id" });
            DropIndex("dbo.HiddenEntities", new[] { "Id" });
            DropIndex("dbo.Children", new[] { "Master_Children_Id" });
            DropIndex("dbo.Children", new[] { "ParentId" });
            DropIndex("dbo.BParentRequireds", new[] { "Id" });
            DropIndex("dbo.BParentOptionals", new[] { "BChildOptional_Id" });
            DropIndex("dbo.BParentOptionals", new[] { "Id" });
            DropIndex("dbo.BParentCollections", new[] { "BChildOptionalId" });
            DropIndex("dbo.BParentCollections", new[] { "BChildRequiredId" });
            DropIndex("dbo.BChilds", new[] { "BParentRequired_2Id" });
            DropIndex("dbo.BChilds", new[] { "BParentOptional_1Id" });
            DropIndex("dbo.BChilds", new[] { "Id" });
            DropTable("dbo.UParentCollection_x_UChildCollection");
            DropTable("dbo.BParentCollection_1_x_BChildCollection");
            DropTable("dbo.UParentRequireds");
            DropTable("dbo.UParentCollections");
            DropTable("dbo.SpatialProperties");
            DropTable("dbo.RenamedColumns");
            DropTable("dbo.ParserTests");
            DropTable("dbo.Masters");
            DropTable("dbo.UChilds");
            DropTable("dbo.HiddenEntities");
            DropTable("dbo.Children");
            DropTable("dbo.BParentRequireds");
            DropTable("dbo.BParentOptionals");
            DropTable("dbo.BParentCollections");
            DropTable("dbo.BChilds");
            DropTable("dbo.AllPropertyTypesRequireds");
            DropTable("dbo.AllPropertyTypesOptionals");
            DropTable("dbo.BaseClassWithRequiredProperties");
        }
    }
}
