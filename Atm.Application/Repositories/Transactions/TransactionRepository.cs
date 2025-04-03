using Atm.Domain.Models.Transactions;
using Atm.Domain.Repositories.Accounts;
using Atm.Domain.Repositories.Transactions;

namespace Atm.Application.Repositories.Transactions;

/// <summary>
/// In-memory implementation of the transaction repository
/// </summary>
public class TransactionRepository : ITransactionRepository
{
    private readonly List<Transaction> _transactions = new();
    private readonly IAccountRepository _accountRepository;

    /// <summary>
    /// Initializes a new instance of the TransactionRepository
    /// </summary>
    /// <param name="accountRepository">The account repository</param>
    /// <exception cref="ArgumentNullException">Thrown when accountRepository is null</exception>
    public TransactionRepository(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
    }

    /// <inheritdoc/>
    public void Add(Transaction transaction)
    {
        if (transaction == null)
        {
            throw new ArgumentNullException(nameof(transaction));
        }

        var account = _accountRepository.GetById(transaction.AccountId);
        if (account == null)
        {
            throw new KeyNotFoundException($"Account with ID {transaction.AccountId} not found");
        }

        _transactions.Add(transaction);
    }

    /// <inheritdoc/>
    public IEnumerable<Transaction> GetByAccountId(Guid accountId)
    {
        if (accountId == Guid.Empty)
        {
            throw new ArgumentException("Account ID cannot be empty", nameof(accountId));
        }

        var account = _accountRepository.GetById(accountId);
        if (account == null)
        {
            throw new KeyNotFoundException($"Account with ID {accountId} not found");
        }

        return _transactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.TimeOfTransaction)
            .ToList();
    }
}