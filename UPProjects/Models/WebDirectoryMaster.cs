using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace UPProjects.Models
{
    public class WebDirectoryMaster
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="* Please enter name.")]
        [DisplayName("Name (in English)")]
        public string Name { get; set; }

        [DisplayName("Position (in English)")]
        public string Position { get; set; }
        [DisplayName("Name (in Hindi)")]
        public string HNName { get; set; }

        [DisplayName("Position (in Hindi)")]
        public string HNPosition { get; set; }

        [DisplayName("Address (in English)")]
        public string Address { get; set; }

        [DisplayName("Address (in Hindi)")]
        public string HNAddress { get; set; }
        [DisplayName("Telephone No.")]
        [StringLength(15, ErrorMessage = "* Please enter telephone no.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9-]+$", ErrorMessage = "Please enter correct telephone no.")]
        public string TelephoneNo { get; set; }
        [DisplayName("CUG No.")]
        [StringLength(10, ErrorMessage = " ", MinimumLength = 1)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "* Please enter correct cug no.")]
        public string CugNo { get; set; }
       
        [DisplayName("Mobile No.")]
        [StringLength(10, ErrorMessage = " ", MinimumLength = 1)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "* Please enter correct mobile no.")]
        public string MobileNo { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "* Please enter email id.")]
        [EmailAddress(ErrorMessage = "* Please enter a valid Email")]
        public string EmailId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "* Please select category")]
        [DisplayName("Category")]
        public string CategoryId { get; set; }
        public int ZoneId { get; set; }

        public string CategoryName { get; set; }

        public string ZoneName { get; set; }

        public string ZoneNameHN { get; set; }

        public string HNCategoryName { get; set; }
    }
}
