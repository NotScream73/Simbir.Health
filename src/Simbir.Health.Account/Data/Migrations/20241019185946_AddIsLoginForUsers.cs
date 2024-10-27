using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Simbir.Health.Account.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsLoginForUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_login",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_login",
                table: "users");
        }
    }
}
