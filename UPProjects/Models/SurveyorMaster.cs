using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class SurveyorMaster
    {

        public string Id { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "* Please select District")]
        public string DistictId { get; set; }

        [Required(ErrorMessage = "Please enter Surveyor Name.")]
        [StringLength(100, ErrorMessage = "Please enter Surveyor Name.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        [DataType(DataType.Text)]
        public string SurveyorName { get; set; }

        [Required(ErrorMessage = "Please enter Mobile Number.")]
        [StringLength(10, ErrorMessage = "Please enter valid Mobile Number.", MinimumLength = 10)]
        [RegularExpression(@"^[0-9-]+$", ErrorMessage = "Please enter valid Mobile Number.")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Please enter valid e-Mail ID.")]
        [StringLength(50, ErrorMessage = "Please enter valid e-Mail ID.", MinimumLength = 1)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter valid e-Mail ID.")]
        [EmailAddress(ErrorMessage = "Please enter valid e-Mail ID.")]
        public string EmailId { get; set; }
      
        public SelectList DistrictList { get; set; }

    }
}
