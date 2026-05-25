using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Data.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInActiveColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InActive",
                table: "Incomes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "InActive",
                table: "Expenses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "InActive",
                table: "Budgets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InActive",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "InActive",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "InActive",
                table: "Budgets");
        }
    }
}
