using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Anketa_4_core.Migrations
{
    /// <inheritdoc />
    public partial class migr_03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MVC_TestableView",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    FilialName = table.Column<string>(type: "text", nullable: false),
                    ReservLevelName = table.Column<string>(type: "text", nullable: false),
                    YearTraining = table.Column<int>(type: "integer", nullable: false),
                    isArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MVC_TestableView", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserFilials",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    FilialID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFilials", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserFilials_Filials_FilialID",
                        column: x => x.FilialID,
                        principalTable: "Filials",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFilials_FilialID",
                table: "UserFilials",
                column: "FilialID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MVC_TestableView");

            migrationBuilder.DropTable(
                name: "UserFilials");
        }
    }
}
