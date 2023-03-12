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
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CustomerAccount>(entity =>
        {
            entity.HasKey(x => new { x.CustomerId, x.Currency });
            entity.Property(x => x.Currency).HasMaxLength(10);
            entity.Property(x => x.EmailAddress).HasMaxLength(100);
            entity.Property(x => x.FirstName).HasMaxLength(50);
            entity.Property(x => x.LastName).HasMaxLength(50);
            entity.Property(x => x.Country).HasMaxLength(20);
            entity.Property(x => x.Status).HasMaxLength(20);
            entity.Property(x => x.Balance).HasPrecision(24, 8);
        });
    }
}