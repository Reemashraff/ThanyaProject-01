using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThanyaProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EditInNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeviceId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Resolved",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_DeviceId",
                table: "Notifications",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Devices_DeviceId",
                table: "Notifications",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Devices_DeviceId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_DeviceId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Resolved",
                table: "Notifications");
        }
    }
}
