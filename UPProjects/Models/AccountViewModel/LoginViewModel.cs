using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models.AccountViewModel
{
    public class LoginViewModel
    {
        [Required (ErrorMessage ="Please enter valid User Id.") ]
        [Display(Name ="User Id")]
        //[EmailAddress]
        public string Email { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [Required(ErrorMessage = "Please enter valid password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
