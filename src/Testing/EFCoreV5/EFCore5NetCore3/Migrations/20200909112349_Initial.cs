using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore5NetCore3.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "AllPropertyTypesOptionals",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id1 = table.Column<int>(type: "int", nullable: false),
                    BinaryAttr = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    BooleanAttr = table.Column<bool>(type: "bit", nullable: true),
                    ByteAttr = table.Column<byte>(type: "tinyint", nullable: true),
                    DateTimeAttr = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateTimeOffsetAttr = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DecimalAttr = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DoubleAttr = table.Column<double>(type: "float", nullable: true),
                    GuidAttr = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Int16Attr = table.Column<short>(type: "smallint", nullable: true),
                    Int32Attr = table.Column<int>(type: "int", nullable: true),
                    Int64Attr = table.Column<long>(type: "bigint", nullable: true),
                    SingleAttr = table.Column<float>(type: "real", nullable: true),
                    StringAttr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeAttr = table.Column<TimeSpan>(type: "time", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    OwnedType_SingleAttr = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllPropertyTypesOptionals", x => new { x.Id, x.Id1 });
                });

            migrationBuilder.CreateTable(
                name: "AllPropertyTypesRequireds",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BinaryAttr = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    BooleanAttr = table.Column<bool>(type: "bit", nullable: false),
                    ByteAttr = table.Column<byte>(type: "tinyint", nullable: false),
                    DateTimeAttr = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeOffsetAttr = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DecimalAttr = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DoubleAttr = table.Column<double>(type: "float", nullable: false),
                    GuidAttr = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Int16Attr = table.Column<short>(type: "smallint", nullable: false),
                    Int32Attr = table.Column<int>(type: "int", nullable: false),
                    Int64Attr = table.Column<long>(type: "bigint", nullable: false),
                    SingleAttr = table.Column<float>(type: "real", nullable: false),
                    StringAttr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeAttr = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllPropertyTypesRequireds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseClassWithRequiredProperties",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Property0 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Property1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyInChild = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcreteDerivedClassWithRequiredProperties_Property1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DerivedClass_Property1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DerivedClass_PropertyInChild = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseClassWithRequiredProperties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BParentRequireds",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BParentRequireds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Masters",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Masters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParserTests",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name3 = table.Column<int>(type: "int", nullable: true),
                    name4 = table.Column<int>(type: "int", nullable: true),
                    name5 = table.Column<int>(type: "int", nullable: true),
                    name6 = table.Column<int>(type: "int", nullable: true),
                    name7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name11 = table.Column<int>(type: "int", nullable: true),
                    name12 = table.Column<int>(type: "int", nullable: true),
                    name13 = table.Column<int>(type: "int", nullable: true),
                    name14 = table.Column<int>(type: "int", nullable: true),
                    name15 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name16 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name17 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name18 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParserTests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RenamedColumns",
                schema: "dbo",
                columns: table => new
                {
                    Foo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenamedColumns", x => x.Foo);
                });

            migrationBuilder.CreateTable(
                name: "Children",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Parent_Id = table.Column<int>(type: "int", nullable: true),
                    Child_Children_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Children", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Children_Children_Parent_Id",
                        column: x => x.Parent_Id,
                        principalSchema: "dbo",
                        principalTable: "Children",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Children_Masters_Child_Children_Id",
                        column: x => x.Child_Children_Id,
                        principalSchema: "dbo",
                        principalTable: "Masters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BParentCollections",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BChildRequired_Id = table.Column<int>(type: "int", nullable: false),
                    BChildOptional_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BParentCollections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BParentOptionals",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BChildRequired_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BParentOptionals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BChilds",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Property1a = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BParentRequired_Id = table.Column<int>(type: "int", nullable: true),
                    BParentRequired_1_Id = table.Column<int>(type: "int", nullable: false),
                    BParentRequired_2_Id = table.Column<int>(type: "int", nullable: true),
                    BParentOptional_1_Id = table.Column<int>(type: "int", nullable: true),
                    BParentOptional_2_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BChilds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BChilds_BParentOptionals_BParentOptional_1_Id",
                        column: x => x.BParentOptional_1_Id,
                        principalSchema: "dbo",
                        principalTable: "BParentOptionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BChilds_BParentOptionals_BParentOptional_2_Id",
                        column: x => x.BParentOptional_2_Id,
                        principalSchema: "dbo",
                        principalTable: "BParentOptionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BChilds_BParentRequireds_BParentRequired_1_Id",
                        column: x => x.BParentRequired_1_Id,
                        principalSchema: "dbo",
                        principalTable: "BParentRequireds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BChilds_BParentRequireds_BParentRequired_2_Id",
                        column: x => x.BParentRequired_2_Id,
                        principalSchema: "dbo",
                        principalTable: "BParentRequireds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BChilds_BParentRequireds_BParentRequired_Id",
                        column: x => x.BParentRequired_Id,
                        principalSchema: "dbo",
                        principalTable: "BParentRequireds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UChilds",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UChild_UChildCollection_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UChilds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HiddenEntities",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Property1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyInChild = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "hello"),
                    UChild_UChildOptional_Id = table.Column<int>(type: "int", nullable: true),
                    UChildRequired_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HiddenEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HiddenEntities_UChilds_UChild_UChildOptional_Id",
                        column: x => x.UChild_UChildOptional_Id,
                        principalSchema: "dbo",
                        principalTable: "UChilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HiddenEntities_UChilds_UChildRequired_Id",
                        column: x => x.UChildRequired_Id,
                        principalSchema: "dbo",
                        principalTable: "UChilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UParentCollections",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UChildRequiredId = table.Column<int>(type: "int", nullable: true),
                    UChildOptionalId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UParentCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UParentCollections_UChilds_UChildOptionalId",
                        column: x => x.UChildOptionalId,
                        principalSchema: "dbo",
                        principalTable: "UChilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UParentCollections_UChilds_UChildRequiredId",
                        column: x => x.UChildRequiredId,
                        principalSchema: "dbo",
                        principalTable: "UChilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UParentRequireds",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Property1ab = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UChild_UChildRequired_Id = table.Column<int>(type: "int", nullable: false),
                    UChild_UChildOptional_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UParentRequireds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UParentRequireds_UChilds_UChild_UChildOptional_Id",
                        column: x => x.UChild_UChildOptional_Id,
                        principalSchema: "dbo",
                        principalTable: "UChilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UParentRequireds_UChilds_UChild_UChildRequired_Id",
                        column: x => x.UChild_UChildRequired_Id,
                        principalSchema: "dbo",
                        principalTable: "UChilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BChilds_BParentOptional_1_Id",
                schema: "dbo",
                table: "BChilds",
                column: "BParentOptional_1_Id");

            migrationBuilder.CreateIndex(
                name: "IX_BChilds_BParentOptional_2_Id",
                schema: "dbo",
                table: "BChilds",
                column: "BParentOptional_2_Id",
                unique: true,
                filter: "[BParentOptional_2_Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BChilds_BParentRequired_1_Id",
                schema: "dbo",
                table: "BChilds",
                column: "BParentRequired_1_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BChilds_BParentRequired_2_Id",
                schema: "dbo",
                table: "BChilds",
                column: "BParentRequired_2_Id");

            migrationBuilder.CreateIndex(
                name: "IX_BChilds_BParentRequired_Id",
                schema: "dbo",
                table: "BChilds",
                column: "BParentRequired_Id",
                unique: true,
                filter: "[BParentRequired_Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BParentCollections_BChildOptional_Id",
                schema: "dbo",
                table: "BParentCollections",
                column: "BChildOptional_Id");

            migrationBuilder.CreateIndex(
                name: "IX_BParentCollections_BChildRequired_Id",
                schema: "dbo",
                table: "BParentCollections",
                column: "BChildRequired_Id");

            migrationBuilder.CreateIndex(
                name: "IX_BParentOptionals_BChildRequired_Id",
                schema: "dbo",
                table: "BParentOptionals",
                column: "BChildRequired_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Children_Child_Children_Id",
                schema: "dbo",
                table: "Children",
                column: "Child_Children_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Children_Parent_Id",
                schema: "dbo",
                table: "Children",
                column: "Parent_Id");

            migrationBuilder.CreateIndex(
                name: "IX_HiddenEntities_UChild_UChildOptional_Id",
                schema: "dbo",
                table: "HiddenEntities",
                column: "UChild_UChildOptional_Id",
                unique: true,
                filter: "[UChild_UChildOptional_Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_HiddenEntities_UChildRequired_Id",
                schema: "dbo",
                table: "HiddenEntities",
                column: "UChildRequired_Id",
                unique: true,
                filter: "[UChildRequired_Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UChilds_UChild_UChildCollection_Id",
                schema: "dbo",
                table: "UChilds",
                column: "UChild_UChildCollection_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UParentCollections_UChildOptionalId",
                schema: "dbo",
                table: "UParentCollections",
                column: "UChildOptionalId");

            migrationBuilder.CreateIndex(
                name: "IX_UParentCollections_UChildRequiredId",
                schema: "dbo",
                table: "UParentCollections",
                column: "UChildRequiredId");

            migrationBuilder.CreateIndex(
                name: "IX_UParentRequireds_UChild_UChildOptional_Id",
                schema: "dbo",
                table: "UParentRequireds",
                column: "UChild_UChildOptional_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UParentRequireds_UChild_UChildRequired_Id",
                schema: "dbo",
                table: "UParentRequireds",
                column: "UChild_UChildRequired_Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BParentCollections_BChilds_BChildOptional_Id",
                schema: "dbo",
                table: "BParentCollections",
                column: "BChildOptional_Id",
                principalSchema: "dbo",
                principalTable: "BChilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BParentCollections_BChilds_BChildRequired_Id",
                schema: "dbo",
                table: "BParentCollections",
                column: "BChildRequired_Id",
                principalSchema: "dbo",
                principalTable: "BChilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BParentOptionals_BChilds_BChildRequired_Id",
                schema: "dbo",
                table: "BParentOptionals",
                column: "BChildRequired_Id",
                principalSchema: "dbo",
                principalTable: "BChilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UChilds_HiddenEntities_UChild_UChildCollection_Id",
                schema: "dbo",
                table: "UChilds",
                column: "UChild_UChildCollection_Id",
                principalSchema: "dbo",
                principalTable: "HiddenEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UChilds_UParentRequireds_UChild_UChildCollection_Id",
                schema: "dbo",
                table: "UChilds",
                column: "UChild_UChildCollection_Id",
                principalSchema: "dbo",
                principalTable: "UParentRequireds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BChilds_BParentOptionals_BParentOptional_1_Id",
                schema: "dbo",
                table: "BChilds");

            migrationBuilder.DropForeignKey(
                name: "FK_BChilds_BParentOptionals_BParentOptional_2_Id",
                schema: "dbo",
                table: "BChilds");

            migrationBuilder.DropForeignKey(
                name: "FK_HiddenEntities_UChilds_UChild_UChildOptional_Id",
                schema: "dbo",
                table: "HiddenEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_HiddenEntities_UChilds_UChildRequired_Id",
                schema: "dbo",
                table: "HiddenEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_HiddenEntities_UChilds_UChild_UChildOptional_Id",
                schema: "dbo",
                table: "HiddenEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_HiddenEntities_UChilds_UChildRequired_Id",
                schema: "dbo",
                table: "HiddenEntities");

            migrationBuilder.DropTable(
                name: "AllPropertyTypesOptionals",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AllPropertyTypesRequireds",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BaseClassWithRequiredProperties",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BParentCollections",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Children",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ParserTests",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RenamedColumns",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UParentCollections",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Masters",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BParentOptionals",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BChilds",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BParentRequireds",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UChilds",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "HiddenEntities",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UParentRequireds",
                schema: "dbo");
        }
    }
}
