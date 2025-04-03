// Models/Account.cs
namespace Atm.Domain.Models;

/// <summary>
/// Represents a bank account in the ATM system
/// </summary>
public class Account
{
    /// <summary>
    /// Unique identifier for the account
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The account number used for identification
    /// </summary>
    public string AccountNumber { get; init; }

    /// <summary>
    /// Current balance of the account
    /// </summary>
    public decimal Balance { get; init; }

    /// <summary>
    /// PIN code for account security
    /// </summary>
    public string Pin { get; init; }

    public Account(string accountNumber, string pin, decimal initialBalance = 0)
    {
        Id = Guid.NewGuid();
        AccountNumber = accountNumber;
        Pin = pin;
        Balance = initialBalance;
    }
}