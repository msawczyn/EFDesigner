using Microsoft.EntityFrameworkCore.Migrations;

namespace Sandbox_EFCore5Net5_Test.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Entity1",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entity2",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Property1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Entity1Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entity2_Entity1_Entity1Id",
                        column: x => x.Entity1Id,
                        principalSchema: "dbo",
                        principalTable: "Entity1",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entity2_Entity1Id",
                schema: "dbo",
                table: "Entity2",
                column: "Entity1Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entity2",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Entity1",
                schema: "dbo");
        }
    }
}
