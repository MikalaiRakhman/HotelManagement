﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewRegisterModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumer",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumer",
                table: "Users");
        }
    }
}
