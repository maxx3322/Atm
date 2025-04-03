using Atm.Web.Models;

namespace Atm.Web.Services.Accounts;

public interface IAccountService
{
    Task<(bool isSuccess, string? errorMessage, Guid? createdAccountId)> CreateAccountAsync(AccountCreateModel model);
}