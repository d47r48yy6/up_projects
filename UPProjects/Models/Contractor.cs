using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class Contractor
    {
        public string Id { get; set; }      
        

        [Required(ErrorMessage = "कृपया ठेकेदार का नाम/फर्म का नाम भरें")]
        [StringLength(100, ErrorMessage = "कृपया ठेकेदार का नाम/फर्म का नाम भरें", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "ठेकेदार का नाम/फर्म का नाम")]
        public string FirmName { get; set; }

        [Required(ErrorMessage = "कृपया आवेदक का नाम एवं पद भरें")]
        [StringLength(100, ErrorMessage = "कृपया आवेदक का नाम एवं पद भरें", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "आवेदक का नाम एवं पद")]
        public string ApplicantName { get; set; }

        [Required(ErrorMessage = "आवेदक का फोटो चुनें")]        
       // [FileSizeValidation(1 * 1024 * 1024)]
       // [FileSizeValidation( ErrorMessage= "Size of File must be less than 1024KB.")]
        public IFormFile ApplicantPhoto { get; set; }

        [Required(ErrorMessage = "कृपया कार्यालय का पता भरें")]
        [StringLength(500, ErrorMessage = "कृपया कार्यालय का पता भरें", MinimumLength = 1)]
        [DataType(DataType.Text)]
       // [Display(Name = "आवेदक का नाम एवं पद")]
        public string OfficeAddress { get; set; }
        [Required(ErrorMessage = "कृपया निवास का पता भरें")]
        [StringLength(500, ErrorMessage = "कृपया निवास का पता भरें", MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string ResidenceAddress { get; set; }
        [Required(ErrorMessage = "कृपया कार्यालय का फोन नं0 भरें")]
        [StringLength(15, ErrorMessage = "कृपया कार्यालय का फोन नं0 भरें", MinimumLength = 1)]
        [DataType(DataType.Text)]
        // [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "कृपया कार्यालय का सही फोन नं0 भरें")]
        [RegularExpression(@"^[0-9-]+$", ErrorMessage = "कृपया कार्यालय का सही फोन नं0 भरें")]
        public string OfficePhone { get; set; }
        [StringLength(15, ErrorMessage = "कृपया निवास का फोन नं0 भरें", MinimumLength = 0)]
        [RegularExpression(@"^[0-9-]+$", ErrorMessage = "कृपया निवास का सही फोन नं0 भरें")]
        public string ResidencePhone { get; set; }
        [Required(ErrorMessage = "कृपया मोबाइल नं0 भरें")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "कृपया सही मोबाइल नं0 भरें")]
        [StringLength(10, ErrorMessage = "कृपया मोबाइल नं0 भरें", MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string Mobile { get; set; }
        [StringLength(15, ErrorMessage = "कृपया FAX भरें", MinimumLength = 0)]
        public string Fax { get; set; }
        [Required(ErrorMessage = "कृपया ई-मेल भरें")]
        [StringLength(50, ErrorMessage = "कृपया ई-मेल भरें", MinimumLength = 1)]
        [EmailAddress(ErrorMessage = "कृपया सही ई-मेल भरें")]
        public string eMail { get; set; }
        [Required(ErrorMessage = "कृपया रजिस्ट्रशन शुल्क में बैंक का नाम/शाखा भरें")]
        [StringLength(100, ErrorMessage = "कृपया रजिस्ट्रशन शुल्क में बैंक का नाम/शाखा भरें", MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string RegFeeBank { get; set; }
        [Required(ErrorMessage = "कृपया रजिस्ट्रशन शुल्क में धनराशि भरें")]
        [StringLength(7, ErrorMessage = "कृपया रजिस्ट्रशन शुल्क में धनराशि भरें", MinimumLength = 1)]       
       // [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public string RegFee { get; set; }
        [Required(ErrorMessage = "कृपया रजिस्ट्रशन शुल्क में डिमाण्ड ड्राफ्ट संख्या भरें")]
        [StringLength(50, ErrorMessage = "कृपया रजिस्ट्रशन शुल्क में डिमाण्ड ड्राफ्ट संख्या भरें", MinimumLength = 1)]      
        public string RegFeeDemandNumber { get; set; }
        [Required(ErrorMessage = "कृपया रजिस्ट्रशन शुल्क में डिमाण्ड ड्राफ्ट दिनांक भरें")]
        [StringLength(10, ErrorMessage = "कृपया रजिस्ट्रशन शुल्क में डिमाण्ड ड्राफ्ट दिनांक भरें", MinimumLength = 1)]
        public string RegFeeDemandDate { get; set; }
        [Required(ErrorMessage = "कृपया जनरल सिक्योरिटी में बैंक का नाम/शाखा भरें")]
        [StringLength(100, ErrorMessage = "कृपया जनरल सिक्योरिटी में बैंक का नाम/शाखा भरें", MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string SecurityBank { get; set; }
        [Required(ErrorMessage = "कृपया जनरल सिक्योरिटी में धनराशि भरें")]
        [StringLength(7, ErrorMessage = "कृपया जनरल सिक्योरिटी में धनराशि भरें", MinimumLength = 1)]
       // [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public string SecurityAmount { get; set; }
        [Required(ErrorMessage = "कृपया जनरल सिक्योरिटी में एफ0डी0आर0 संख्या भरें")]
        [StringLength(50, ErrorMessage = "कृपया जनरल सिक्योरिटी में एफ0डी0आर0 संख्या भरें", MinimumLength = 1)]
        public string SecurityFDRNuber { get; set; }
        [Required(ErrorMessage = "कृपया जनरल सिक्योरिटी में एफ0डी0आर0 दिनांक भरें")]
        [StringLength(10, ErrorMessage = "कृपया जनरल सिक्योरिटी में एफ0डी0आर0 दिनांक भरें", MinimumLength = 1)]
        public string SecurityFDRDate { get; set; }
        [Required(ErrorMessage = "कृपया जनरल सिक्योरिटी में परिपक्वता तिथि भरें")]
        [StringLength(10, ErrorMessage = "कृपया जनरल सिक्योरिटी में परिपक्वता तिथि भरें", MinimumLength = 1)]
        public string SecurityMatureDate { get; set; }
        [Required(ErrorMessage = "कृपया फर्म का विवरण भरें")]
        [StringLength(500, ErrorMessage = "कृपया फर्म का विवरण भरें", MinimumLength = 1)]
        public string FirmDetail { get; set; }
        public string Category { get; set; }
        public DateTime CreatedOn { get; set; }
        public IFormFile ToolsPlantsFile { get; set; }
        public IFormFile ListofStaffFile { get; set; }
        public IFormFile BlackListFile { get; set; }
        public IFormFile FourYearMainWorksFile { get; set; }
        public IFormFile BalanceSheetFile { get; set; }
        public IFormFile IncomeTaxReturnFile { get; set; }
        [Required(ErrorMessage = "कृपया PAN भरें")]
        [StringLength(10, ErrorMessage = "कृपया PAN भरें", MinimumLength = 1)]
        [RegularExpression(@"[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}", ErrorMessage = "कृपया सही PAN भरें")]
        public string PAN { get; set; }
        [Required(ErrorMessage = "PAN अपलोड करें")]
        //[FileSizeValidation(1 * 1024 * 1024)]
        public IFormFile PANFile { get; set; }
        [Required(ErrorMessage = "कृपया GST भरें")]
        [StringLength(16, ErrorMessage = "कृपया GST भरें", MinimumLength = 1)]
        public string GST { get; set; }
        [Required(ErrorMessage = "GST अपलोड करें")]
       // [FileSizeValidation(1 * 1024 * 1024)]
        public IFormFile GSTFile { get; set; }
        [Required(ErrorMessage = "हैसियत प्रमाण पत्र अपलोड करें")]
     //   [FileSizeValidation(1 * 1024 * 1024)]
        public IFormFile HasiyatCertificate { get; set; }
        public IFormFile BloodRelation { get; set; }

    }
}
