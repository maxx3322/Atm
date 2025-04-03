using Atm.Domain.Models.Accounts;

namespace Atm.Domain.Services.Accounts;

/// <summary>
/// Provides functionality for managing bank account operations
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Validates if the provided PIN matches the account's PIN
    /// </summary>
    /// <param name="account">The account to validate against</param>
    /// <param name="pin">The PIN to validate</param>
    /// <returns>True if the PIN matches, false otherwise</returns>
    bool ValidatePin(Account account, string pin);

    /// <summary>
    /// Deposits money into the specified account
    /// </summary>
    /// <param name="account">The account to deposit into</param>
    /// <param name="amount">The amount to deposit</param>
    /// <returns>A new account instance with the updated balance</returns>
    /// <exception cref="ArgumentException">Thrown when amount is 0 or negative</exception>
    Account Deposit(Account account, decimal amount);

    /// <summary>
    /// Withdraws money from the specified account
    /// </summary>
    /// <param name="account">The account to withdraw from</param>
    /// <param name="amount">The amount to withdraw</param>
    /// <returns>A new account instance with the updated balance</returns>
    /// <exception cref="ArgumentException">Thrown when amount is 0 or negative</exception>
    /// <exception cref="InvalidOperationException">Thrown when insufficient funds</exception>
    Account Withdraw(Account account, decimal amount);
}