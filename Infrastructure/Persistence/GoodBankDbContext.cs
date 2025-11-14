using Domine.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public sealed class GoodBankDbContext : DbContext
    {
        public GoodBankDbContext(DbContextOptions<GoodBankDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Transfer> Transfers => Set<Transfer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GoodBankDbContext).Assembly);
        }

        public override int SaveChanges()
        {
            Console.WriteLine($"[DB PATH] {Database.GetDbConnection().DataSource}");
            return base.SaveChanges();
        }
    }
}
