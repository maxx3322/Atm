using Atm.Domain.Models.Accounts;
using Atm.Domain.Repositories.Accounts;
using Atm.Web.Models;

namespace Atm.Web.Services.Accounts
{
    public class AccountService: IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<(bool isSuccess, string? errorMessage, Guid? createdAccountId)> CreateAccountAsync(AccountCreateModel model)
        {
            if (model.InitialBalance > 10000)
            {
                return (false, "For balances over $10,000, please contact a bank manager.", null);
            }

            var existingAccount = _accountRepository.GetByUsername(model.AccountNumber);
            if (existingAccount != null)
            {
                return (false, "An account with this username already exists.", null);
            }

            Account account = new(model.AccountNumber, model.InitialBalance);
            _accountRepository.Add(account);

            return (true, null, account.Id);
        }
    }
}