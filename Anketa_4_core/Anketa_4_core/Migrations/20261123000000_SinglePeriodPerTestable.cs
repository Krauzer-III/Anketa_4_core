using Anketa_4_core.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Anketa_4_core.Migrations
{
    [DbContext(typeof(AnketaContext))]
    [Migration("20261123000000_SinglePeriodPerTestable")]
    public partial class SinglePeriodPerTestable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Duplicates can contain distinct test and JSON records. Do not delete
            // or silently merge them in a schema migration: fail before any changes.
            migrationBuilder.Sql(@"
DO $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM ""AccessForTestables""
        GROUP BY ""TestableID""
        HAVING COUNT(*) > 1
    ) THEN
        RAISE EXCEPTION
            'Найдены тестируемые с несколькими назначениями. Миграция остановлена без удаления данных. Сначала сохраните и объедините результаты каждого AccessID.';
    END IF;
END $$;");

            migrationBuilder.DropIndex(
                name: "IX_AccessForTestables_TestableID",
                table: "AccessForTestables");
            migrationBuilder.CreateIndex(
                name: "IX_AccessForTestables_TestableID",
                table: "AccessForTestables",
                column: "TestableID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccessForTestables_TestableID",
                table: "AccessForTestables");
            migrationBuilder.CreateIndex(
                name: "IX_AccessForTestables_TestableID",
                table: "AccessForTestables",
                column: "TestableID");
        }
    }
}
