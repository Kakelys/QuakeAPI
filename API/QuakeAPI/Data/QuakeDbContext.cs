using Microsoft.EntityFrameworkCore;
using QuakeAPI.Data.Models;

namespace QuakeAPI.Data
{
    public class QuakeDbContext : DbContext
    {
        public virtual DbSet<Session> Sessions { get;set; } = null!;
        public virtual DbSet<Location> Locations { get;set; } = null!;
        public virtual DbSet<Account> Accounts { get;set; } = null!;
        public virtual DbSet<ActiveAccount> ActiveAccounts { get;set; } = null!;
        public virtual DbSet<Token> Tokens { get;set; } = null!;

        public QuakeDbContext(DbContextOptions<QuakeDbContext> options) : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>(account => 
            {
                account.HasKey(a => a.Id);
                account.HasIndex(a => a.Email).IsUnique();

                account.Property(a => a.Id)
                    .ValueGeneratedOnAdd();
                account.Property(a => a.Email)
                    .IsRequired().
                    HasColumnType("varchar(255)");
                account.Property(a => a.PasswordHash)
                    .IsRequired()
                    .HasColumnType("varchar(max)");
                account.Property(a => a.Role)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValue(Role.User);
                account.Property(a => a.Username)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasDefaultValue("noname");

                account.HasOne(a => a.ActiveAccount)
                    .WithOne(aa => aa.Account)
                    .HasForeignKey<ActiveAccount>(aa => aa.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ActiveAccount>(active => 
            {
                active.HasKey(aa => aa.Id);

                active.Property(aa => aa.Id)
                    .ValueGeneratedOnAdd();

                active.Property(aa => aa.Connected)
                    .IsRequired();

                active.Property(aa => aa.Disconnected)
                    .HasDefaultValue(null);

                active.HasOne(aa => aa.Account)
                    .WithOne(a => a.ActiveAccount)
                    .HasForeignKey<ActiveAccount>(aa => aa.AccountId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ActiveAccount_AccountId");

                active.HasOne(aa => aa.Session)
                    .WithMany(s => s.ActiveAccounts)
                    .HasForeignKey(aa => aa.SessionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ActiveAccount_SessionId");
            });

            builder.Entity<Session>(session => 
            {
                session.HasKey(s => s.Id);

                session.Property(s => s.Id).ValueGeneratedOnAdd();
                session.Property(s => s.Name)
                    .IsRequired()
                    .HasColumnType("nvarchar(255)");

                session.Property(s => s.Created)
                    .IsRequired();

                session.Property(s => s.Deleted)
                    .HasDefaultValue(null);

                session.HasOne(s => s.Location)
                    .WithMany(l => l.Sessions)
                    .HasForeignKey(s => s.LocationId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Session_LocationId");

                session.HasMany(s => s.ActiveAccounts)
                    .WithOne(aa => aa.Session)
                    .HasForeignKey(aa => aa.SessionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Location>(location => 
            {
                location.HasKey(l => l.Id);

                location.HasIndex(l => l.Name).IsUnique();
                
                location.Property(l => l.Id).ValueGeneratedOnAdd();
                location.Property(l => l.Name)
                    .IsRequired()
                    .HasColumnType("varchar(255)");
                location.Property(l => l.Description)
                    .IsRequired()
                    .HasColumnType("nvarchar(3000)");
                location.Property(l => l.LocationPath)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                location.HasMany(l => l.Sessions)
                    .WithOne(s => s.Location)
                    .HasForeignKey(s => s.LocationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Token>(t => 
            {
                t.HasKey(t => t.Id);

                t.HasIndex(t => t.RefreshToken).IsUnique();

                t.Property(t => t.Id)
                    .ValueGeneratedOnAdd();
                t.Property(t => t.RefreshToken)
                    .IsRequired()
                    .HasColumnType("nvarchar(1000)");

                t.HasOne(t => t.Account).WithMany(a => a.Tokens)
                    .HasForeignKey(t => t.AccountId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Token_AccountId");
            });

           base.OnModelCreating(builder);
        }
    }
}