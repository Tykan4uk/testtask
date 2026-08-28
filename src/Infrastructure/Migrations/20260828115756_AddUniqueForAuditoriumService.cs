using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueForAuditoriumService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuditoriumServices_AuditoriumId",
                table: "AuditoriumServices");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriumServices_AuditoriumId_ServiceId",
                table: "AuditoriumServices",
                columns: new[] { "AuditoriumId", "ServiceId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuditoriumServices_AuditoriumId_ServiceId",
                table: "AuditoriumServices");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriumServices_AuditoriumId",
                table: "AuditoriumServices",
                column: "AuditoriumId");
        }
    }
}
