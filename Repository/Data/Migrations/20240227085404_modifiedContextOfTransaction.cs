using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class modifiedContextOfTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoneyTransactionDetail_MoneyTransaction_MoneyTransactionDetailId",
                table: "MoneyTransactionDetail");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransactionDetail_MoneyTransactionId",
                table: "MoneyTransactionDetail",
                column: "MoneyTransactionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MoneyTransactionDetail_MoneyTransaction_MoneyTransactionId",
                table: "MoneyTransactionDetail",
                column: "MoneyTransactionId",
                principalTable: "MoneyTransaction",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoneyTransactionDetail_MoneyTransaction_MoneyTransactionId",
                table: "MoneyTransactionDetail");

            migrationBuilder.DropIndex(
                name: "IX_MoneyTransactionDetail_MoneyTransactionId",
                table: "MoneyTransactionDetail");

            migrationBuilder.AddForeignKey(
                name: "FK_MoneyTransactionDetail_MoneyTransaction_MoneyTransactionDetailId",
                table: "MoneyTransactionDetail",
                column: "MoneyTransactionDetailId",
                principalTable: "MoneyTransaction",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
