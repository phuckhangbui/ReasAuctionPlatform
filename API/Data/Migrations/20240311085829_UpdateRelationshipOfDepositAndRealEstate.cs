using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationshipOfDepositAndRealEstate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepositAmount_RealEstate_ReasId",
                table: "DepositAmount");

            migrationBuilder.AddForeignKey(
                name: "FK_DepositAmount_RealEstate_ReasId",
                table: "DepositAmount",
                column: "ReasId",
                principalTable: "RealEstate",
                principalColumn: "ReasId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepositAmount_RealEstate_ReasId",
                table: "DepositAmount");

            migrationBuilder.AddForeignKey(
                name: "FK_DepositAmount_RealEstate_ReasId",
                table: "DepositAmount",
                column: "ReasId",
                principalTable: "RealEstate",
                principalColumn: "ReasId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
