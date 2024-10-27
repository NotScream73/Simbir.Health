using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Simbir.Health.Document.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHospitalId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "hospital_id",
                table: "histories",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hospital_id",
                table: "histories");
        }
    }
}
