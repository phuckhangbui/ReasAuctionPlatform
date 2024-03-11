using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddParticipateHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParticipateAuctionHistories",
                columns: table => new
                {
                    ParticipateAuctionHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuctionAccountingId = table.Column<int>(type: "int", nullable: false),
                    AccountBidId = table.Column<int>(type: "int", nullable: false),
                    LastBid = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipateAuctionHistories", x => x.ParticipateAuctionHistoryId);
                    table.ForeignKey(
                        name: "FK_ParticipateAuctionHistories_Account_AccountBidId",
                        column: x => x.AccountBidId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParticipateAuctionHistories_AuctionsAccounting_AuctionAccountingId",
                        column: x => x.AuctionAccountingId,
                        principalTable: "AuctionsAccounting",
                        principalColumn: "AuctionAccountingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParticipateAuctionHistories_AccountBidId",
                table: "ParticipateAuctionHistories",
                column: "AccountBidId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipateAuctionHistories_AuctionAccountingId",
                table: "ParticipateAuctionHistories",
                column: "AuctionAccountingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParticipateAuctionHistories");
        }
    }
}
