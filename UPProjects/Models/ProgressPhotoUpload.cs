using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class ProgressPhotoUpload
    {
        public string Id { get; set; }
        // [Required, Range(1, int.MaxValue, ErrorMessage = "Please select Category.")]
        // [Display(Name = "Category")]
        [Required, Range(1,int.MaxValue,ErrorMessage ="Please select Category")]                
        [Display(Name = "Category *")]
        public string Category { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Please select Project")]
        [Display(Name = "Project *")]
        public string ProjectId { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Please select Unit")]
        [Display(Name = "Unit *")]
        public string UnitId { get; set; }

        //[Required(ErrorMessage = "Please enter Title.")]
        [StringLength(50, ErrorMessage = "Please enter Title.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Title * (Max Size: 50 Character)")]
        public string Title { get; set; }
       // [Required(ErrorMessage = "Please enter Description.")]
        [StringLength(100, ErrorMessage = "Please enter Description.", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [Display(Name = "Description of Progress (Max Size: 100 Character)")]
        public string Description { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string FileName { get; set; }


        public List<IFormFile> file { get; set; }
        public SelectList Months { get; set; }
        public SelectList Years { get; set; }
        public SelectList Units { get; set; }
        public SelectList Projects { get; set; }
    }


}
