using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class Tender
    {

        public string Id { get; set; }

        // [Display(Name = "Category")]
        public string ZoneId { get; set; }
        public string UnitId { get; set; }
        public string CategoryId { get; set; }

        [Required(ErrorMessage = "Please enter Tender Ref.No.")]
        [StringLength(500, ErrorMessage = "Please enter Tender Number.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Tender Ref.No.")]
        public string TenderNumber { get; set; }
        [Required(ErrorMessage = "Please enter Tender Title.")]
        [StringLength(500, ErrorMessage = "Please enter Tender Title.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Tender Title (in English)")]
        public string TenderTitle { get; set; }
        [StringLength(500, ErrorMessage = "Please enter Tender Title.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Tender Title (in Hindi)")]
        public string HNTenderTitle { get; set; }
        [Required(ErrorMessage = "Please enter e-Published Date.")]
        [StringLength(10, ErrorMessage = "Please enter Date of Tender.", MinimumLength = 10)]
        [DataType(DataType.Text)]
        [Display(Name = "e-Published Date")]
        public string DateofTender { get; set; }
        public string DateofTender1 { get; set; }
        [Required(ErrorMessage = "Please enter Closing Date.")]
        [StringLength(10, ErrorMessage = "Please enter Last Date of Sale.", MinimumLength = 10)]
        [DataType(DataType.Text)]
        [Display(Name = "Closing Date")]
        public string LastDateofSale { get; set; }
        [Required(ErrorMessage = "Please enter Opening Date.")]
        [StringLength(10, ErrorMessage = "Please enter Opening Date.", MinimumLength = 10)]
        [DataType(DataType.Text)]
        [Display(Name = "Opening Date")]
        public string OpeningDate { get; set; }
        //[Required(ErrorMessage = "Please select Tender Attachment.")]                
        //[Display(Name = "Tender Attachment")]
        public IFormFile file { get; set; }
        public string FileName { get; set; }
        public string FinYear { get; set; }

        public string ZoneIdHN { get; set; }
        public string UnitIdHN { get; set; }
    }
}
