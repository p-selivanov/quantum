using Microsoft.EntityFrameworkCore;
using Quantum.AccountSearch.Persistence.Models;

namespace Quantum.AccountSearch.Persistence;

public class AccountSearchDbContext : DbContext
{
    public DbSet<CustomerAccount> CustomerAccounts { get; set; }

    public AccountSearchDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("citext");

        modelBuilder.Entity<CustomerAccount>(entity =>
        {
            entity.HasKey(x => new { x.CustomerId, x.Currency });
            entity.Property(x => x.CustomerId).HasMaxLength(32);
            entity.Property(x => x.Currency).HasMaxLength(10);
            entity.Property(x => x.EmailAddress).HasMaxLength(100);
            entity.Property(x => x.FirstName).HasMaxLength(50).HasColumnType("citext");
            entity.Property(x => x.LastName).HasMaxLength(50).HasColumnType("citext");
            entity.Property(x => x.Country).HasMaxLength(20).HasColumnType("citext");
            entity.Property(x => x.Status).HasMaxLength(20).HasColumnType("citext");
            entity.Property(x => x.Balance).HasPrecision(24, 8);

            entity.HasIndex(x => x.EmailAddress);
            entity.HasIndex(x => x.FirstName);
            entity.HasIndex(x => x.LastName);
            entity.HasIndex(x => x.Country);
            entity.HasIndex(x => x.Balance);
            entity.HasIndex(x => x.CustomerCreatedAt);
            entity.HasIndex(x => x.BalanceUpdatedAt);
        });
    }
}