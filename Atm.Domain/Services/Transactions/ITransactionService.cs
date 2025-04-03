using Atm.Domain.Models.Transactions;

namespace Atm.Domain.Services.Transactions;

public interface ITransactionService
{
    /// <summary>
    /// Deposits money into an account
    /// </summary>
    /// <param name="accountId">The account ID</param>
    /// <param name="amount">Amount to deposit</param>
    void Deposit(Guid accountId, decimal amount);

    /// <summary>
    /// Withdraws money from an account
    /// </summary>
    /// <param name="accountId">The account ID</param>
    /// <param name="amount">Amount to withdraw</param>
    void Withdraw(Guid accountId, decimal amount);

    /// <summary>
    /// Transfers money between accounts
    /// </summary>
    /// <param name="fromAccountId">Source account ID</param>
    /// <param name="toAccountId">Destination account ID</param>
    /// <param name="amount">Amount to transfer</param>
    void Transfer(Guid fromAccountId, Guid toAccountId, decimal amount);

    /// <summary>
    /// Gets transaction history for an account
    /// </summary>
    /// <param name="accountId">The account ID</param>
    /// <returns>List of transactions ordered by most recent first</returns>
    IEnumerable<Transaction> GetTransactionHistory(Guid accountId);
}