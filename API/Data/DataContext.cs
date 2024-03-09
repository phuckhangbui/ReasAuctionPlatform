using API.Entity;
using Microsoft.EntityFrameworkCore;
using Task = API.Entity.Task;

namespace API.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Account> Account { get; set; }
    public DbSet<Auction> Auction { get; set; }
    public DbSet<AuctionAccounting> AuctionsAccounting { get; set; }
    public DbSet<DepositAmount> DepositAmount { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<Major> Major { get; set; }
    public DbSet<Message> Message { get; set; }
    public DbSet<MoneyTransaction> MoneyTransaction { get; set; }
    public DbSet<MoneyTransactionType> MoneyTransactionType { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<RealEstate> RealEstate { get; set; }
    public DbSet<RealEstateDetail> RealEstateDetail { get; set; }
    public DbSet<RealEstatePhoto> RealEstatePhoto { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Rule> Rule { get; set; }
    public DbSet<Task> Task { get; set; }
    public DbSet<Type_REAS> type_REAS { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>()
            .HasKey(k => k.AccountId);

        modelBuilder.Entity<Account>()
            .Property(a => a.AccountId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        modelBuilder.Entity<Auction>()
            .HasKey(k => k.AuctionId);

        modelBuilder.Entity<Auction>()
            .Property(a => a.AuctionId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        modelBuilder.Entity<AuctionAccounting>()
            .HasKey(k => k.AuctionAccountingId);

        modelBuilder.Entity<AuctionAccounting>()
            .Property(a => a.AuctionAccountingId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        modelBuilder.Entity<DepositAmount>()
            .HasKey(k => k.DepositId);

        modelBuilder.Entity<DepositAmount>()
            .Property(a => a.DepositId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        modelBuilder.Entity<Log>()
            .HasKey(k => k.LogId);

        modelBuilder.Entity<Log>()
            .Property(a => a.LogId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        modelBuilder.Entity<Major>()
            .HasKey(k => k.MajorId);

        modelBuilder.Entity<Major>()
            .Property(a => a.MajorId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        modelBuilder.Entity<Message>()
            .HasKey(k => k.MessageId);

        modelBuilder.Entity<Message>()
            .Property(a => a.MessageId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        modelBuilder.Entity<MoneyTransaction>()
            .HasKey(k => k.TransactionId);

        modelBuilder.Entity<MoneyTransaction>()
            .Property(a => a.TransactionId)
            .UseIdentityColumn();

        modelBuilder.Entity<MoneyTransactionType>()
            .HasKey(k => k.TypeId);

        modelBuilder.Entity<MoneyTransactionType>()
            .Property(a => a.TypeId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        modelBuilder.Entity<News>()
            .HasKey(k => k.NewsId);

        modelBuilder.Entity<News>()
            .Property(a => a.NewsId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        modelBuilder.Entity<RealEstate>()
            .HasKey(k => k.ReasId);

        modelBuilder.Entity<RealEstate>()
            .Property(a => a.ReasId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        modelBuilder.Entity<Type_REAS>()
            .HasKey(k => k.Type_ReasId);

        modelBuilder.Entity<Type_REAS>()
            .Property(a => a.Type_ReasId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        modelBuilder.Entity<RealEstateDetail>()
            .HasKey(k => k.ReasDetailId);

        modelBuilder.Entity<RealEstateDetail>()
            .Property(a => a.ReasDetailId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        modelBuilder.Entity<RealEstatePhoto>()
            .HasKey(k => k.ReasPhotoId);

        modelBuilder.Entity<RealEstatePhoto>()
            .Property(a => a.ReasPhotoId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        modelBuilder.Entity<Role>()
            .HasKey(k => k.RoleId);

        modelBuilder.Entity<Role>()
            .Property(a => a.RoleId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        modelBuilder.Entity<Rule>()
            .HasKey(k => k.RuleId);

        modelBuilder.Entity<Rule>()
            .Property(a => a.RuleId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        modelBuilder.Entity<Task>()
            .HasKey(k => k.TaskId);

        modelBuilder.Entity<Task>()
            .Property(a => a.TaskId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        //one account can create many tasks
        modelBuilder.Entity<Task>()
            .HasOne(t => t.AccountCreate)
            .WithMany(a => a.TasksCreated)
            .HasForeignKey(t => t.AccountCreateId)
            .OnDelete(DeleteBehavior.Restrict);

        //many task can assign to one account
        modelBuilder.Entity<Task>()
            .HasOne(t => t.AccountAssigned)
            .WithMany(a => a.TasksAssigned)
            .HasForeignKey(t => t.AccountAssignedId)
            .OnDelete(DeleteBehavior.Restrict);

        //one account can write many log
        modelBuilder.Entity<Log>()
            .HasOne(l => l.AccountWriter)
            .WithMany(a => a.LogWrote)
            .HasForeignKey(l => l.AccountWriterId)
            .OnDelete(DeleteBehavior.Restrict);

        //one account has only one major
        modelBuilder.Entity<Account>()
            .HasOne(a => a.Major)
            .WithOne()
            .HasForeignKey<Account>(a => a.MajorId)
            .OnDelete(DeleteBehavior.Restrict);

        //one account has one role
        modelBuilder.Entity<Account>()
            .HasOne(x => x.Role)
            .WithOne()
            .HasForeignKey<Account>(y => y.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MoneyTransaction>()
        .HasOne(mt => mt.AccountSend)
        .WithMany(a => a.MoneyTransactionsSent)
        .HasForeignKey(mt => mt.AccountSendId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MoneyTransaction>()
            .HasOne(mt => mt.AccountReceive)
            .WithMany(a => a.MoneyTransactionsReceived)
            .HasForeignKey(mt => mt.AccountReceiveId)
            .OnDelete(DeleteBehavior.Restrict);

        //one money transaction has one type
        modelBuilder.Entity<MoneyTransaction>()
            .HasOne(mt => mt.Type)
            .WithOne()
            .HasForeignKey<MoneyTransaction>(mtt => mtt.TypeId)
            .OnDelete(DeleteBehavior.Restrict);


        ////one money transaction detail has one deposit
        modelBuilder.Entity<MoneyTransaction>()
            .HasOne(mtt => mtt.DepositAmount)
            .WithOne()
            .HasForeignKey<MoneyTransaction>(mtt => mtt.DepositId)
            .OnDelete(DeleteBehavior.Restrict);

        ////one money transaction has one real estate
        modelBuilder.Entity<MoneyTransaction>()
            .HasOne(mtd => mtd.RealEstate)
            .WithOne()
            .HasForeignKey<MoneyTransaction>(mtd => mtd.ReasId)
            .OnDelete(DeleteBehavior.Restrict);

        //one account can create many auctions
        modelBuilder.Entity<Auction>()
            .HasOne(au => au.AccountCreate)
            .WithMany(ac => ac.Auctions)
            .HasForeignKey(au => au.AccountCreateId)
            .HasConstraintName("FK_Auction_RealEstate");

        //one auction has one real estate
        modelBuilder.Entity<Auction>()
            .HasOne(a => a.RealEstate)
            .WithOne()
            .HasForeignKey<Auction>(re => re.ReasId)
            .OnDelete(DeleteBehavior.Restrict);

        //one account can sent many messages
        modelBuilder.Entity<Message>()
            .HasOne(m => m.AccountSerder)
            .WithMany(a => a.MessagesSent)
            .HasForeignKey(m => m.AccountSerderId)
            .OnDelete(DeleteBehavior.Restrict);

        //one account can receive many messages
        modelBuilder.Entity<Message>()
            .HasOne(m => m.AccountReceiver)
            .WithMany(a => a.MessagesReceived)
            .HasForeignKey(m => m.AccountReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        //one account can create many news 
        modelBuilder.Entity<News>()
            .HasOne(n => n.AccountCreate)
            .WithMany(a => a.NewsCreated)
            .HasForeignKey(n => n.AccountCreateId)
            .OnDelete(DeleteBehavior.Restrict);

        //one account has many deposit amounts
        modelBuilder.Entity<DepositAmount>()
            .HasOne(da => da.AccountSign)
            .WithMany(a => a.DepositAmount)
            .HasForeignKey(da => da.AccountSignId)
            .OnDelete(DeleteBehavior.Restrict);

        //one deposit amount has one rule
        modelBuilder.Entity<DepositAmount>()
            .HasOne(da => da.Rule)
            .WithOne()
            .HasForeignKey<DepositAmount>(da => da.RuleId)
            .OnDelete(DeleteBehavior.Restrict);

        //one deposit amount has one real estate
        modelBuilder.Entity<DepositAmount>()
            .HasOne(da => da.RealEstate)
            .WithOne()
            .HasForeignKey<DepositAmount>(da => da.ReasId)
            .OnDelete(DeleteBehavior.Restrict);

        //one account can has many auction win 
        modelBuilder.Entity<AuctionAccounting>()
            .HasOne(ac => ac.AccountWin)
            .WithMany(a => a.WonAuctionAccountings)
            .HasForeignKey(ac => ac.AccountWinId)
            .OnDelete(DeleteBehavior.Restrict);

        //one account can be owner of auction accounting
        modelBuilder.Entity<AuctionAccounting>()
            .HasOne(ac => ac.AccountOwner)
            .WithMany(a => a.OwnedAuctionAccountings)
            .HasForeignKey(ac => ac.AccountOwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        //one auction accounting has one real estate
        modelBuilder.Entity<AuctionAccounting>()
            .HasOne(ac => ac.RealEstate)
            .WithOne()
            .HasForeignKey<AuctionAccounting>(ac => ac.ReasId)
            .HasConstraintName("FK_AuctionAccounting_RealEstate");

        //one account can create many real estate
        modelBuilder.Entity<RealEstate>()
            .HasOne(re => re.AccountOwner)
            .WithMany(a => a.RealEstate)
            .HasForeignKey(re => re.AccountOwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RealEstate>()
            .HasOne(x => x.Type_REAS)
            .WithOne()
            .HasForeignKey<RealEstate>(y => y.Type_Reas)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RealEstateDetail>()
            .HasOne(d => d.RealEstate)
            .WithOne(e => e.Detail)
            .HasForeignKey<RealEstateDetail>(d => d.ReasId);

        modelBuilder.Entity<RealEstatePhoto>()
            .HasOne(ac => ac.RealEstate)
            .WithMany(a => a.Photos)
            .HasForeignKey(ac => ac.ReasId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}