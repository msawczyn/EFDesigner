using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore5Net5.Migrations
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
                    Id = table.Column<int>(type: "int", nullable: false),
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
                    TimeAttr = table.Column<TimeSpan>(type: "time", nullable: true),
                    StringAttr = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllPropertyTypesOptionals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AllPropertyTypesRequireds",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
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
                    TimeAttr = table.Column<TimeSpan>(type: "time", nullable: false),
                    StringAttr = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllPropertyTypesRequireds", x => x.Id);
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
                    Id = table.Column<int>(type: "int", nullable: false),
                    foo = table.Column<long>(type: "bigint", nullable: false),
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
                name: "UParentRequireds",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    Master_Children_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Children", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Children_Children_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "dbo",
                        principalTable: "Children",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Children_Masters_Master_Children_Id",
                        column: x => x.Master_Children_Id,
                        principalSchema: "dbo",
                        principalTable: "Masters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BChild_BParentCollection_1_x_BParentCollection_BChildCollection",
                schema: "dbo",
                columns: table => new
                {
                    BChildCollectionId = table.Column<int>(type: "int", nullable: false),
                    BParentCollection_1Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BChild_BParentCollection_1_x_BParentCollection_BChildCollection", x => new { x.BChildCollectionId, x.BParentCollection_1Id });
                });

            migrationBuilder.CreateTable(
                name: "BParentCollections",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BChildRequiredId = table.Column<int>(type: "int", nullable: false),
                    BChildOptionalId = table.Column<int>(type: "int", nullable: true)
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
                    BChildRequiredId = table.Column<int>(type: "int", nullable: false)
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
                    BParentRequiredId = table.Column<int>(type: "int", nullable: false),
                    BParentRequired_1Id = table.Column<int>(type: "int", nullable: false),
                    BParentRequired_2Id = table.Column<int>(type: "int", nullable: false),
                    BParentOptional_1Id = table.Column<int>(type: "int", nullable: true),
                    BParentOptional_2Id = table.Column<int>(type: "int", nullable: true)
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
                        name: "FK_BChilds_BParentRequireds_BParentRequired_1Id",
                        column: x => x.BParentRequired_1Id,
                        principalSchema: "dbo",
                        principalTable: "BParentRequireds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BChilds_BParentRequireds_BParentRequired_2Id",
                        column: x => x.BParentRequired_2Id,
                        principalSchema: "dbo",
                        principalTable: "BParentRequireds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BChilds_BParentRequireds_BParentRequiredId",
                        column: x => x.BParentRequiredId,
                        principalSchema: "dbo",
                        principalTable: "BParentRequireds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UParentCollections",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UChildRequiredId = table.Column<int>(type: "int", nullable: false),
                    UChildOptionalId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UParentCollections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UParentOptionals",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyInChild = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UChildRequiredId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UParentOptionals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UChilds",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UParentOptional_UChildCollection_Id = table.Column<int>(type: "int", nullable: true),
                    UParentOptional_UChildOptional_Id = table.Column<int>(type: "int", nullable: true),
                    UParentRequired_UChildCollection_Id = table.Column<int>(type: "int", nullable: false),
                    UParentRequired_UChildOptional_Id = table.Column<int>(type: "int", nullable: false),
                    UParentRequired_UChildRequired_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UChilds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UChilds_UParentOptionals_UParentOptional_UChildCollection_Id",
                        column: x => x.UParentOptional_UChildCollection_Id,
                        principalSchema: "dbo",
                        principalTable: "UParentOptionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UChilds_UParentOptionals_UParentOptional_UChildOptional_Id",
                        column: x => x.UParentOptional_UChildOptional_Id,
                        principalSchema: "dbo",
                        principalTable: "UParentOptionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BChild_BParentCollection_1_x_BParentCollection_BChildCollection_BParentCollection_1Id",
                schema: "dbo",
                table: "BChild_BParentCollection_1_x_BParentCollection_BChildCollection",
                column: "BParentCollection_1Id");

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
                name: "IX_BChilds_BParentRequired_1Id",
                schema: "dbo",
                table: "BChilds",
                column: "BParentRequired_1Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BChilds_BParentRequired_2Id",
                schema: "dbo",
                table: "BChilds",
                column: "BParentRequired_2Id");

            migrationBuilder.CreateIndex(
                name: "IX_BChilds_BParentRequiredId",
                schema: "dbo",
                table: "BChilds",
                column: "BParentRequiredId",
                unique: true);

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
                unique: true);

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
                unique: true);

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
                name: "IX_UParentOptionals_UChildRequiredId",
                schema: "dbo",
                table: "UParentOptionals",
                column: "UChildRequiredId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BChild_BParentCollection_1_x_BParentCollection_BChildCollection_BChilds_BChildCollectionId",
                schema: "dbo",
                table: "BChild_BParentCollection_1_x_BParentCollection_BChildCollection",
                column: "BChildCollectionId",
                principalSchema: "dbo",
                principalTable: "BChilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BChild_BParentCollection_1_x_BParentCollection_BChildCollection_BParentCollections_BParentCollection_1Id",
                schema: "dbo",
                table: "BChild_BParentCollection_1_x_BParentCollection_BChildCollection",
                column: "BParentCollection_1Id",
                principalSchema: "dbo",
                principalTable: "BParentCollections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BParentOptionals_BChilds_BChildRequiredId",
                schema: "dbo",
                table: "BParentOptionals",
                column: "BChildRequiredId",
                principalSchema: "dbo",
                principalTable: "BChilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UParentCollections_UChilds_UChildOptionalId",
                schema: "dbo",
                table: "UParentCollections",
                column: "UChildOptionalId",
                principalSchema: "dbo",
                principalTable: "UChilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UParentCollections_UChilds_UChildRequiredId",
                schema: "dbo",
                table: "UParentCollections",
                column: "UChildRequiredId",
                principalSchema: "dbo",
                principalTable: "UChilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UParentOptionals_UChilds_UChildRequiredId",
                schema: "dbo",
                table: "UParentOptionals",
                column: "UChildRequiredId",
                principalSchema: "dbo",
                principalTable: "UChilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BParentOptionals_BChilds_BChildRequiredId",
                schema: "dbo",
                table: "BParentOptionals");

            migrationBuilder.DropForeignKey(
                name: "FK_UChilds_UParentOptionals_UParentOptional_UChildCollection_Id",
                schema: "dbo",
                table: "UChilds");

            migrationBuilder.DropForeignKey(
                name: "FK_UChilds_UParentOptionals_UParentOptional_UChildOptional_Id",
                schema: "dbo",
                table: "UChilds");

            migrationBuilder.DropTable(
                name: "AllPropertyTypesOptionals",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AllPropertyTypesRequireds",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BChild_BParentCollection_1_x_BParentCollection_BChildCollection",
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
                name: "BParentCollections",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Masters",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BChilds",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BParentOptionals",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BParentRequireds",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UParentOptionals",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UChilds",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UParentRequireds",
                schema: "dbo");
        }
    }
}
