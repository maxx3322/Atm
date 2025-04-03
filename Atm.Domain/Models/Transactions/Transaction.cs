namespace Atm.Domain.Models.Transactions;

public class Transaction
{
    
    public Transaction(Guid accountId, decimal amount, TransactionType type)
    {
        Id = Guid.NewGuid();
        AccountId = accountId;
        Amount = amount;
        Type = type;
        TimeOfTransaction = DateTime.Now;
    }
    
    /// <summary>
    /// Unique identifier for the transaction
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Reference to the account involved in the transaction
    /// </summary>
    public Guid AccountId { get; init; }

    /// <summary>
    /// The monetary value of the transaction
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    /// When the transaction occurred (in UTC)
    /// </summary>
    public DateTime TimeOfTransaction { get; init; }

    /// <summary>
    /// The type of transaction (Deposit or Withdrawal)
    /// </summary>
    public TransactionType Type { get; init; }
    
    /// <summary>
    /// Balance after the transaction;
    /// </summary>
    public decimal BalanceAfter { get; init; }
    
    /// <summary>
    /// The Id of the account that received the transfer.
    /// </summary>
    public Guid? ReceiverAccountId { get; init; }
    
    /// <summary>
    /// Receiving account username.
    /// </summary>
    public string ReceiverAccountUsername { get; init; }
    
}