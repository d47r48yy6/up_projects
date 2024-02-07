using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class ContractorRenewal
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "कृपया ठेकेदार का नाम/फर्म का नाम भरें")]
        [StringLength(100, ErrorMessage = "कृपया ठेकेदार का नाम/फर्म का नाम भरें", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "ठेकेदार का नाम/फर्म का नाम")]
        public string FirmName { get; set; }
        [Required(ErrorMessage = "कृपया मोबाइल नं0 भरें")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "कृपया सही मोबाइल नं0 भरें")]
        [StringLength(10, ErrorMessage = "कृपया मोबाइल नं0 भरें", MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string Mobile { get; set; }


      
        [Required(ErrorMessage = "कृपया ठेकेदार का पता भरें")]
        [StringLength(100, ErrorMessage = "कृपया ठेकेदार का पता भरें", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "कृपया ठेकेदार का पता भरें")]
        public string Address { get; set; }

       
        [Required(ErrorMessage = "कृपया ठेकेदार का पैन नंबर भरें")]
        [StringLength(100, ErrorMessage = "कृपया ठेकेदार का पैन नंबर भरें", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "कृपया ठेकेदार का पैन नंबर भरें")]
        public string PAN { get; set; }

       
        [Required(ErrorMessage = "कृपया ठेकेदार का जी० एस० टी० नंबर भरें")]
        [StringLength(100, ErrorMessage = "कृपया ठेकेदार का जी० एस० टी० नंबर भरें", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "कृपया ठेकेदार का जी० एस० टी० नंबर भरें")]
        public string GST { get; set; }

        [Required(ErrorMessage = "नवीनीकरण हेतु प्रार्थना पत्र अपलोड करें।")]
        public IFormFile RequestFile { get; set; }

        [Required(ErrorMessage = "पुराने पंजीकरण की छायाप्रति अपलोड करें।")]
        public IFormFile OldRegistration { get; set; }

        [Required(ErrorMessage = "जिलाधिकारी द्वार निर्गत वैध चरित्र प्रमाण पत्र अपलोड करें।")]
        public IFormFile CharacterCertificate { get; set; }

        [Required(ErrorMessage = "हैसियत प्रमाण पत्र अपलोड करें।")]
        public IFormFile HasiyatCertificate { get; set; }

         
        public IFormFile WorkCertificate { get; set; }

        [Required(ErrorMessage = "संस्तुति पत्र अपलोड करें।")]
        public IFormFile StampCertificate { get; set; }

        [Required(ErrorMessage = "डिमाण्ड ड्राफ्ट अपलोड करें।")]
        public IFormFile DemandDraft { get; set; }
        [Required(ErrorMessage = "GSTR1 ड्राफ्ट अपलोड करें।")]
        public IFormFile GSTR1 { get; set; }

        [Required(ErrorMessage = "GSTR2 ड्राफ्ट अपलोड करें।")]
        public IFormFile GSTR2 { get; set; }

    }
}
