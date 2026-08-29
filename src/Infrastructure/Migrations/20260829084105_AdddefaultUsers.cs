using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdddefaultUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserInfos",
                columns: new[] { "Id", "Email", "Password", "UserName" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "admin@email.com", "AQAAAAIAAYagAAAAEHq+Bqv0k+G1nZ915gjGtJA17bcsK/wm8aROVZF5aVAGvDcTvZ+L8G1R4n6eYv4+ww==", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserInfos",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));
        }
    }
}
