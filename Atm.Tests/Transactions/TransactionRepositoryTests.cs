using Atm.Application.Repositories.Transactions;
using Atm.Domain.Models.Accounts;
using Atm.Domain.Models.Transactions;
using Atm.Domain.Repositories.Accounts;
using Moq;

namespace TestProject1.Transactions;

[TestClass]
public class TransactionRepositoryTests
{
    private Mock<IAccountRepository> _accountRepositoryMock = null!;
    private TransactionRepository _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _sut = new TransactionRepository(_accountRepositoryMock.Object);
    }

    [TestMethod]
    public void Constructor_WithNullAccountRepository_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() => new TransactionRepository(null!));
    }

    [TestClass]
    public class AddTests : TransactionRepositoryTests
    {
        [TestMethod]
        public void Add_WithValidTransaction_StoresTransaction()
        {
            // Arrange
            var account = new Account("test123");
            var transaction = new Transaction(account.Id, 100m, TransactionType.Deposit);
            _accountRepositoryMock.Setup(x => x.GetById(account.Id)).Returns(account);

            // Act
            _sut.Add(transaction);

            // Assert
            var transactions = _sut.GetByAccountId(account.Id);
            Assert.AreEqual(1, transactions.Count());
            Assert.AreEqual(transaction, transactions.First());
        }

        [TestMethod]
        public void Add_WithNullTransaction_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => _sut.Add(null!));
        }

        [TestMethod]
        public void Add_WithNonExistentAccount_ThrowsKeyNotFoundException()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var transaction = new Transaction(accountId, 100m, TransactionType.Deposit);
            _accountRepositoryMock.Setup(x => x.GetById(accountId)).Returns((Account?)null);

            // Act & Assert
            Assert.ThrowsException<KeyNotFoundException>(() => _sut.Add(transaction));
        }
    }

    [TestClass]
    public class GetByAccountIdTests : TransactionRepositoryTests
    {
        [TestMethod]
        public void GetByAccountId_WithValidAccount_ReturnsTransactions()
        {
            // Arrange
            var account = new Account("test123");
            var transaction1 = new Transaction(account.Id, 100m, TransactionType.Deposit);
            var transaction2 = new Transaction(account.Id, 50m, TransactionType.Withdrawal);

            _accountRepositoryMock.Setup(x => x.GetById(account.Id)).Returns(account);
            _sut.Add(transaction1);
            _sut.Add(transaction2);

            // Act
            var transactions = _sut.GetByAccountId(account.Id);

            // Assert
            Assert.AreEqual(2, transactions.Count());
            CollectionAssert.AreEqual(
                new[] { transaction2, transaction1 }, 
                transactions.ToList()
            );
        }

        [TestMethod]
        public void GetByAccountId_WithEmptyGuid_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => _sut.GetByAccountId(Guid.Empty));
        }

        [TestMethod]
        public void GetByAccountId_WithNonExistentAccount_ThrowsKeyNotFoundException()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            _accountRepositoryMock.Setup(x => x.GetById(accountId)).Returns((Account?)null);

            // Act & Assert
            Assert.ThrowsException<KeyNotFoundException>(() => _sut.GetByAccountId(accountId));
        }

        [TestMethod]
        public void GetByAccountId_ReturnsTransactionsInDescendingOrder()
        {
            // Arrange
            var account = new Account("test123");
            _accountRepositoryMock.Setup(x => x.GetById(account.Id)).Returns(account);

            var transaction1 = new Transaction(account.Id, 100m, TransactionType.Deposit);
            Thread.Sleep(10); // Ensure different timestamps
            var transaction2 = new Transaction(account.Id, 50m, TransactionType.Withdrawal);

            _sut.Add(transaction1);
            _sut.Add(transaction2);

            // Act
            var transactions = _sut.GetByAccountId(account.Id).ToList();

            // Assert
            Assert.IsTrue(transactions[0].TimeOfTransaction > transactions[1].TimeOfTransaction);
            Assert.AreEqual(transaction2.Id, transactions[0].Id);
            Assert.AreEqual(transaction1.Id, transactions[1].Id);
        }
    }
}