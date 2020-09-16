using Microsoft.EntityFrameworkCore.Migrations;

namespace Testing.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Entity11",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Property1 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Property2 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Property3 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity11", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bases",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Entity11_Id = table.Column<long>(type: "bigint", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Property1 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Property2 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bases_Entity11_Entity11_Id",
                        column: x => x.Entity11_Id,
                        principalSchema: "dbo",
                        principalTable: "Entity11",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Entity1",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Property1 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Property2 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Property3 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Entity2Id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entity1_Bases_Entity2Id",
                        column: x => x.Entity2Id,
                        principalSchema: "dbo",
                        principalTable: "Bases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Entity1Entity2",
                schema: "dbo",
                columns: table => new
                {
                    Entity1Id = table.Column<long>(type: "bigint", nullable: false),
                    Entity2_1Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity1Entity2", x => new { x.Entity1Id, x.Entity2_1Id });
                    table.ForeignKey(
                        name: "FK_Entity1Entity2_Bases_Entity2_1Id",
                        column: x => x.Entity2_1Id,
                        principalSchema: "dbo",
                        principalTable: "Bases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entity1Entity2_Entity1_Entity1Id",
                        column: x => x.Entity1Id,
                        principalSchema: "dbo",
                        principalTable: "Entity1",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bases_Entity11_Id",
                schema: "dbo",
                table: "Bases",
                column: "Entity11_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Entity1_Entity2Id",
                schema: "dbo",
                table: "Entity1",
                column: "Entity2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Entity1Entity2_Entity2_1Id",
                schema: "dbo",
                table: "Entity1Entity2",
                column: "Entity2_1Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entity1Entity2",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Entity1",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Bases",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Entity11",
                schema: "dbo");
        }
    }
}
