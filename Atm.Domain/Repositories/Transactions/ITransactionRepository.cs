using Atm.Domain.Models.Transactions;

namespace Atm.Domain.Repositories.Transactions;

/// <summary>
/// Repository for managing transaction persistence
/// </summary>
public interface ITransactionRepository
{
    /// <summary>
    /// Adds a new transaction to the repository
    /// </summary>
    /// <param name="transaction">The transaction to add</param>
    /// <exception cref="ArgumentNullException">Thrown when transaction is null</exception>
    void Add(Transaction transaction);

    /// <summary>
    /// Retrieves all transactions for a specific account
    /// </summary>
    /// <param name="accountId">The account ID to find transactions for</param>
    /// <returns>A collection of transactions ordered by transaction time descending</returns>
    /// <exception cref="ArgumentException">Thrown when accountId is empty</exception>
    IEnumerable<Transaction> GetByAccountId(Guid accountId);
}