using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Please enter password.")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#^&])[A-Za-z\d@$!%*#^&]{8,}$", ErrorMessage = "Please follow password policy.")]
        [StringLength(15, ErrorMessage = "Password must have a minimum of 8 characters and a maximum of 15 characters.", MinimumLength = 8)]

        public string NewPassword { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage =
            "The new password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
