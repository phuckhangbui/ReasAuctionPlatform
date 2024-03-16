using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RealEstateDetail_ReasId",
                table: "RealEstateDetail");

            migrationBuilder.DropIndex(
                name: "IX_RealEstate_Type_Reas",
                table: "RealEstate");

            migrationBuilder.DropIndex(
                name: "IX_MoneyTransactionDetail_AuctionId",
                table: "MoneyTransactionDetail");

            migrationBuilder.DropIndex(
                name: "IX_MoneyTransactionDetail_ReasId",
                table: "MoneyTransactionDetail");

            migrationBuilder.DropIndex(
                name: "IX_MoneyTransaction_TypeId",
                table: "MoneyTransaction");

            migrationBuilder.DropIndex(
                name: "IX_DepositAmount_ReasId",
                table: "DepositAmount");

            migrationBuilder.DropIndex(
                name: "IX_DepositAmount_RuleId",
                table: "DepositAmount");

            migrationBuilder.DropIndex(
                name: "IX_AuctionsAccounting_AuctionId",
                table: "AuctionsAccounting");

            migrationBuilder.DropIndex(
                name: "IX_AuctionsAccounting_ReasId",
                table: "AuctionsAccounting");

            migrationBuilder.DropIndex(
                name: "IX_Auction_ReasId",
                table: "Auction");

            migrationBuilder.DropIndex(
                name: "IX_Account_MajorId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_RoleId",
                table: "Account");

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateDetail_ReasId",
                table: "RealEstateDetail",
                column: "ReasId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RealEstate_Type_Reas",
                table: "RealEstate",
                column: "Type_Reas",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransactionDetail_AuctionId",
                table: "MoneyTransactionDetail",
                column: "AuctionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransactionDetail_ReasId",
                table: "MoneyTransactionDetail",
                column: "ReasId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransaction_TypeId",
                table: "MoneyTransaction",
                column: "TypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepositAmount_ReasId",
                table: "DepositAmount",
                column: "ReasId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepositAmount_RuleId",
                table: "DepositAmount",
                column: "RuleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuctionsAccounting_AuctionId",
                table: "AuctionsAccounting",
                column: "AuctionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuctionsAccounting_ReasId",
                table: "AuctionsAccounting",
                column: "ReasId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auction_ReasId",
                table: "Auction",
                column: "ReasId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_MajorId",
                table: "Account",
                column: "MajorId",
                unique: true,
                filter: "[MajorId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Account_RoleId",
                table: "Account",
                column: "RoleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RealEstateDetail_ReasId",
                table: "RealEstateDetail");

            migrationBuilder.DropIndex(
                name: "IX_RealEstate_Type_Reas",
                table: "RealEstate");

            migrationBuilder.DropIndex(
                name: "IX_MoneyTransactionDetail_AuctionId",
                table: "MoneyTransactionDetail");

            migrationBuilder.DropIndex(
                name: "IX_MoneyTransactionDetail_ReasId",
                table: "MoneyTransactionDetail");

            migrationBuilder.DropIndex(
                name: "IX_MoneyTransaction_TypeId",
                table: "MoneyTransaction");

            migrationBuilder.DropIndex(
                name: "IX_DepositAmount_ReasId",
                table: "DepositAmount");

            migrationBuilder.DropIndex(
                name: "IX_DepositAmount_RuleId",
                table: "DepositAmount");

            migrationBuilder.DropIndex(
                name: "IX_AuctionsAccounting_AuctionId",
                table: "AuctionsAccounting");

            migrationBuilder.DropIndex(
                name: "IX_AuctionsAccounting_ReasId",
                table: "AuctionsAccounting");

            migrationBuilder.DropIndex(
                name: "IX_Auction_ReasId",
                table: "Auction");

            migrationBuilder.DropIndex(
                name: "IX_Account_MajorId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_RoleId",
                table: "Account");

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateDetail_ReasId",
                table: "RealEstateDetail",
                column: "ReasId");

            migrationBuilder.CreateIndex(
                name: "IX_RealEstate_Type_Reas",
                table: "RealEstate",
                column: "Type_Reas");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransactionDetail_AuctionId",
                table: "MoneyTransactionDetail",
                column: "AuctionId");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransactionDetail_ReasId",
                table: "MoneyTransactionDetail",
                column: "ReasId");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransaction_TypeId",
                table: "MoneyTransaction",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositAmount_ReasId",
                table: "DepositAmount",
                column: "ReasId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositAmount_RuleId",
                table: "DepositAmount",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionsAccounting_AuctionId",
                table: "AuctionsAccounting",
                column: "AuctionId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionsAccounting_ReasId",
                table: "AuctionsAccounting",
                column: "ReasId");

            migrationBuilder.CreateIndex(
                name: "IX_Auction_ReasId",
                table: "Auction",
                column: "ReasId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_MajorId",
                table: "Account",
                column: "MajorId",
                filter: "[MajorId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Account_RoleId",
                table: "Account",
                column: "RoleId");
        }
    }
}
