using System.ComponentModel.DataAnnotations;

namespace Atm.Web.Models
{
    public class AccountCreateModel
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(9, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 9 characters")]
        public string AccountNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Initial balance is required")]
        [Range(0, 10000, ErrorMessage = "Initial balance must be between $0 and $10,000. For larger amounts, please contact a bank manager.")]
        public decimal InitialBalance { get; set; }
    }
}