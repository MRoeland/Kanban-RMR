using System.ComponentModel.DataAnnotations;

namespace Kanban_RMR.Models
{
    public class LoginViewModel
    {
        [Required]
        //[EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
