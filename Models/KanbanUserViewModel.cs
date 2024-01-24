using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Kanban_RMR.Models
{
    public class CreateKanbanUserViewModel
    {
        [Required]
        [Display(Name = "UserName")]
        [Remote(action: "VerifyUserName", controller: "Account")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [Remote(action: "VerifyEmail", controller: "Account")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The confirmation password differs from the password.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Roles")]
        public List<string> Roles { get; set; } = new List<string>();
    }

    public class EditKanbanUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "EmailConfirmed")]
        public bool EmailConfirmed { get; set; }

        [Required]
        [Display(Name = "Deleted")]
        public bool deleted { get; set; }

        [Required]
        [Display(Name = "Roles")]
        public List<string> Roles { get; set; } = new List<string>();
    }

    public class DeleteKanbanUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string EmailAddress { get; set; }
    }
}

