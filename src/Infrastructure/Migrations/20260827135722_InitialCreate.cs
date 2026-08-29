using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auditoriums",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    BaseRentalPrice = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditoriums", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Rate = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditoriumReserves",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuditoriumId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriumReserves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditoriumReserves_Auditoriums_AuditoriumId",
                        column: x => x.AuditoriumId,
                        principalTable: "Auditoriums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditoriumServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuditoriumId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriumServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditoriumServices_Auditoriums_AuditoriumId",
                        column: x => x.AuditoriumId,
                        principalTable: "Auditoriums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditoriumServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditoriumReserveServices",
                columns: table => new
                {
                    AuditoriumReserveId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuditoriumServiceId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriumReserveServices", x => new { x.AuditoriumReserveId, x.AuditoriumServiceId });
                    table.ForeignKey(
                        name: "FK_AuditoriumReserveServices_AuditoriumReserves_AuditoriumRese~",
                        column: x => x.AuditoriumReserveId,
                        principalTable: "AuditoriumReserves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditoriumReserveServices_AuditoriumServices_AuditoriumServ~",
                        column: x => x.AuditoriumServiceId,
                        principalTable: "AuditoriumServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Auditoriums",
                columns: new[] { "Id", "BaseRentalPrice", "Capacity", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 2000, 50, "Зал А" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 3500, 100, "Зал В" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 1500, 30, "Зал С" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Проєктор", 500 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Wi-Fi", 300 },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Звук", 700 }
                });

            migrationBuilder.InsertData(
                table: "TimeRates",
                columns: new[] { "Id", "EndTime", "Rate", "StartTime" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new TimeOnly(17, 59, 59), 1m, new TimeOnly(9, 0, 0) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new TimeOnly(22, 59, 59), 0.8m, new TimeOnly(18, 0, 0) },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new TimeOnly(8, 59, 59), 0.9m, new TimeOnly(6, 0, 0) },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new TimeOnly(13, 59, 59), 1.15m, new TimeOnly(12, 0, 0) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriumReserves_AuditoriumId",
                table: "AuditoriumReserves",
                column: "AuditoriumId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriumReserveServices_AuditoriumServiceId",
                table: "AuditoriumReserveServices",
                column: "AuditoriumServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriumServices_AuditoriumId",
                table: "AuditoriumServices",
                column: "AuditoriumId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriumServices_ServiceId",
                table: "AuditoriumServices",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditoriumReserveServices");

            migrationBuilder.DropTable(
                name: "TimeRates");

            migrationBuilder.DropTable(
                name: "AuditoriumReserves");

            migrationBuilder.DropTable(
                name: "AuditoriumServices");

            migrationBuilder.DropTable(
                name: "Auditoriums");

            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
