using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ParperaEngineeringTest.Models
{
    public class TransactionContext : DbContext
    {
        public TransactionContext(DbContextOptions<TransactionContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"Data Source = Transactions.db;");
            }
        }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public DbSet<Transaction> Transaction { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // creating the data as per the test requirements
            modelBuilder.Entity<Transaction>().HasData(
                new Transaction() { Id = 11, Datetime = Convert.ToDateTime("2020-09-08T16:34:23Z"), Descripton = "Bank Deposit", Amount = 500.00, Status = "Completed"},
                new Transaction() { Id = 10, Datetime = Convert.ToDateTime("2020-09-08T09:02:23Z"), Descripton = "Transfer to James", Amount = -20.00, Status = "Pending" },
                new Transaction() { Id = 9, Datetime = Convert.ToDateTime("2020-09-07T21:52:23Z"), Descripton = "ATM withdrawal", Amount = -20.00, Status = "Completed" },
                new Transaction() { Id = 8, Datetime = Convert.ToDateTime("2020-09-06T10:32:23Z"), Descripton = "Google Subscription", Amount = -15.00, Status = "Completed" },
                new Transaction() { Id = 7, Datetime = Convert.ToDateTime("2020-09-06T07:33:23Z"), Descripton = "Apple Store", Amount = -2000.00, Status = "Completed" },
                new Transaction() { Id = 6, Datetime = Convert.ToDateTime("2020-08-23T17:39:23Z"), Descripton = "Mini Mart", Amount = -23.76, Status = "Cancelled" },
                new Transaction() { Id = 5, Datetime = Convert.ToDateTime("2020-08-16T21:06:23Z"), Descripton = "Mini Mart", Amount = -56.43, Status = "Completed" },
                new Transaction() { Id = 4, Datetime = Convert.ToDateTime("2020-08-16T21:06:23Z"), Descripton = "Country Railways", Amount = -167.78, Status = "Completed" },
                new Transaction() { Id = 3, Datetime = Convert.ToDateTime("2020-08-16T21:06:23Z"), Descripton = "Refund", Amount = 30.00, Status = "Completed" },
                new Transaction() { Id = 2, Datetime = Convert.ToDateTime("2020-08-16T21:06:23Z"), Descripton = "Amazon Online", Amount = -30.00, Status = "Completed" },
                new Transaction() { Id = 1, Datetime = Convert.ToDateTime("2020-08-16T21:06:23Z"), Descripton = "Bank Deposit", Amount = 500.00, Status = "Completed" }
            );
        }
    }
}
