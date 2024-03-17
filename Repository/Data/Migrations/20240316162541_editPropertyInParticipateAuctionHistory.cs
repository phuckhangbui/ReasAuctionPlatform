using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class editPropertyInParticipateAuctionHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isWinner",
                table: "ParticipateAuctionHistories");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ParticipateAuctionHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ParticipateAuctionHistories");

            migrationBuilder.AddColumn<bool>(
                name: "isWinner",
                table: "ParticipateAuctionHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
