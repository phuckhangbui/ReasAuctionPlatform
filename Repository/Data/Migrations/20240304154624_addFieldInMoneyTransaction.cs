using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class addFieldInMoneyTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "MoneyTransaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TransactionNo",
                table: "MoneyTransaction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TxnRef",
                table: "MoneyTransaction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "MoneyTransaction");

            migrationBuilder.DropColumn(
                name: "TransactionNo",
                table: "MoneyTransaction");

            migrationBuilder.DropColumn(
                name: "TxnRef",
                table: "MoneyTransaction");
        }
    }
}
