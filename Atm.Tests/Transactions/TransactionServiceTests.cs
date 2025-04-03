using Atm.Domain.Models.Accounts;
using Atm.Domain.Models.Transactions;
using Atm.Domain.Repositories.Accounts;
using Atm.Domain.Repositories.Transactions;
using Atm.Application.Services.Transactions;
using Moq;

namespace TestProject1;

[TestClass]
public class TransactionServiceTests
{
    private Mock<IAccountRepository> _accountRepositoryMock = null!;
    private Mock<ITransactionRepository> _transactionRepositoryMock = null!;
    private TransactionService _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _sut = new TransactionService(_accountRepositoryMock.Object, _transactionRepositoryMock.Object);
    }

    [TestClass]
    public class DepositTests : TransactionServiceTests
    {
        [TestMethod]
        public void Deposit_WithValidAmount_UpdatesBalance()
        {
            // Arrange
            var account = new Account("test123") { Balance = 100m };
            _accountRepositoryMock.Setup(x => x.GetById(account.Id)).Returns(account);

            // Act
            _sut.Deposit(account.Id, 50m);

            // Assert
            Assert.AreEqual(150m, account.Balance);
            _accountRepositoryMock.Verify(x => x.Update(account), Times.Once);
            _transactionRepositoryMock.Verify(x => x.Add(It.Is<Transaction>(t => 
                t.AccountId == account.Id && 
                t.Amount == 50m && 
                t.Type == TransactionType.Deposit)), 
                Times.Once);
        }

        [TestMethod]
        public void Deposit_WithNonExistentAccount_ThrowsException()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            _accountRepositoryMock.Setup(x => x.GetById(accountId)).Returns((Account?)null);

            // Act & Assert
            Assert.ThrowsException<KeyNotFoundException>(() => _sut.Deposit(accountId, 50m));
        }

        [TestMethod]
        public void Deposit_WithNegativeAmount_ThrowsException()
        {
            // Arrange
            var accountId = Guid.NewGuid();

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => _sut.Deposit(accountId, -50m));
        }
    }

    [TestClass]
    public class WithdrawTests : TransactionServiceTests
    {
        [TestMethod]
        public void Withdraw_WithSufficientFunds_UpdatesBalance()
        {
            // Arrange
            var account = new Account("test123") { Balance = 100m };
            _accountRepositoryMock.Setup(x => x.GetById(account.Id)).Returns(account);

            // Act
            _sut.Withdraw(account.Id, 50m);

            // Assert
            Assert.AreEqual(50m, account.Balance);
            _accountRepositoryMock.Verify(x => x.Update(account), Times.Once);
            _transactionRepositoryMock.Verify(x => x.Add(It.Is<Transaction>(t => 
                t.AccountId == account.Id && 
                t.Amount == 50m && 
                t.Type == TransactionType.Withdrawal)), 
                Times.Once);
        }

        [TestMethod]
        public void Withdraw_WithInsufficientFunds_ThrowsException()
        {
            // Arrange
            var account = new Account("test123") { Balance = 40m };
            _accountRepositoryMock.Setup(x => x.GetById(account.Id)).Returns(account);

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => _sut.Withdraw(account.Id, 50m));
        }
    }

    [TestClass]
    public class TransferTests : TransactionServiceTests
    {
        [TestMethod]
        public void Transfer_WithValidAccounts_UpdatesBalances()
        {
            // Arrange
            var fromAccount = new Account("from123") { Balance = 100m };
            var toAccount = new Account("to123") { Balance = 50m };

            _accountRepositoryMock.Setup(x => x.GetById(fromAccount.Id)).Returns(fromAccount);
            _accountRepositoryMock.Setup(x => x.GetById(toAccount.Id)).Returns(toAccount);

            // Act
            _sut.Transfer(fromAccount.Id, toAccount.Id, 50m);

            // Assert
            Assert.AreEqual(50m, fromAccount.Balance);
            Assert.AreEqual(100m, toAccount.Balance);
            _accountRepositoryMock.Verify(x => x.Update(fromAccount), Times.Once);
            _accountRepositoryMock.Verify(x => x.Update(toAccount), Times.Once);
            _transactionRepositoryMock.Verify(x => x.Add(It.Is<Transaction>(t => 
                t.AccountId == fromAccount.Id && 
                t.Amount == 50m && 
                t.Type == TransactionType.Transfer)), 
                Times.Once);
            _transactionRepositoryMock.Verify(x => x.Add(It.Is<Transaction>(t => 
                t.AccountId == toAccount.Id && 
                t.Amount == 50m && 
                t.Type == TransactionType.Transfer)), 
                Times.Once);
        }

        [TestMethod]
        public void Transfer_WithInsufficientFunds_ThrowsException()
        {
            // Arrange
            var fromAccount = new Account("from123") { Balance = 40m };
            var toAccount = new Account("to123") { Balance = 50m };

            _accountRepositoryMock.Setup(x => x.GetById(fromAccount.Id)).Returns(fromAccount);
            _accountRepositoryMock.Setup(x => x.GetById(toAccount.Id)).Returns(toAccount);

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() =>
                _sut.Transfer(fromAccount.Id, toAccount.Id, 50m));
        }
    }
}