using Atm.Domain.Models;
using Atm.Domain.Models.Accounts;

namespace Atm.Domain.Repositories.Accounts;

/// <summary>
/// Repository for managing account persistence
/// </summary>
public interface IAccountRepository
{
    /// <summary>
    /// Adds a new account to the repository
    /// </summary>
    /// <param name="account">The account to add</param>
    /// <exception cref="ArgumentNullException">Thrown when account is null</exception>
    void Add(Account account);

    /// <summary>
    /// Gets an account by its ID
    /// </summary>
    /// <param name="accountId">The account ID to find</param>
    /// <returns>The account if found, null otherwise</returns>
    /// <exception cref="ArgumentException">Thrown when accountId is empty</exception>
    Account? GetById(Guid accountId);
    
    /// <summary>
    /// Gets an account by its username
    /// </summary>
    /// <param name="username">The username to find</param>
    /// <returns>The account if found, null otherwise</returns>
    /// <exception cref="ArgumentException">Thrown when username is empty</exception>
    Account? GetByUsername(string username);

    /// <summary>
    /// Updates an existing account
    /// </summary>
    /// <param name="account">The account to update</param>
    /// <exception cref="ArgumentNullException">Thrown when account is null</exception>
    /// <exception cref="KeyNotFoundException">Thrown when account doesn't exist</exception>
    void Update(Account account);
}