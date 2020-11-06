using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore5NetCore3.Migrations.BidirectionalAssociations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Detail3",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detail3", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Masters",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToOneDetail2_Id = table.Column<long>(type: "bigint", nullable: true),
                    ToZeroOrOneDetail3_Id = table.Column<long>(type: "bigint", nullable: true),
                    ToOneDetail3_Id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Masters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Masters_Detail3_ToOneDetail3_Id",
                        column: x => x.ToOneDetail3_Id,
                        principalSchema: "dbo",
                        principalTable: "Detail3",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Masters_Detail3_ToZeroOrOneDetail3_Id",
                        column: x => x.ToZeroOrOneDetail3_Id,
                        principalSchema: "dbo",
                        principalTable: "Detail3",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Detail1",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    B_Id = table.Column<long>(type: "bigint", nullable: false),
                    A_Id = table.Column<long>(type: "bigint", nullable: false),
                    C_Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detail1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Detail1_Masters_A_Id",
                        column: x => x.A_Id,
                        principalSchema: "dbo",
                        principalTable: "Masters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Detail1_Masters_B_Id",
                        column: x => x.B_Id,
                        principalSchema: "dbo",
                        principalTable: "Masters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Detail1_Masters_C_Id",
                        column: x => x.C_Id,
                        principalSchema: "dbo",
                        principalTable: "Masters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Detail2",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    B_Id = table.Column<long>(type: "bigint", nullable: true),
                    C_Id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detail2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Detail2_Masters_B_Id",
                        column: x => x.B_Id,
                        principalSchema: "dbo",
                        principalTable: "Masters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Detail2_Masters_C_Id",
                        column: x => x.C_Id,
                        principalSchema: "dbo",
                        principalTable: "Masters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ToManyDetail3_x_ToManyDetail3",
                schema: "dbo",
                columns: table => new
                {
                    CId = table.Column<long>(type: "bigint", nullable: false),
                    ToManyDetail3Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToManyDetail3_x_ToManyDetail3", x => new { x.CId, x.ToManyDetail3Id });
                    table.ForeignKey(
                        name: "FK_ToManyDetail3_x_ToManyDetail3_Detail3_ToManyDetail3Id",
                        column: x => x.ToManyDetail3Id,
                        principalSchema: "dbo",
                        principalTable: "Detail3",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToManyDetail3_x_ToManyDetail3_Masters_CId",
                        column: x => x.CId,
                        principalSchema: "dbo",
                        principalTable: "Masters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Detail1_A_Id",
                schema: "dbo",
                table: "Detail1",
                column: "A_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Detail1_B_Id",
                schema: "dbo",
                table: "Detail1",
                column: "B_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Detail1_C_Id",
                schema: "dbo",
                table: "Detail1",
                column: "C_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Detail2_B_Id",
                schema: "dbo",
                table: "Detail2",
                column: "B_Id",
                unique: true,
                filter: "[B_Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Detail2_C_Id",
                schema: "dbo",
                table: "Detail2",
                column: "C_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Masters_ToOneDetail2_Id",
                schema: "dbo",
                table: "Masters",
                column: "ToOneDetail2_Id",
                unique: true,
                filter: "[ToOneDetail2_Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Masters_ToOneDetail3_Id",
                schema: "dbo",
                table: "Masters",
                column: "ToOneDetail3_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Masters_ToZeroOrOneDetail3_Id",
                schema: "dbo",
                table: "Masters",
                column: "ToZeroOrOneDetail3_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ToManyDetail3_x_ToManyDetail3_ToManyDetail3Id",
                schema: "dbo",
                table: "ToManyDetail3_x_ToManyDetail3",
                column: "ToManyDetail3Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Masters_Detail2_ToOneDetail2_Id",
                schema: "dbo",
                table: "Masters",
                column: "ToOneDetail2_Id",
                principalSchema: "dbo",
                principalTable: "Detail2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Detail2_Masters_B_Id",
                schema: "dbo",
                table: "Detail2");

            migrationBuilder.DropForeignKey(
                name: "FK_Detail2_Masters_C_Id",
                schema: "dbo",
                table: "Detail2");

            migrationBuilder.DropTable(
                name: "Detail1",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ToManyDetail3_x_ToManyDetail3",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Masters",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Detail2",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Detail3",
                schema: "dbo");
        }
    }
}
