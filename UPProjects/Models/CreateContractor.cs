using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UPProjects.Models
{
    public class CreateContractor
    {
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Please select Zone.")]
        public string ZoneId { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Please select Unit.")]
        public string UnitId { get; set; }

        [Required(ErrorMessage = "Please Enter Firm Name")]
        [Display(Name = "Firm Name")]
        public string FirmName { get; set; }


        [Required(ErrorMessage = "Please Enter Name of Contractor")]
        [Display(Name = "Name of Contractor")]
        public string ApplicantName { get; set; }

        //public IFormFile ApplicantPhoto { get; set; }

        [Display(Name = "PAN No.")]
        [Required(ErrorMessage = "Please enter Pan Number.")]
        [StringLength(10, ErrorMessage = "Please enter valid Pan Number", MinimumLength = 1)]
        [RegularExpression(@"[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}", ErrorMessage = "Please enter valid Pan Number")]
        public string PAN { get; set; }

        [Required(ErrorMessage = "Please enter GST No.")]
        [Display(Name = "GST No.")]
        public string GST { get; set; }
        public IFormFile PANFile { get; set; }

        public IFormFile GSTFile { get; set; }

        [Required(ErrorMessage = "Please Enter Email Id.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$", ErrorMessage = "Please Enter Valid Email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile No.")]
        [Display(Name = "Mobile No.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid Mobile No.")]

        public string Mobile { get; set; }
        [Required(ErrorMessage = "Please enter Office Address.")]

        [Display(Name = "Office Address")]
        public string OfficeAddress { get; set; }

        [Display(Name = "District")]
        public List<string> Districts { get; set; }

        public SelectList ZoneList { get; set; }

    }

    public class District
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }
}
