using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class deleteTransactionDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoneyTransactionDetail");

            migrationBuilder.AlterColumn<double>(
                name: "ReasPrice",
                table: "RealEstate",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");


            migrationBuilder.AlterColumn<double>(
                name: "Money",
                table: "MoneyTransaction",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "AccountReceiveId",
                table: "MoneyTransaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepositId",
                table: "MoneyTransaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReasId",
                table: "MoneyTransaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "DepositAmount",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "MaxAmount",
                table: "AuctionsAccounting",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<double>(
                name: "DepositAmount",
                table: "AuctionsAccounting",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<double>(
                name: "CommissionAmount",
                table: "AuctionsAccounting",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<double>(
                name: "AmountOwnerReceived",
                table: "AuctionsAccounting",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransaction_AccountReceiveId",
                table: "MoneyTransaction",
                column: "AccountReceiveId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_MoneyTransaction_DepositId",
            //    table: "MoneyTransaction",
            //    column: "DepositId",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_MoneyTransaction_ReasId",
            //    table: "MoneyTransaction",
            //    column: "ReasId",
            //    unique: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_MoneyTransaction_Account_AccountReceiveId",
            //    table: "MoneyTransaction",
            //    column: "AccountReceiveId",
            //    principalTable: "Account",
            //    principalColumn: "AccountId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_MoneyTransaction_DepositAmount_DepositId",
            //    table: "MoneyTransaction",
            //    column: "DepositId",
            //    principalTable: "DepositAmount",
            //    principalColumn: "DepositId",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_MoneyTransaction_RealEstate_ReasId",
            //    table: "MoneyTransaction",
            //    column: "ReasId",
            //    principalTable: "RealEstate",
            //    principalColumn: "ReasId",
            //    onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoneyTransaction_Account_AccountReceiveId",
                table: "MoneyTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_MoneyTransaction_DepositAmount_DepositId",
                table: "MoneyTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_MoneyTransaction_RealEstate_ReasId",
                table: "MoneyTransaction");

            migrationBuilder.DropIndex(
                name: "IX_MoneyTransaction_AccountReceiveId",
                table: "MoneyTransaction");

            migrationBuilder.DropIndex(
                name: "IX_MoneyTransaction_DepositId",
                table: "MoneyTransaction");

            migrationBuilder.DropIndex(
                name: "IX_MoneyTransaction_ReasId",
                table: "MoneyTransaction");


            migrationBuilder.DropColumn(
                name: "AccountReceiveId",
                table: "MoneyTransaction");

            migrationBuilder.DropColumn(
                name: "DepositId",
                table: "MoneyTransaction");

            migrationBuilder.DropColumn(
                name: "ReasId",
                table: "MoneyTransaction");

            migrationBuilder.AlterColumn<string>(
                name: "ReasPrice",
                table: "RealEstate",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Money",
                table: "MoneyTransaction",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Amount",
                table: "DepositAmount",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "MaxAmount",
                table: "AuctionsAccounting",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "DepositAmount",
                table: "AuctionsAccounting",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "CommissionAmount",
                table: "AuctionsAccounting",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "AmountOwnerReceived",
                table: "AuctionsAccounting",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateTable(
                name: "MoneyTransactionDetail",
                columns: table => new
                {
                    MoneyTransactionDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountReceiveId = table.Column<int>(type: "int", nullable: false),
                    AuctionId = table.Column<int>(type: "int", nullable: false),
                    MoneyTransactionId = table.Column<int>(type: "int", nullable: false),
                    ReasId = table.Column<int>(type: "int", nullable: false),
                    DateExecution = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RemainingAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmmount = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyTransactionDetail", x => x.MoneyTransactionDetailId);
                    table.ForeignKey(
                        name: "FK_MoneyTransactionDetail_Account_AccountReceiveId",
                        column: x => x.AccountReceiveId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MoneyTransactionDetail_Auction_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auction",
                        principalColumn: "AuctionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MoneyTransactionDetail_MoneyTransaction_MoneyTransactionId",
                        column: x => x.MoneyTransactionId,
                        principalTable: "MoneyTransaction",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MoneyTransactionDetail_RealEstate_ReasId",
                        column: x => x.ReasId,
                        principalTable: "RealEstate",
                        principalColumn: "ReasId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransactionDetail_AccountReceiveId",
                table: "MoneyTransactionDetail",
                column: "AccountReceiveId");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransactionDetail_AuctionId",
                table: "MoneyTransactionDetail",
                column: "AuctionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransactionDetail_MoneyTransactionId",
                table: "MoneyTransactionDetail",
                column: "MoneyTransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransactionDetail_ReasId",
                table: "MoneyTransactionDetail",
                column: "ReasId",
                unique: true);
        }
    }
}
