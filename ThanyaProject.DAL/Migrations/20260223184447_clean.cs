using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThanyaProject.DAL.Migrations
{
    /// <inheritdoc />
    public partial class clean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Devices",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_UserId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "Devices",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "DeviceName",
                table: "Devices",
                newName: "Name");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Notifications",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Notifications",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "Devices",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Devices",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Battery",
                table: "Devices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "Devices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Lat",
                table: "Devices",
                type: "decimal(9,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Long",
                table: "Devices",
                type: "decimal(9,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Devices",
                table: "Devices",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UserId_DeviceId",
                table: "Devices",
                columns: new[] { "UserId", "DeviceId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Devices",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_UserId_DeviceId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Battery",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Long",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Devices",
                newName: "SerialNumber");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Devices",
                newName: "DeviceName");

            migrationBuilder.AlterColumn<int>(
                name: "DeviceId",
                table: "Devices",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Devices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Devices",
                table: "Devices",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UserId",
                table: "Devices",
                column: "UserId");
        }
    }
}
