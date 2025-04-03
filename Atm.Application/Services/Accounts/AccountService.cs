using Atm.Domain.Models.Accounts;
using Atm.Domain.Models.Transactions;
using Atm.Domain.Repositories.Accounts;
using Atm.Domain.Repositories.Transactions;

namespace Atm.Application.Services.Accounts;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public AccountService(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
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

            Transaction transaction = new(accountId, amount, TransactionType.Deposit);
            _transactionRepository.Add(transaction);

            account.Balance += transaction.Amount;
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

            Transaction transaction = new(accountId, amount, TransactionType.Withdrawal);
            _transactionRepository.Add(transaction);

            account.Balance -= transaction.Amount;
            _accountRepository.Update(account);
        }
        catch (Exception ex) when (ex is not ArgumentException && ex is not KeyNotFoundException && ex is not InvalidOperationException)
        {
            throw new InvalidOperationException("Failed to process withdrawal", ex);
        }
    }
}