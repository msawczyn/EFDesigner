using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore2NetCore3.Migrations
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Id1 = table.Column<int>(nullable: false),
                    BinaryAttr = table.Column<byte[]>(nullable: true),
                    BooleanAttr = table.Column<bool>(nullable: true),
                    ByteAttr = table.Column<byte>(nullable: true),
                    DateTimeAttr = table.Column<DateTime>(nullable: true),
                    DateTimeOffsetAttr = table.Column<DateTimeOffset>(nullable: true),
                    DecimalAttr = table.Column<decimal>(nullable: true),
                    DoubleAttr = table.Column<double>(nullable: true),
                    GuidAttr = table.Column<Guid>(nullable: true),
                    Int16Attr = table.Column<short>(nullable: true),
                    Int32Attr = table.Column<int>(nullable: true),
                    Int64Attr = table.Column<long>(nullable: true),
                    SingleAttr = table.Column<float>(nullable: true),
                    StringAttr = table.Column<string>(nullable: true),
                    TimeAttr = table.Column<TimeSpan>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    OwnedType_SingleAttr = table.Column<float>(nullable: true)
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BinaryAttr = table.Column<byte[]>(nullable: false),
                    BooleanAttr = table.Column<bool>(nullable: false),
                    ByteAttr = table.Column<byte>(nullable: false),
                    DateTimeAttr = table.Column<DateTime>(nullable: false),
                    DateTimeOffsetAttr = table.Column<DateTimeOffset>(nullable: false),
                    DecimalAttr = table.Column<decimal>(nullable: false),
                    DoubleAttr = table.Column<double>(nullable: false),
                    GuidAttr = table.Column<Guid>(nullable: false),
                    Int16Attr = table.Column<short>(nullable: false),
                    Int32Attr = table.Column<int>(nullable: false),
                    Int64Attr = table.Column<long>(nullable: false),
                    SingleAttr = table.Column<float>(nullable: false),
                    StringAttr = table.Column<string>(nullable: false),
                    TimeAttr = table.Column<TimeSpan>(nullable: false)
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Property0 = table.Column<string>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Property1 = table.Column<string>(nullable: true),
                    PropertyInChild = table.Column<string>(nullable: true),
                    ConcreteDerivedClassWithRequiredProperties_Property1 = table.Column<string>(nullable: true),
                    DerivedClass_Property1 = table.Column<string>(nullable: true),
                    DerivedClass_PropertyInChild = table.Column<string>(nullable: true)
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name1 = table.Column<string>(nullable: true),
                    name2 = table.Column<string>(nullable: true),
                    name3 = table.Column<int>(nullable: true),
                    name4 = table.Column<int>(nullable: true),
                    name5 = table.Column<int>(nullable: true),
                    name6 = table.Column<int>(nullable: true),
                    name7 = table.Column<string>(nullable: true),
                    name8 = table.Column<string>(nullable: true),
                    name9 = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    name11 = table.Column<int>(nullable: true),
                    name12 = table.Column<int>(nullable: true),
                    name13 = table.Column<int>(nullable: true),
                    name14 = table.Column<int>(nullable: true),
                    name15 = table.Column<string>(nullable: true),
                    name16 = table.Column<string>(nullable: true),
                    name17 = table.Column<string>(nullable: true),
                    name18 = table.Column<string>(nullable: true)
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
                    Foo = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenamedColumns", x => x.Foo);
                });

            migrationBuilder.CreateTable(
                name: "UParentRequireds",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UParentRequireds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Children",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<int>(nullable: false),
                    Master_Children_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Children", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Children_Masters_Master_Children_Id",
                        column: x => x.Master_Children_Id,
                        principalSchema: "dbo",
                        principalTable: "Masters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Children_Children_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "dbo",
                        principalTable: "Children",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BParentCollections",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BChildRequiredId = table.Column<int>(nullable: true),
                    BChildOptionalId = table.Column<int>(nullable: true)
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BChildRequiredId = table.Column<int>(nullable: true)
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BParentRequiredId = table.Column<int>(nullable: false),
                    BParentRequired_1Id = table.Column<int>(nullable: true),
                    BParentRequired_2Id = table.Column<int>(nullable: false),
                    BParentOptional_1Id = table.Column<int>(nullable: true),
                    BParentOptional_2Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BChilds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BChilds_BParentOptionals_BParentOptional_1Id",
                        column: x => x.BParentOptional_1Id,
                        principalSchema: "dbo",
                        principalTable: "BParentOptionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BChilds_BParentOptionals_BParentOptional_2Id",
                        column: x => x.BParentOptional_2Id,
                        principalSchema: "dbo",
                        principalTable: "BParentOptionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BChilds_BParentRequireds_BParentRequiredId",
                        column: x => x.BParentRequiredId,
                        principalSchema: "dbo",
                        principalTable: "BParentRequireds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BChilds_BParentRequireds_BParentRequired_1Id",
                        column: x => x.BParentRequired_1Id,
                        principalSchema: "dbo",
                        principalTable: "BParentRequireds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BChilds_BParentRequireds_BParentRequired_2Id",
                        column: x => x.BParentRequired_2Id,
                        principalSchema: "dbo",
                        principalTable: "BParentRequireds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UChilds",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UParentOptional_UChildCollection_Id = table.Column<int>(nullable: true),
                    UParentOptional_UChildOptional_Id = table.Column<int>(nullable: true),
                    UParentRequired_UChildCollection_Id = table.Column<int>(nullable: false),
                    UParentRequired_UChildOptional_Id = table.Column<int>(nullable: false),
                    UParentRequired_UChildRequired_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UChilds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UChilds_UParentRequireds_UParentRequired_UChildCollection_Id",
                        column: x => x.UParentRequired_UChildCollection_Id,
                        principalSchema: "dbo",
                        principalTable: "UParentRequireds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UChilds_UParentRequireds_UParentRequired_UChildOptional_Id",
                        column: x => x.UParentRequired_UChildOptional_Id,
                        principalSchema: "dbo",
                        principalTable: "UParentRequireds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UChilds_UParentRequireds_UParentRequired_UChildRequired_Id",
                        column: x => x.UParentRequired_UChildRequired_Id,
                        principalSchema: "dbo",
                        principalTable: "UParentRequireds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HiddenEntities",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Property1 = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    PropertyInChild = table.Column<string>(nullable: true),
                    UChildRequiredId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HiddenEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HiddenEntities_UChilds_UChildRequiredId",
                        column: x => x.UChildRequiredId,
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UChildRequiredId = table.Column<int>(nullable: true),
                    UChildOptionalId = table.Column<int>(nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_BChilds_BParentOptional_1Id",
                schema: "dbo",
                table: "BChilds",
                column: "BParentOptional_1Id");

            migrationBuilder.CreateIndex(
                name: "IX_BChilds_BParentOptional_2Id",
                schema: "dbo",
                table: "BChilds",
                column: "BParentOptional_2Id",
                unique: true,
                filter: "[BParentOptional_2Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BChilds_BParentRequiredId",
                schema: "dbo",
                table: "BChilds",
                column: "BParentRequiredId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BChilds_BParentRequired_1Id",
                schema: "dbo",
                table: "BChilds",
                column: "BParentRequired_1Id",
                unique: true,
                filter: "[BParentRequired_1Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BChilds_BParentRequired_2Id",
                schema: "dbo",
                table: "BChilds",
                column: "BParentRequired_2Id");

            migrationBuilder.CreateIndex(
                name: "IX_BParentCollections_BChildOptionalId",
                schema: "dbo",
                table: "BParentCollections",
                column: "BChildOptionalId");

            migrationBuilder.CreateIndex(
                name: "IX_BParentCollections_BChildRequiredId",
                schema: "dbo",
                table: "BParentCollections",
                column: "BChildRequiredId");

            migrationBuilder.CreateIndex(
                name: "IX_BParentOptionals_BChildRequiredId",
                schema: "dbo",
                table: "BParentOptionals",
                column: "BChildRequiredId",
                unique: true,
                filter: "[BChildRequiredId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Children_Master_Children_Id",
                schema: "dbo",
                table: "Children",
                column: "Master_Children_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Children_ParentId",
                schema: "dbo",
                table: "Children",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_HiddenEntities_UChildRequiredId",
                schema: "dbo",
                table: "HiddenEntities",
                column: "UChildRequiredId",
                unique: true,
                filter: "[UChildRequiredId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UChilds_UParentOptional_UChildCollection_Id",
                schema: "dbo",
                table: "UChilds",
                column: "UParentOptional_UChildCollection_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UChilds_UParentOptional_UChildOptional_Id",
                schema: "dbo",
                table: "UChilds",
                column: "UParentOptional_UChildOptional_Id",
                unique: true,
                filter: "[UParentOptional_UChildOptional_Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UChilds_UParentRequired_UChildCollection_Id",
                schema: "dbo",
                table: "UChilds",
                column: "UParentRequired_UChildCollection_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UChilds_UParentRequired_UChildOptional_Id",
                schema: "dbo",
                table: "UChilds",
                column: "UParentRequired_UChildOptional_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UChilds_UParentRequired_UChildRequired_Id",
                schema: "dbo",
                table: "UChilds",
                column: "UParentRequired_UChildRequired_Id",
                unique: true,
                filter: "[UParentRequired_UChildRequired_Id] IS NOT NULL");

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

            migrationBuilder.AddForeignKey(
                name: "FK_BParentCollections_BChilds_BChildOptionalId",
                schema: "dbo",
                table: "BParentCollections",
                column: "BChildOptionalId",
                principalSchema: "dbo",
                principalTable: "BChilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BParentCollections_BChilds_BChildRequiredId",
                schema: "dbo",
                table: "BParentCollections",
                column: "BChildRequiredId",
                principalSchema: "dbo",
                principalTable: "BChilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BParentOptionals_BChilds_BChildRequiredId",
                schema: "dbo",
                table: "BParentOptionals",
                column: "BChildRequiredId",
                principalSchema: "dbo",
                principalTable: "BChilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UChilds_HiddenEntities_UParentOptional_UChildCollection_Id",
                schema: "dbo",
                table: "UChilds",
                column: "UParentOptional_UChildCollection_Id",
                principalSchema: "dbo",
                principalTable: "HiddenEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UChilds_HiddenEntities_UParentOptional_UChildOptional_Id",
                schema: "dbo",
                table: "UChilds",
                column: "UParentOptional_UChildOptional_Id",
                principalSchema: "dbo",
                principalTable: "HiddenEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BChilds_BParentOptionals_BParentOptional_1Id",
                schema: "dbo",
                table: "BChilds");

            migrationBuilder.DropForeignKey(
                name: "FK_BChilds_BParentOptionals_BParentOptional_2Id",
                schema: "dbo",
                table: "BChilds");

            migrationBuilder.DropForeignKey(
                name: "FK_HiddenEntities_UChilds_UChildRequiredId",
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
