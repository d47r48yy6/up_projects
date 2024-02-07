using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class UserProfileEdit
    {
        [Required]
        [Display(Name = "UserName")]
        public string Name { get; set; }

        [Display(Name = "UserId")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email Id.")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        public string Mobile { get; set; }
    }
}
