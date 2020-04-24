using Microsoft.EntityFrameworkCore.Migrations;

namespace SMGApp.EntityFramework.Migrations
{
    public partial class AddedNewServiceItemFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BagIncluded",
                table: "ServiceItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CaseIncluded",
                table: "ServiceItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ChargerIncluded",
                table: "ServiceItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeviceAccountPassword",
                table: "ServiceItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceAccountUsername",
                table: "ServiceItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceDescription",
                table: "ServiceItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DevicePassword",
                table: "ServiceItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "ServiceItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SimPassword",
                table: "ServiceItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BagIncluded",
                table: "ServiceItems");

            migrationBuilder.DropColumn(
                name: "CaseIncluded",
                table: "ServiceItems");

            migrationBuilder.DropColumn(
                name: "ChargerIncluded",
                table: "ServiceItems");

            migrationBuilder.DropColumn(
                name: "DeviceAccountPassword",
                table: "ServiceItems");

            migrationBuilder.DropColumn(
                name: "DeviceAccountUsername",
                table: "ServiceItems");

            migrationBuilder.DropColumn(
                name: "DeviceDescription",
                table: "ServiceItems");

            migrationBuilder.DropColumn(
                name: "DevicePassword",
                table: "ServiceItems");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "ServiceItems");

            migrationBuilder.DropColumn(
                name: "SimPassword",
                table: "ServiceItems");
        }
    }
}
