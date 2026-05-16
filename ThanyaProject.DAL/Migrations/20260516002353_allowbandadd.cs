using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThanyaProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class allowbandadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HeartRate",
                table: "Devices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OxygenLevel",
                table: "Devices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeartRate",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "OxygenLevel",
                table: "Devices");
        }
    }
}