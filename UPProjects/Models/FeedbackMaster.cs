using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class FeedbackMaster
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="* Please enter name.")]
        [StringLength(100, ErrorMessage = "chaarecter should be less than or equal to 100.", MinimumLength = 0)]
        public string Name { get; set; }
        [Required(ErrorMessage = "* Please enter mail id.")]
        [StringLength(100, ErrorMessage = "Please enter email.", MinimumLength = 0)]
        //[RegularExpression(@"^([A-Za-z0-9][^'!&\\#*$%^?<>()+=:;`~\[\]{}|/,₹€@ ][a-zA-z0-9-._][^!&\\#*$%^?<>()+=:;`~\[\]{}|/,₹€@ ]*\@[a-zA-Z0-9][^!&@\\#*$%^?<> ()+=':;~`.\[\]{}|/,₹€ ]*\.[a-zA-Z]{2,6})$", ErrorMessage = "Please enter a valid Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid Email")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "* Please enter mobile no.")]
        [StringLength(10, ErrorMessage = " ", MinimumLength = 1)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "* Please enter correct mobile no.")]
        [DataType(DataType.Text)]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "* Please enter address.")]

        [StringLength(200, ErrorMessage = " ", MinimumLength = 0)]
        public string Address { get; set; }
        [Required(ErrorMessage = "* Please enter feedback query.")]
        [StringLength(300, ErrorMessage = ".", MinimumLength = 0)]
        public string FeedbackQuery { get; set; }
        public DateTime QueryAskOn { get; set; }
        public string UserIP { get; set; }
    }
}
