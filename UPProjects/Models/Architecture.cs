using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class Architecture
    {
        [Required(ErrorMessage = "Please enter Category")]
        [StringLength(1, ErrorMessage = "Please enter Category", MinimumLength = 1)]
        [RegularExpression(@"^[abcABC]+$", ErrorMessage = "Enter Valid Category")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Please enter Name of Consultant.")]
        [StringLength(50, ErrorMessage = "Please enter Name of Consultant.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string NameofConsultant { get; set; }


        [Required(ErrorMessage = "Please enter Address.")]
        [StringLength(500, ErrorMessage = "Please enter Address.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string Address { get; set; }
        
        
        [Required(ErrorMessage = "Please enter Name of Person.")]
        [StringLength(500, ErrorMessage = "Please enter Name of Person.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Please select Photo (Signature & Designation)")]
        public IFormFile Photo { get; set; }


        [StringLength(15, ErrorMessage = "Please enter valid Phone Number.", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9-]+$", ErrorMessage ="Please enter valid Phone Number.")]
        public string PhoneNumber { get; set; }



        [Required(ErrorMessage = "Please enter Mobile number of authorize Person.")]
        [StringLength(10, ErrorMessage = "Please enter Mobile number of authorize Person.", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]+$")]
        public string Mobile { get; set; }


        [StringLength(15, ErrorMessage = "Please enter valid Fax Number.", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9-]+$", ErrorMessage = "Please enter valid Fax Number.")]
        public string Fax { get; set; }

        [Required (ErrorMessage = "Please enter valid e-Mail ID.")]
        [StringLength(50, ErrorMessage = "Please enter valid e-Mail ID.", MinimumLength = 1)]
        [DataType(DataType.EmailAddress, ErrorMessage ="Please enter valid e-Mail ID.")]
        [EmailAddress(ErrorMessage ="Please enter valid e-Mail ID.")]
        public string eMail { get; set; }


        [StringLength(50, ErrorMessage = "Please enter valid Website URL.", MinimumLength = 1)]
        [DataType(DataType.Url, ErrorMessage = "Please enter valid Website URL.")]
        [Url(ErrorMessage = "Please enter valid Website URL.")]
        
        public string Website { get; set; }


        [Required(ErrorMessage = "Please enter Demand Draft No.")]
        [StringLength(50, ErrorMessage = "Please enter Demand Draft No.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string DataProcessingDD { get; set; }



        [Required(ErrorMessage = "Please enter Demand Draft Date.")]
        [StringLength(10, ErrorMessage = "Please enter Demand Draft Date.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]{2}[/]{1}[0-9]{2}[/]{1}[0-9]{4}", ErrorMessage = "Please enter valid Demand Draft Date.")]
        public string DataProcessingDDDate { get; set; }


        [Required(ErrorMessage = "Please enter Name of Bank/ Branch.")]
        [StringLength(50, ErrorMessage = "Please enter Name of Bank/ Branch.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string DataProcessingDDBank { get; set; }

        [Required(ErrorMessage = "Please enter Demand Draft No./ FDR/ TDR/ NSC No.")]
        [StringLength(50, ErrorMessage = "Please enter Demand Draft No./ FDR/ TDR/ NSC No.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string SecurityDDNumber { get; set; }



        [Required(ErrorMessage = "Please enter Demand Draft Date.")]
        [StringLength(10, ErrorMessage = "Please enter Demand Draft Date.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]{2}[/]{1}[0-9]{2}[/]{1}[0-9]{4}", ErrorMessage = "Please enter valid Demand Draft Date.")]
        public string SecurityDDDate { get; set; }

        [Required(ErrorMessage = "Please enter Name of Bank/ Branch.")]
        [StringLength(50, ErrorMessage = "Please enter Name of Bank/ Branch.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string SecurityBank { get; set; }



        [Required(ErrorMessage = "Please enter Amount.")]
        [StringLength(7, ErrorMessage = "Please enter Amount.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid Amount.")]
        public string SecurityAmount { get; set; }


        [Required(ErrorMessage = "Please enter Registration Date.")]
        [StringLength(10, ErrorMessage = "Please enter Registration Date.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]{2}[/]{1}[0-9]{2}[/]{1}[0-9]{4}", ErrorMessage = "Please enter valid Registration Date.")]

        public string CouncilRegistrationDate { get; set; }

        [Required(ErrorMessage = "Please enter Date Validity.")]
        [StringLength(10, ErrorMessage = "Please enter Date Validity.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]{2}[/]{1}[0-9]{2}[/]{1}[0-9]{4}", ErrorMessage = "Please enter valid Date Validity.")]
        public string CouncilDateofValidity { get; set; }


       // [Required(ErrorMessage = "Please upload Photo Copy self attested of Registration.")]
        public IFormFile CouncilPhotoCoopy { get; set; }

        [StringLength(7, ErrorMessage = "Please enter Value of projects.", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid Value of projects.")]
        public string AnnexureAThirdYear { get; set; }


       // [Required(ErrorMessage = "Please upload Value of projects.")]
        public IFormFile AnnexureAThirdYearFile { get; set; }


        [StringLength(7, ErrorMessage = "Please enter Value of projects.", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid Value of projects.")]
        public string AnnexureASecondYear { get; set; }


        //[Required(ErrorMessage = "Please upload Value of projects.")]
        public IFormFile AnnexureASecondYearFile { get; set; }



      //  [Required(ErrorMessage = "Please enter Value of projects.")]
        [StringLength(7, ErrorMessage = "Please enter Value of projects.", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid Value of projects.")]
        public string AnnexureAFirstYear { get; set; }


        
       // [Required(ErrorMessage = "Please upload Value of projects.")]
        public IFormFile AnnexureAFirstYearFile { get; set; }


        [StringLength(7, ErrorMessage = "Please enter maximum value of single project .", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid value.")]
        public string AnnexureAThirdYearSingle { get; set; }


       // [Required(ErrorMessage = "Please upload Value of projects.")]
        public IFormFile AnnexureAThirdYearFileSingle { get; set; }



        [StringLength(7, ErrorMessage = "Please enter maximum value of single project .", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid value.")]
        public string AnnexureASecondYearSingle { get; set; }

       // [Required(ErrorMessage = "Please upload Value of projects.")]
        public IFormFile AnnexureASecondYearFileSingle { get; set; }


        [StringLength(7, ErrorMessage = "Please enter maximum value of single project .", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid value.")]
        public string AnnexureAFirstYearSingle { get; set; }

       // [Required(ErrorMessage = "Please upload Value of projects.")]
        public IFormFile AnnexureAFirstYearFileSingle { get; set; }


        [StringLength(7, ErrorMessage = "Please enter Financial Information.", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid Financial Information.")]
        public string AnnexureBThirdYear { get; set; }

       // [Required(ErrorMessage = "Please upload Financial Information.")]
        public IFormFile AnnexureBThirdYearFile { get; set; }


        [StringLength(7, ErrorMessage = "Please enter Financial Information.", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid Financial Information.")]
        public string AnnexureBSecondYear { get; set; }

       // [Required(ErrorMessage = "Please upload Financial Information.")]
        public IFormFile AnnexureBSecondYearFile { get; set; }


        [StringLength(7, ErrorMessage = "Please enter Financial Information.", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter valid Financial Information.")]
        public string AnnexureBFirstYear { get; set; }

       // [Required(ErrorMessage = "Please upload Financial Information.")]
        public IFormFile AnnexureBFirstYearFile { get; set; }


        [Required(ErrorMessage = "Please enter Income Tax Permanent Account Number.")]
        [StringLength(10, ErrorMessage = "Please enter valid Income Tax Permanent Account Number", MinimumLength = 1)]
        [RegularExpression(@"[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}", ErrorMessage = "Please enter valid Income Tax Permanent Account Number")]
        public string Pan { get; set; }


        [Required(ErrorMessage = "Please upload PAN.")]
        public IFormFile PanFile { get; set; }

        [StringLength(50, ErrorMessage = "Please enter valid Service Tax Registration Number", MinimumLength = 0)]
        public string ServiceTax { get; set; }
        public IFormFile ServiceTaxFile { get; set; }

        public IFormFile OtherempanelledFile { get; set; }
        public IFormFile AnnexureCFile { get; set; }

      


    }
}
