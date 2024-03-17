using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Major",
                columns: table => new
                {
                    MajorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MajorName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Major", x => x.MajorId);
                });

            migrationBuilder.CreateTable(
                name: "MoneyTransactionType",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyTransactionType", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Rule",
                columns: table => new
                {
                    RuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rule", x => x.RuleId);
                });

            migrationBuilder.CreateTable(
                name: "type_REAS",
                columns: table => new
                {
                    Type_ReasId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type_Reas_Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_type_REAS", x => x.Type_ReasId);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    AccountEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Citizen_identification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MajorId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Account_Status = table.Column<int>(type: "int", nullable: false),
                    Date_Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_End = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Account_Major_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Major",
                        principalColumn: "MajorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Account_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountWriterId = table.Column<int>(type: "int", nullable: false),
                    AccountWriterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogEndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountSerderId = table.Column<int>(type: "int", nullable: false),
                    AccounSerdertName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountReceiverId = table.Column<int>(type: "int", nullable: false),
                    DateSend = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "MoneyTransaction",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    AccountSendId = table.Column<int>(type: "int", nullable: false),
                    TransactionStatus = table.Column<int>(type: "int", nullable: false),
                    Money = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateExecution = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyTransaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_MoneyTransaction_Account_AccountSendId",
                        column: x => x.AccountSendId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MoneyTransaction_MoneyTransactionType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "MoneyTransactionType",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    NewsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountCreateId = table.Column<int>(type: "int", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewsTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewsContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.NewsId);
                    table.ForeignKey(
                        name: "FK_News_Account_AccountCreateId",
                        column: x => x.AccountCreateId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RealEstate",
                columns: table => new
                {
                    ReasId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReasName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReasAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReasPrice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReasDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReasStatus = table.Column<int>(type: "int", nullable: false),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type_Reas = table.Column<int>(type: "int", nullable: false),
                    AccountOwnerId = table.Column<int>(type: "int", nullable: false),
                    AccountOwnerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealEstate", x => x.ReasId);
                    table.ForeignKey(
                        name: "FK_RealEstate_Account_AccountOwnerId",
                        column: x => x.AccountOwnerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RealEstate_type_REAS_Type_Reas",
                        column: x => x.Type_Reas,
                        principalTable: "type_REAS",
                        principalColumn: "Type_ReasId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountCreateId = table.Column<int>(type: "int", nullable: false),
                    AccountCreateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountAssignedId = table.Column<int>(type: "int", nullable: false),
                    AccountAssignedName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TaskNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Auction",
                columns: table => new
                {
                    AuctionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReasId = table.Column<int>(type: "int", nullable: false),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountCreateId = table.Column<int>(type: "int", nullable: false),
                    AccountCreateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auction", x => x.AuctionId);
                    table.ForeignKey(
                        name: "FK_Auction_RealEstate",
                        column: x => x.AccountCreateId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Auction_RealEstate_ReasId",
                        column: x => x.ReasId,
                        principalTable: "RealEstate",
                        principalColumn: "ReasId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepositAmount",
                columns: table => new
                {
                    DepositId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RuleId = table.Column<int>(type: "int", nullable: false),
                    AccountSignId = table.Column<int>(type: "int", nullable: false),
                    ReasId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateSign = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositAmount", x => x.DepositId);
                    table.ForeignKey(
                        name: "FK_DepositAmount_Account_AccountSignId",
                        column: x => x.AccountSignId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepositAmount_RealEstate_ReasId",
                        column: x => x.ReasId,
                        principalTable: "RealEstate",
                        principalColumn: "ReasId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepositAmount_Rule_RuleId",
                        column: x => x.RuleId,
                        principalTable: "Rule",
                        principalColumn: "RuleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RealEstateDetail",
                columns: table => new
                {
                    ReasDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReasId = table.Column<int>(type: "int", nullable: false),
                    Reas_Cert_Of_Land_Img_Front = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reas_Cert_Of_Land_Img_After = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reas_Cert_Of_Home_Ownership = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reas_Registration_Book = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Documents_Proving_Marital_Relationship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sales_Authorization_Contract = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealEstateDetail", x => x.ReasDetailId);
                    table.ForeignKey(
                        name: "FK_RealEstateDetail_RealEstate_ReasId",
                        column: x => x.ReasId,
                        principalTable: "RealEstate",
                        principalColumn: "ReasId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RealEstatePhoto",
                columns: table => new
                {
                    ReasPhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReasPhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealEstatePhoto", x => x.ReasPhotoId);
                    table.ForeignKey(
                        name: "FK_RealEstatePhoto_RealEstate_ReasId",
                        column: x => x.ReasId,
                        principalTable: "RealEstate",
                        principalColumn: "ReasId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuctionsAccounting",
                columns: table => new
                {
                    AuctionAccountingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuctionId = table.Column<int>(type: "int", nullable: false),
                    ReasId = table.Column<int>(type: "int", nullable: false),
                    DepositAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommissionAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmountOwnerReceived = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountWinId = table.Column<int>(type: "int", nullable: false),
                    AccountWinName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountOwnerId = table.Column<int>(type: "int", nullable: false),
                    AccountOwnerName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionsAccounting", x => x.AuctionAccountingId);
                    table.ForeignKey(
                        name: "FK_AuctionAccounting_RealEstate",
                        column: x => x.ReasId,
                        principalTable: "RealEstate",
                        principalColumn: "ReasId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuctionsAccounting_Account_AccountOwnerId",
                        column: x => x.AccountOwnerId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuctionsAccounting_Account_AccountWinId",
                        column: x => x.AccountWinId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuctionsAccounting_Auction_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auction",
                        principalColumn: "AuctionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MoneyTransactionDetail",
                columns: table => new
                {
                    MoneyTransactionDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoneyTransactionId = table.Column<int>(type: "int", nullable: false),
                    AccountReceiveId = table.Column<int>(type: "int", nullable: false),
                    ReasId = table.Column<int>(type: "int", nullable: false),
                    AuctionId = table.Column<int>(type: "int", nullable: false),
                    TotalAmmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaidAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RemainingAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateExecution = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        name: "FK_MoneyTransactionDetail_MoneyTransaction_MoneyTransactionDetailId",
                        column: x => x.MoneyTransactionDetailId,
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
                name: "IX_Account_MajorId",
                table: "Account",
                column: "MajorId",
                unique: false,
                filter: "[MajorId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Account_RoleId",
                table: "Account",
                column: "RoleId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_Auction_AccountCreateId",
                table: "Auction",
                column: "AccountCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_Auction_ReasId",
                table: "Auction",
                column: "ReasId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_AuctionsAccounting_AccountOwnerId",
                table: "AuctionsAccounting",
                column: "AccountOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionsAccounting_AccountWinId",
                table: "AuctionsAccounting",
                column: "AccountWinId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionsAccounting_AuctionId",
                table: "AuctionsAccounting",
                column: "AuctionId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_AuctionsAccounting_ReasId",
                table: "AuctionsAccounting",
                column: "ReasId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_DepositAmount_AccountSignId",
                table: "DepositAmount",
                column: "AccountSignId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositAmount_ReasId",
                table: "DepositAmount",
                column: "ReasId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_DepositAmount_RuleId",
                table: "DepositAmount",
                column: "RuleId",
                unique: false);

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
                name: "IX_MoneyTransaction_AccountSendId",
                table: "MoneyTransaction",
                column: "AccountSendId");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransaction_TypeId",
                table: "MoneyTransaction",
                column: "TypeId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransactionDetail_AccountReceiveId",
                table: "MoneyTransactionDetail",
                column: "AccountReceiveId");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransactionDetail_AuctionId",
                table: "MoneyTransactionDetail",
                column: "AuctionId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_MoneyTransactionDetail_ReasId",
                table: "MoneyTransactionDetail",
                column: "ReasId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_News_AccountCreateId",
                table: "News",
                column: "AccountCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_RealEstate_AccountOwnerId",
                table: "RealEstate",
                column: "AccountOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_RealEstate_Type_Reas",
                table: "RealEstate",
                column: "Type_Reas",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateDetail_ReasId",
                table: "RealEstateDetail",
                column: "ReasId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_RealEstatePhoto_ReasId",
                table: "RealEstatePhoto",
                column: "ReasId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_AccountAssignedId",
                table: "Task",
                column: "AccountAssignedId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_AccountCreateId",
                table: "Task",
                column: "AccountCreateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuctionsAccounting");

            migrationBuilder.DropTable(
                name: "DepositAmount");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "MoneyTransactionDetail");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "RealEstateDetail");

            migrationBuilder.DropTable(
                name: "RealEstatePhoto");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "Rule");

            migrationBuilder.DropTable(
                name: "Auction");

            migrationBuilder.DropTable(
                name: "MoneyTransaction");

            migrationBuilder.DropTable(
                name: "RealEstate");

            migrationBuilder.DropTable(
                name: "MoneyTransactionType");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "type_REAS");

            migrationBuilder.DropTable(
                name: "Major");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
