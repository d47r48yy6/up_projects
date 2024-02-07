using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class ContractorNew
    {
        public string Id { get; set; }


        [Required(ErrorMessage = "Please enter the name of the contractor/name of the firm.")]
        [StringLength(100, ErrorMessage = "Please enter the name of the contractor/name of the firm.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Name of the Contractor/Name of the Firm")]
        public string FirmName { get; set; }

        [Required(ErrorMessage = "Please fill the name and designation of the applicant.")]
        [StringLength(100, ErrorMessage = "Please fill the name and designation of the applicant.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Name and designation of the applicant.")]
        public string ApplicantName { get; set; }

        [Required(ErrorMessage = "Select applicant's photo.")]
        // [FileSizeValidation(1 * 1024 * 1024)]
        // [FileSizeValidation( ErrorMessage= "Size of File must be less than 1024KB.")]
        public IFormFile ApplicantPhoto { get; set; }

        [Required(ErrorMessage = "Please enter office address.")]
        [StringLength(500, ErrorMessage = "Please enter office address.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        // [Display(Name = "आवेदक का नाम एवं पद")]
        public string OfficeAddress { get; set; }
        [Required(ErrorMessage = "Please enter residence address.")]
        [StringLength(500, ErrorMessage = "Please enter residence address.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string ResidenceAddress { get; set; }
        [Required(ErrorMessage = "Please enter office phone no.")]
        [StringLength(15, ErrorMessage = "Please enter office phone no.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        // [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "कृपया कार्यालय का सही फोन नं0 भरें")]
        [RegularExpression(@"^[0-9-]+$", ErrorMessage = "Please enter correct office phone number.")]
        public string OfficePhone { get; set; }
        [StringLength(15, ErrorMessage = "Please enter residence phone number.", MinimumLength = 0)]
        [RegularExpression(@"^[0-9-]+$", ErrorMessage = "Please enter correct residence phone number.")]
        public string ResidencePhone { get; set; }
        [Required(ErrorMessage = "Please enter mobile no.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please enter correct mobile number.")]
        [StringLength(10, ErrorMessage = "Please enter mobile no.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string Mobile { get; set; }
        [StringLength(15, ErrorMessage = "Please fill the FAX.", MinimumLength = 0)]
        public string Fax { get; set; }
        [Required(ErrorMessage = "Please fill in email.")]
        [StringLength(50, ErrorMessage = "Please fill in email.", MinimumLength = 1)]
        [EmailAddress(ErrorMessage = "Please fill in the correct email.")]
        public string eMail { get; set; }
        [Required(ErrorMessage = "Please fill the name/branch of the bank in the registration fee.")]
        [StringLength(100, ErrorMessage = "Please fill the name/branch of the bank in the registration fee.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string RegFeeBank { get; set; }
        [Required(ErrorMessage = "Please fill the amount in the registration fee.")]
        [StringLength(7, ErrorMessage = "Please fill the amount in the registration fee.", MinimumLength = 1)]
        // [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public string RegFee { get; set; }
        [Required(ErrorMessage = "Please enter the demand draft number in the registration fee.")]
        [StringLength(50, ErrorMessage = "Please enter the demand draft number in the registration fee.", MinimumLength = 1)]
        public string RegFeeDemandNumber { get; set; }
        [Required(ErrorMessage = "Please fill the demand draft date in the registration fee.")]
        [StringLength(10, ErrorMessage = "Please fill the demand draft date in the registration fee.", MinimumLength = 1)]
        public string RegFeeDemandDate { get; set; }
        [Required(ErrorMessage = "Please enter Bank Name/Branch in General Security.")]
        [StringLength(100, ErrorMessage = "Please enter Bank Name/Branch in General Security.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string SecurityBank { get; set; }
        [Required(ErrorMessage = "Please fill the amount in General Security.")]
        [StringLength(7, ErrorMessage = "Please fill the amount in General Security.", MinimumLength = 1)]
        // [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public string SecurityAmount { get; set; }
        [Required(ErrorMessage = "Please enter FDR number in General Security.")]
        [StringLength(50, ErrorMessage = "Please enter FDR number in General Security.", MinimumLength = 1)]
        public string SecurityFDRNuber { get; set; }
        [Required(ErrorMessage = "Please enter FDR date in General Security.")]
        [StringLength(10, ErrorMessage = "Please enter FDR date in General Security.", MinimumLength = 1)]
        public string SecurityFDRDate { get; set; }
        [Required(ErrorMessage = "Please enter maturity date in General Security.")]
        [StringLength(10, ErrorMessage = "Please enter maturity date in General Security.", MinimumLength = 1)]
        public string SecurityMatureDate { get; set; }
        [Required(ErrorMessage = "Please fill the form details.")]
        [StringLength(500, ErrorMessage = "Please fill the form details.", MinimumLength = 1)]
        public string FirmDetail { get; set; }
        public string Category { get; set; }
        public DateTime CreatedOn { get; set; }
        public IFormFile ToolsPlantsFile { get; set; }
        public IFormFile ListofStaffFile { get; set; }
        public IFormFile BlackListFile { get; set; }
        public IFormFile FourYearMainWorksFile { get; set; }
        public IFormFile BalanceSheetFile { get; set; }
        public IFormFile IncomeTaxReturnFile { get; set; }
        [Required(ErrorMessage = "Please fill PAN.")]
        [StringLength(10, ErrorMessage = "Please fill PAN.", MinimumLength = 1)]
        [RegularExpression(@"[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}", ErrorMessage = "Please fill correct PAN.")]
        public string PAN { get; set; }
        [Required(ErrorMessage = "Upload PAN.")]
        //[FileSizeValidation(1 * 1024 * 1024)]
        public IFormFile PANFile { get; set; }
        [Required(ErrorMessage = "Please fill GST.")]
        [StringLength(16, ErrorMessage = "Please fill GST.", MinimumLength = 1)]
        public string GST { get; set; }
        [Required(ErrorMessage = "Upload GST.")]
        // [FileSizeValidation(1 * 1024 * 1024)]
        public IFormFile GSTFile { get; set; }
        [Required(ErrorMessage = "Upload Status Certificate")]
        //   [FileSizeValidation(1 * 1024 * 1024)]
        public IFormFile HasiyatCertificate { get; set; }
        public IFormFile BloodRelation { get; set; }
    }
}
