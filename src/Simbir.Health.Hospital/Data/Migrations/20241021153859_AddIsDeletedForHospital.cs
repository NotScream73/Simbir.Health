﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Simbir.Health.Hospital.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedForHospital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "hospitals",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "hospitals");
        }
    }
}
