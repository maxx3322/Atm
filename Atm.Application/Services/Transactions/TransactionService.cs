using Atm.Domain.Models.Accounts;
using Atm.Domain.Models.Transactions;
using Atm.Domain.Repositories.Accounts;
using Atm.Domain.Repositories.Transactions;
using Atm.Domain.Services.Transactions;

namespace Atm.Application.Services.Transactions;

public class TransactionService : ITransactionService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
    }

    public void Deposit(Guid accountId, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be positive", nameof(amount));
        }

        try
        {
            Account? account = _accountRepository.GetById(accountId);
            if (account == null)
            {
                throw new KeyNotFoundException($"Account with ID {accountId} not found");
            }

            Transaction transaction = new(accountId, amount, TransactionType.Deposit)
            {
                BalanceAfter = account.Balance + amount
            };
            _transactionRepository.Add(transaction);

            account.Balance += amount;
            _accountRepository.Update(account);
        }
        catch (Exception ex) when (ex is not ArgumentException && ex is not KeyNotFoundException)
        {
            throw new InvalidOperationException("Failed to process deposit", ex);
        }
    }

    public void Withdraw(Guid accountId, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be positive", nameof(amount));
        }

        try
        {
            Account? account = _accountRepository.GetById(accountId);
            if (account == null)
            {
                throw new KeyNotFoundException($"Account with ID {accountId} not found");
            }

            if (account.Balance < amount)
            {
                throw new InvalidOperationException("Insufficient funds");
            }

            Transaction transaction = new(accountId, amount, TransactionType.Withdrawal)
            {
                BalanceAfter = account.Balance - amount
            };
            _transactionRepository.Add(transaction);

            account.Balance -= amount;
            _accountRepository.Update(account);
        }
        catch (Exception ex) when (ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
        {
            throw new InvalidOperationException("Failed to process withdrawal", ex);
        }
    }



    public void Transfer(Guid fromAccountId, Guid toAccountId, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be positive", nameof(amount));
        }

        if (fromAccountId == toAccountId)
        {
            throw new ArgumentException("Cannot transfer to the same account");
        }

        try
        {
            Account? fromAccount = _accountRepository.GetById(fromAccountId);
            if (fromAccount == null)
            {
                throw new KeyNotFoundException($"Source account with ID {fromAccountId} not found");
            }

            Account? toAccount = _accountRepository.GetById(toAccountId);
            if (toAccount == null)
            {
                throw new KeyNotFoundException($"Destination account with ID {toAccountId} not found");
            }

            if (fromAccount.Balance < amount)
            {
                throw new InvalidOperationException("Insufficient funds for transfer");
            }

            // Create withdrawal transaction
            Transaction withdrawalTransaction = new(fromAccountId, amount, TransactionType.Transfer)
            {
                BalanceAfter = fromAccount.Balance - amount,
                ReceiverAccountId = toAccountId,
                ReceiverAccountUsername = toAccount.Username,
                
            };
            _transactionRepository.Add(withdrawalTransaction);

            // Create deposit transaction
            Transaction depositTransaction = new(toAccountId, amount, TransactionType.Transfer)
            {
                BalanceAfter = toAccount.Balance + amount,
                ReceiverAccountId = fromAccountId,
                ReceiverAccountUsername = fromAccount.Username,
            };
            _transactionRepository.Add(depositTransaction);

            // Update account balances
            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            _accountRepository.Update(fromAccount);
            _accountRepository.Update(toAccount);
        }
        catch (Exception ex) when (ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
        {
            throw new InvalidOperationException("Failed to process transfer", ex);
        }
    }

    public IEnumerable<Transaction> GetTransactionHistory(Guid accountId)
    {
        try
        {
            return _transactionRepository.GetByAccountId(accountId);
        }
        catch (Exception ex) when (ex is not ArgumentException && ex is not KeyNotFoundException)
        {
            throw new InvalidOperationException("Failed to retrieve transaction history", ex);
        }
    }
}