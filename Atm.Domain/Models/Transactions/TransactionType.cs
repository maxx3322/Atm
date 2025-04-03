// Models/TransactionType.cs
namespace Atm.Domain.Models.Transactions;

/// <summary>
/// Defines the possible types of transactions
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Money added to the account
    /// </summary>
    Deposit,

    /// <summary>
    /// Money removed from the account
    /// </summary>
    Withdrawal
}