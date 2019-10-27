using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sandbox_EFCore.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "BaseClasses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Discriminator = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Symbol = table.Column<string>(nullable: true),
                    Derived2_Name = table.Column<string>(nullable: true),
                    Derived2_Symbol = table.Column<string>(nullable: true),
                    Optional_Id = table.Column<int>(nullable: true),
                    Required_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseClasses_BaseClasses_Optional_Id",
                        column: x => x.Optional_Id,
                        principalSchema: "dbo",
                        principalTable: "BaseClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseClasses_BaseClasses_Required_Id",
                        column: x => x.Required_Id,
                        principalSchema: "dbo",
                        principalTable: "BaseClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseClasses_Name",
                schema: "dbo",
                table: "BaseClasses",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BaseClasses_Symbol",
                schema: "dbo",
                table: "BaseClasses",
                column: "Symbol",
                unique: true,
                filter: "[Symbol] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BaseClasses_Derived2_Name",
                schema: "dbo",
                table: "BaseClasses",
                column: "Derived2_Name",
                unique: true,
                filter: "[Derived2_Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BaseClasses_Optional_Id",
                schema: "dbo",
                table: "BaseClasses",
                column: "Optional_Id");

            migrationBuilder.CreateIndex(
                name: "IX_BaseClasses_Required_Id",
                schema: "dbo",
                table: "BaseClasses",
                column: "Required_Id");

            migrationBuilder.CreateIndex(
                name: "IX_BaseClasses_Derived2_Symbol",
                schema: "dbo",
                table: "BaseClasses",
                column: "Derived2_Symbol",
                unique: true,
                filter: "[Derived2_Symbol] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseClasses",
                schema: "dbo");
        }
    }
}
