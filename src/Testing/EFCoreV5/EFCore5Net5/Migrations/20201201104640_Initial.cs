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
                name: "EntityAbstract",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Test = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityAbstract", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entity1",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityImplementationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entity1_EntityAbstract_EntityImplementationId",
                        column: x => x.EntityImplementationId,
                        principalSchema: "dbo",
                        principalTable: "EntityAbstract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityRelated",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityAbstractId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityRelated", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityRelated_EntityAbstract_EntityAbstractId",
                        column: x => x.EntityAbstractId,
                        principalSchema: "dbo",
                        principalTable: "EntityAbstract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entity1_EntityImplementationId",
                schema: "dbo",
                table: "Entity1",
                column: "EntityImplementationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntityRelated_EntityAbstractId",
                schema: "dbo",
                table: "EntityRelated",
                column: "EntityAbstractId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entity1",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EntityRelated",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EntityAbstract",
                schema: "dbo");
        }
    }
}
