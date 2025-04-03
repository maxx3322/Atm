namespace Atm.Domain.Models.Accounts;

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
    public string Username { get; init; }

    /// <summary>
    /// Current balance of the account
    /// </summary>
    public decimal Balance { get; set; }
    

    /// <summary>
    /// Creates a new account
    /// </summary>
    /// <param name="username">The account number</param>
    /// <param name="initialBalance">Initial balance, defaults to 0</param>
    public Account(string username,  decimal initialBalance = 0)
    {
        Id = Guid.NewGuid();
        Username = username;
        Balance = initialBalance;
    }
}