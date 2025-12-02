using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleemCare.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddGuestExpiryToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "GuestExpiresAt",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestExpiresAt",
                table: "Users");
        }
    }
}
