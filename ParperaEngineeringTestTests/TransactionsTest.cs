using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParperaEngineeringTest.Controllers;
using ParperaEngineeringTest.Models;
using ParperaEngineeringTest.Models.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace ParperaEngineeringTestTests
{
    [TestClass]
    public class TransactionsUnitTest : IDisposable
    {
        private const string InMemoryConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;
        protected readonly TransactionContext DbContext;

        public TransactionsUnitTest()
        {
            //use in-memory DB for unit testing
            _connection = new SqliteConnection(InMemoryConnectionString);
            _connection.Open();
            var options = new DbContextOptionsBuilder<TransactionContext>()
                    .UseSqlite(_connection)
                    .Options;
            DbContext = new TransactionContext(options);
            DbContext.Database.EnsureCreated();

            ClearDb();
        }

        private void ClearDb()
        {
            DbContext.Transaction.RemoveRange(DbContext.Transaction);
            DbContext.SaveChanges();
        }

        public void Dispose() => _connection.Dispose();

        public Transaction[] SampleTransactions()
        {
            Transaction[] transactions = {
                new Transaction() { Id = 1, Datetime = Convert.ToDateTime("2020-07-08T09:02:23Z"), Descripton = "Bank Deposit", Amount = 500.00, Status = "Completed" },
                new Transaction() { Id = 2, Datetime = Convert.ToDateTime("2020-09-09T16:34:23Z"), Descripton = "Bank Deposit", Amount = 500.00, Status = "Completed" },
                new Transaction() { Id = 3, Datetime = Convert.ToDateTime("2020-09-08T16:34:00Z"), Descripton = "Transfer to James", Amount = -20.00, Status = "Pending" }
            };
            return transactions;
        }

        [TestMethod]
        public async Task TestBasicGetTransactions()
        {
            DbContext.Transaction.AddRange(SampleTransactions());
            DbContext.SaveChanges();

            var controller = new TransactionsController(DbContext);

            var result = (await controller.GetTransactions()).Value;

            Assert.IsNotNull(result);

            Transaction[] transactions = {
                new Transaction() { Id = 2, Datetime = Convert.ToDateTime("2020-09-09T16:34:23Z"), Descripton = "Bank Deposit", Amount = 500.00, Status = "Completed" },
                new Transaction() { Id = 3, Datetime = Convert.ToDateTime("2020-09-08T16:34:00Z"), Descripton = "Transfer to James", Amount = -20.00, Status = "Pending" },
                new Transaction() { Id = 1, Datetime = Convert.ToDateTime("2020-07-08T09:02:23Z"), Descripton = "Bank Deposit", Amount = 500.00, Status = "Completed" },
            };

            Assert.AreEqual(transactions.Length, result.Count());

            for (int i = 0; i < transactions.Length; i++)
            {
                var expected = transactions[i];
                var actual = result[i];

                Assert.AreEqual(expected.Id, actual.Id, "Ids did not match");
                Assert.AreEqual(expected.Datetime, actual.Datetime, "Datetimes did not match");
                Assert.AreEqual(expected.Descripton, actual.Descripton, "Desciptions did not match");
                Assert.AreEqual(expected.Amount, actual.Amount, "Amounts did not match");
                Assert.AreEqual(expected.Status, actual.Status, "Status did not match");
            }
            ClearDb();
        }

        [TestMethod]
        public async Task TestNoneGetTransactions()
        {

            var controller = new TransactionsController(DbContext);

            var result = (await controller.GetTransactions()).Value;

            Assert.IsNotNull(result);

            Assert.AreEqual(result.Length, 0);

            ClearDb();
        }

        [TestMethod]
        public async Task TestUpdateStatus()
        {
            var toAdd = SampleTransactions()[0];
            DbContext.Transaction.Add(toAdd);
            DbContext.SaveChanges();

            var controller = new TransactionsController(DbContext);

            //change the transaction status from Completed to Pending
            var statusUpdate = new StatusUpdate() { Status = "Pending" };

            var result = (await controller.ModifyStatus(1, statusUpdate)) as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(204, result.StatusCode);

            var transaction = await DbContext.Transaction.FindAsync((long) 1);

            Assert.AreEqual(transaction.Id, toAdd.Id, "Ids did not match");
            Assert.AreEqual(transaction.Datetime, toAdd.Datetime, "Datetimes did not match");
            Assert.AreEqual(transaction.Descripton, toAdd.Descripton, "Desciptions did not match");
            Assert.AreEqual(transaction.Amount, toAdd.Amount, "Amounts did not match");
            Assert.AreEqual(transaction.Status, statusUpdate.Status, "Status was not updated");

            ClearDb();
        }

        [TestMethod]
        public async Task TestNotFoundStatusUpdate()
        {
            var statusUpdate = new StatusUpdate() { Status = "Pending" };

            var controller = new TransactionsController(DbContext);
            var result = (await controller.ModifyStatus(1, statusUpdate)) as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public async Task TestBadRequestStatusUpdate()
        {
            var toAdd = SampleTransactions()[0];
            DbContext.Transaction.Add(toAdd);
            DbContext.SaveChanges();
        
            var controller = new TransactionsController(DbContext);
        
            var statusUpdate = new StatusUpdate() { Status = "hhhhhhh" };
        
            var result = (await controller.ModifyStatus(1, statusUpdate)) as StatusCodeResult;
        
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(400, result.StatusCode);
            
            ClearDb();
        }
    }
}
