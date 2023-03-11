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
        });
    }
}