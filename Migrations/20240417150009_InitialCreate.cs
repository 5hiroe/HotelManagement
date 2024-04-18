using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerFirstName = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerLastName = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerEmail = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerPhone = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoomType = table.Column<string>(type: "TEXT", nullable: false),
                    RoomCapacity = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    RoomStatus = table.Column<string>(type: "TEXT", nullable: false),
                    RoomPrice = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    StaffId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StaffFirstName = table.Column<string>(type: "TEXT", nullable: false),
                    StaffLastName = table.Column<string>(type: "TEXT", nullable: false),
                    StaffPosition = table.Column<string>(type: "TEXT", nullable: false),
                    StaffEmail = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.StaffId);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReservationCustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReservationCheckInDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReservationCheckOutDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReservationTotalPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    ReservationIsCancelled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_Reservations_Customers_ReservationCustomerId",
                        column: x => x.ReservationCustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomReservations",
                columns: table => new
                {
                    RoomReservationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoomReservationRoomId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomReservationReservationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomReservations", x => x.RoomReservationId);
                    table.ForeignKey(
                        name: "FK_RoomReservations_Reservations_RoomReservationReservationId",
                        column: x => x.RoomReservationReservationId,
                        principalTable: "Reservations",
                        principalColumn: "ReservationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomReservations_Rooms_RoomReservationRoomId",
                        column: x => x.RoomReservationRoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ReservationCustomerId",
                table: "Reservations",
                column: "ReservationCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomReservations_RoomReservationReservationId",
                table: "RoomReservations",
                column: "RoomReservationReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomReservations_RoomReservationRoomId",
                table: "RoomReservations",
                column: "RoomReservationRoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomReservations");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
