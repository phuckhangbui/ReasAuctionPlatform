﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("API.Entity.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"));

                    b.Property<string>("AccountEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Account_Status")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Citizen_identification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date_Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_End")
                        .HasColumnType("datetime2");

                    b.Property<int?>("MajorId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountId");

                    b.HasIndex("MajorId")
                        .IsUnique();

                    b.HasIndex("RoleId")
                        .IsUnique();

                    b.ToTable("Account");
                });

            modelBuilder.Entity("API.Entity.Auction", b =>
                {
                    b.Property<int>("AuctionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuctionId"));

                    b.Property<int>("AccountCreateId")
                        .HasColumnType("int");

                    b.Property<string>("AccountCreateName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateStart")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReasId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("AuctionId");

                    b.HasIndex("AccountCreateId");

                    b.HasIndex("ReasId")
                        .IsUnique();

                    b.ToTable("Auction");
                });

            modelBuilder.Entity("API.Entity.AuctionAccounting", b =>
                {
                    b.Property<int>("AuctionAccountingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuctionAccountingId"));

                    b.Property<int>("AccountOwnerId")
                        .HasColumnType("int");

                    b.Property<string>("AccountOwnerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AccountWinId")
                        .HasColumnType("int");

                    b.Property<string>("AccountWinName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AmountOwnerReceived")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AuctionId")
                        .HasColumnType("int");

                    b.Property<string>("CommissionAmount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DepositAmount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EstimatedPaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MaxAmount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReasId")
                        .HasColumnType("int");

                    b.HasKey("AuctionAccountingId");

                    b.HasIndex("AccountOwnerId");

                    b.HasIndex("AccountWinId");

                    b.HasIndex("AuctionId")
                        .IsUnique();

                    b.HasIndex("ReasId")
                        .IsUnique();

                    b.ToTable("AuctionsAccounting");
                });

            modelBuilder.Entity("API.Entity.DepositAmount", b =>
                {
                    b.Property<int>("DepositId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepositId"));

                    b.Property<int>("AccountSignId")
                        .HasColumnType("int");

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateSign")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReasId")
                        .HasColumnType("int");

                    b.Property<int>("RuleId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("DepositId");

                    b.HasIndex("AccountSignId");

                    b.HasIndex("ReasId")
                        .IsUnique();

                    b.HasIndex("RuleId")
                        .IsUnique();

                    b.ToTable("DepositAmount");
                });

            modelBuilder.Entity("API.Entity.Log", b =>
                {
                    b.Property<int>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LogId"));

                    b.Property<int>("AccountWriterId")
                        .HasColumnType("int");

                    b.Property<string>("AccountWriterName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LogEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LogStartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LogId");

                    b.HasIndex("AccountWriterId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("API.Entity.Major", b =>
                {
                    b.Property<int>("MajorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MajorId"));

                    b.Property<string>("MajorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MajorId");

                    b.ToTable("Major");
                });

            modelBuilder.Entity("API.Entity.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MessageId"));

                    b.Property<string>("AccounSerdertName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AccountReceiverId")
                        .HasColumnType("int");

                    b.Property<int>("AccountSerderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateSend")
                        .HasColumnType("datetime2");

                    b.Property<string>("MessageContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MessageId");

                    b.HasIndex("AccountReceiverId");

                    b.HasIndex("AccountSerderId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("API.Entity.MoneyTransaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionId"));

                    b.Property<int>("AccountSendId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateExecution")
                        .HasColumnType("datetime2");

                    b.Property<string>("Money")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TransactionStatus")
                        .HasColumnType("int");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.HasKey("TransactionId");

                    b.HasIndex("AccountSendId");

                    b.HasIndex("TypeId")
                        .IsUnique();

                    b.ToTable("MoneyTransaction");
                });

            modelBuilder.Entity("API.Entity.MoneyTransactionDetail", b =>
                {
                    b.Property<int>("MoneyTransactionDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MoneyTransactionDetailId"));

                    b.Property<int>("AccountReceiveId")
                        .HasColumnType("int");

                    b.Property<int>("AuctionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateExecution")
                        .HasColumnType("datetime2");

                    b.Property<int>("MoneyTransactionId")
                        .HasColumnType("int");

                    b.Property<string>("PaidAmount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReasId")
                        .HasColumnType("int");

                    b.Property<string>("RemainingAmount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TotalAmmount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MoneyTransactionDetailId");

                    b.HasIndex("AccountReceiveId");

                    b.HasIndex("AuctionId")
                        .IsUnique();

                    b.HasIndex("ReasId")
                        .IsUnique();

                    b.ToTable("MoneyTransactionDetail");
                });

            modelBuilder.Entity("API.Entity.MoneyTransactionType", b =>
                {
                    b.Property<int>("TypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TypeId"));

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TypeId");

                    b.ToTable("MoneyTransactionType");
                });

            modelBuilder.Entity("API.Entity.News", b =>
                {
                    b.Property<int>("NewsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NewsId"));

                    b.Property<int>("AccountCreateId")
                        .HasColumnType("int");

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("NewsContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewsTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NewsId");

                    b.HasIndex("AccountCreateId");

                    b.ToTable("News");
                });

            modelBuilder.Entity("API.Entity.RealEstate", b =>
                {
                    b.Property<int>("ReasId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReasId"));

                    b.Property<int>("AccountOwnerId")
                        .HasColumnType("int");

                    b.Property<string>("AccountOwnerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateStart")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReasAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReasDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReasName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReasPrice")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReasStatus")
                        .HasColumnType("int");

                    b.HasKey("ReasId");

                    b.HasIndex("AccountOwnerId");

                    b.ToTable("RealEstate");
                });

            modelBuilder.Entity("API.Entity.RealEstateDetail", b =>
                {
                    b.Property<int>("ReasDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReasDetailId"));

                    b.Property<int>("RealEstateReasId")
                        .HasColumnType("int");

                    b.Property<int>("ReasId")
                        .HasColumnType("int");

                    b.HasKey("ReasDetailId");

                    b.HasIndex("RealEstateReasId");

                    b.ToTable("RealEstateDetail");
                });

            modelBuilder.Entity("API.Entity.RealEstatePhoto", b =>
                {
                    b.Property<int>("ReasPhotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReasPhotoId"));

                    b.Property<int>("RealEstateReasId")
                        .HasColumnType("int");

                    b.Property<int>("ReasId")
                        .HasColumnType("int");

                    b.Property<string>("ReasPhotoUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ReasPhotoId");

                    b.HasIndex("RealEstateReasId");

                    b.ToTable("RealEstatePhoto");
                });

            modelBuilder.Entity("API.Entity.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("API.Entity.Rule", b =>
                {
                    b.Property<int>("RuleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RuleId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.HasKey("RuleId");

                    b.ToTable("Rule");
                });

            modelBuilder.Entity("API.Entity.Task", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaskId"));

                    b.Property<int>("AccountAssignedId")
                        .HasColumnType("int");

                    b.Property<string>("AccountAssignedName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AccountCreateId")
                        .HasColumnType("int");

                    b.Property<string>("AccountCreateName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TaskContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaskNotes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaskTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TaskId");

                    b.HasIndex("AccountAssignedId");

                    b.HasIndex("AccountCreateId");

                    b.ToTable("Task");
                });

            modelBuilder.Entity("API.Entity.Account", b =>
                {
                    b.HasOne("API.Entity.Major", "Major")
                        .WithOne()
                        .HasForeignKey("API.Entity.Account", "MajorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Entity.Role", "Role")
                        .WithOne()
                        .HasForeignKey("API.Entity.Account", "RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Major");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("API.Entity.Auction", b =>
                {
                    b.HasOne("API.Entity.Account", "AcountCreate")
                        .WithMany("Auctions")
                        .HasForeignKey("AccountCreateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Auction_RealEstate");

                    b.HasOne("API.Entity.RealEstate", "RealEstate")
                        .WithOne()
                        .HasForeignKey("API.Entity.Auction", "ReasId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AcountCreate");

                    b.Navigation("RealEstate");
                });

            modelBuilder.Entity("API.Entity.AuctionAccounting", b =>
                {
                    b.HasOne("API.Entity.Account", "AccountOwner")
                        .WithMany("OwnedAuctionAccountings")
                        .HasForeignKey("AccountOwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Entity.Account", "AccountWin")
                        .WithMany("WonAuctionAccountings")
                        .HasForeignKey("AccountWinId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Entity.Auction", "Auction")
                        .WithOne("AuctionAccounting")
                        .HasForeignKey("API.Entity.AuctionAccounting", "AuctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entity.RealEstate", "RealEstate")
                        .WithOne()
                        .HasForeignKey("API.Entity.AuctionAccounting", "ReasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_AuctionAccounting_RealEstate");

                    b.Navigation("AccountOwner");

                    b.Navigation("AccountWin");

                    b.Navigation("Auction");

                    b.Navigation("RealEstate");
                });

            modelBuilder.Entity("API.Entity.DepositAmount", b =>
                {
                    b.HasOne("API.Entity.Account", "AccountSign")
                        .WithMany("DepositAmount")
                        .HasForeignKey("AccountSignId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Entity.RealEstate", "RealEstate")
                        .WithOne()
                        .HasForeignKey("API.Entity.DepositAmount", "ReasId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Entity.Rule", "Rule")
                        .WithOne()
                        .HasForeignKey("API.Entity.DepositAmount", "RuleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AccountSign");

                    b.Navigation("RealEstate");

                    b.Navigation("Rule");
                });

            modelBuilder.Entity("API.Entity.Log", b =>
                {
                    b.HasOne("API.Entity.Account", "AccountWriter")
                        .WithMany("LogWrote")
                        .HasForeignKey("AccountWriterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AccountWriter");
                });

            modelBuilder.Entity("API.Entity.Message", b =>
                {
                    b.HasOne("API.Entity.Account", "AccountReceiver")
                        .WithMany("MessagesReceived")
                        .HasForeignKey("AccountReceiverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Entity.Account", "AccountSerder")
                        .WithMany("MessagesSent")
                        .HasForeignKey("AccountSerderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AccountReceiver");

                    b.Navigation("AccountSerder");
                });

            modelBuilder.Entity("API.Entity.MoneyTransaction", b =>
                {
                    b.HasOne("API.Entity.Account", "AccountSend")
                        .WithMany("MoneyTransactions")
                        .HasForeignKey("AccountSendId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Entity.MoneyTransactionType", "Type")
                        .WithOne()
                        .HasForeignKey("API.Entity.MoneyTransaction", "TypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AccountSend");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("API.Entity.MoneyTransactionDetail", b =>
                {
                    b.HasOne("API.Entity.Account", "AccountReceive")
                        .WithMany("MoneyTransactionDetails")
                        .HasForeignKey("AccountReceiveId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Entity.Auction", "Auction")
                        .WithOne()
                        .HasForeignKey("API.Entity.MoneyTransactionDetail", "AuctionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Entity.MoneyTransaction", "MoneyTransaction")
                        .WithOne("MoneyTransactionDetail")
                        .HasForeignKey("API.Entity.MoneyTransactionDetail", "MoneyTransactionDetailId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Entity.RealEstate", "RealEstate")
                        .WithOne()
                        .HasForeignKey("API.Entity.MoneyTransactionDetail", "ReasId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AccountReceive");

                    b.Navigation("Auction");

                    b.Navigation("MoneyTransaction");

                    b.Navigation("RealEstate");
                });

            modelBuilder.Entity("API.Entity.News", b =>
                {
                    b.HasOne("API.Entity.Account", "AccountCreate")
                        .WithMany("NewsCreated")
                        .HasForeignKey("AccountCreateId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AccountCreate");
                });

            modelBuilder.Entity("API.Entity.RealEstate", b =>
                {
                    b.HasOne("API.Entity.Account", "AccountOwner")
                        .WithMany("RealEstate")
                        .HasForeignKey("AccountOwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AccountOwner");
                });

            modelBuilder.Entity("API.Entity.RealEstateDetail", b =>
                {
                    b.HasOne("API.Entity.RealEstate", "RealEstate")
                        .WithMany()
                        .HasForeignKey("RealEstateReasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RealEstate");
                });

            modelBuilder.Entity("API.Entity.RealEstatePhoto", b =>
                {
                    b.HasOne("API.Entity.RealEstate", "RealEstate")
                        .WithMany("Photos")
                        .HasForeignKey("RealEstateReasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RealEstate");
                });

            modelBuilder.Entity("API.Entity.Task", b =>
                {
                    b.HasOne("API.Entity.Account", "AccountAssigned")
                        .WithMany("TasksAssigned")
                        .HasForeignKey("AccountAssignedId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Entity.Account", "AccountCreate")
                        .WithMany("TasksCreated")
                        .HasForeignKey("AccountCreateId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AccountAssigned");

                    b.Navigation("AccountCreate");
                });

            modelBuilder.Entity("API.Entity.Account", b =>
                {
                    b.Navigation("Auctions");

                    b.Navigation("DepositAmount");

                    b.Navigation("LogWrote");

                    b.Navigation("MessagesReceived");

                    b.Navigation("MessagesSent");

                    b.Navigation("MoneyTransactionDetails");

                    b.Navigation("MoneyTransactions");

                    b.Navigation("NewsCreated");

                    b.Navigation("OwnedAuctionAccountings");

                    b.Navigation("RealEstate");

                    b.Navigation("TasksAssigned");

                    b.Navigation("TasksCreated");

                    b.Navigation("WonAuctionAccountings");
                });

            modelBuilder.Entity("API.Entity.Auction", b =>
                {
                    b.Navigation("AuctionAccounting")
                        .IsRequired();
                });

            modelBuilder.Entity("API.Entity.MoneyTransaction", b =>
                {
                    b.Navigation("MoneyTransactionDetail")
                        .IsRequired();
                });

            modelBuilder.Entity("API.Entity.RealEstate", b =>
                {
                    b.Navigation("Photos");
                });
#pragma warning restore 612, 618
        }
    }
}
