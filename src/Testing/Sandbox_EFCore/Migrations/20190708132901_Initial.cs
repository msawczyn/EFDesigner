using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sandbox_EFCore.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "PressReleases",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PressRelease_PressReleases_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PressReleases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PressReleaseDetails",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PressRelease_Id = table.Column<int>(nullable: true),
                    PressReleaseDetail_PressReleaseDetails_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PressReleaseDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PressReleaseDetails_PressReleases_PressReleaseDetail_PressReleaseDetails_Id",
                        column: x => x.PressReleaseDetail_PressReleaseDetails_Id,
                        principalSchema: "dbo",
                        principalTable: "PressReleases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PressReleaseDetails_PressReleases_PressRelease_Id",
                        column: x => x.PressRelease_Id,
                        principalSchema: "dbo",
                        principalTable: "PressReleases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PressReleaseDetails_PressReleaseDetail_PressReleaseDetails_Id",
                schema: "dbo",
                table: "PressReleaseDetails",
                column: "PressReleaseDetail_PressReleaseDetails_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PressReleaseDetails_PressRelease_Id",
                schema: "dbo",
                table: "PressReleaseDetails",
                column: "PressRelease_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PressReleases_PressRelease_PressReleases_Id",
                schema: "dbo",
                table: "PressReleases",
                column: "PressRelease_PressReleases_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PressReleases_PressReleaseDetails_PressRelease_PressReleases_Id",
                schema: "dbo",
                table: "PressReleases",
                column: "PressRelease_PressReleases_Id",
                principalSchema: "dbo",
                principalTable: "PressReleaseDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PressReleaseDetails_PressReleases_PressReleaseDetail_PressReleaseDetails_Id",
                schema: "dbo",
                table: "PressReleaseDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PressReleaseDetails_PressReleases_PressRelease_Id",
                schema: "dbo",
                table: "PressReleaseDetails");

            migrationBuilder.DropTable(
                name: "PressReleases",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PressReleaseDetails",
                schema: "dbo");
        }
    }
}
