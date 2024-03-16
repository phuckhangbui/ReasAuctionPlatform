using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class deleteLogMessageTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Task");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountWriterId = table.Column<int>(type: "int", nullable: false),
                    AccountWriterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_Logs_Account_AccountWriterId",
                        column: x => x.AccountWriterId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountReceiverId = table.Column<int>(type: "int", nullable: false),
                    AccountSerderId = table.Column<int>(type: "int", nullable: false),
                    AccounSerdertName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateSend = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Message_Account_AccountReceiverId",
                        column: x => x.AccountReceiverId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_Account_AccountSerderId",
                        column: x => x.AccountSerderId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountAssignedId = table.Column<int>(type: "int", nullable: false),
                    AccountCreateId = table.Column<int>(type: "int", nullable: false),
                    AccountAssignedName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountCreateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TaskContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskTitle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.TaskId);
                    table.ForeignKey(
                        name: "FK_Task_Account_AccountAssignedId",
                        column: x => x.AccountAssignedId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Task_Account_AccountCreateId",
                        column: x => x.AccountCreateId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_AccountWriterId",
                table: "Logs",
                column: "AccountWriterId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_AccountReceiverId",
                table: "Message",
                column: "AccountReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_AccountSerderId",
                table: "Message",
                column: "AccountSerderId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_AccountAssignedId",
                table: "Task",
                column: "AccountAssignedId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_AccountCreateId",
                table: "Task",
                column: "AccountCreateId");
        }
    }
}
