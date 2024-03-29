﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardStorageService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AccountsSessionEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "AccountSessions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "AccountSessions");
        }
    }
}
