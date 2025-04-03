using Atm.Domain.Models.Accounts;
using Atm.Domain.Repositories.Accounts;

namespace Atm.Application.Repositories.Accounts;

/// <summary>
/// In-memory implementation of the account repository
/// </summary>
public class AccountRepository : IAccountRepository
{
    private readonly List<Account> _accounts = new();

    /// <inheritdoc/>
    public void Add(Account account)
    {
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account));
        }

        if (_accounts.Any(a => a.Username.Equals(account.Username, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"Account with username {account.Username} already exists");
        }

        _accounts.Add(account);
    }

    /// <inheritdoc/>
    public Account? GetById(Guid accountId)
    {
        if (accountId == Guid.Empty)
        {
            throw new ArgumentException("Account ID cannot be empty", nameof(accountId));
        }

        return _accounts.FirstOrDefault(a => a.Id == accountId);
    }
    public Account? GetByUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty", nameof(username));
        }

        return _accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc/>
    public void Update(Account account)
    {
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account));
        }

        Account? existingAccount = GetById(account.Id);
        if (existingAccount == null)
        {
            throw new KeyNotFoundException($"Account with ID {account.Id} not found");
        }

        int index = _accounts.IndexOf(existingAccount);
        _accounts[index] = account;
    }
}