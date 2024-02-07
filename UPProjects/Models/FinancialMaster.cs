using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace UPProjects.Models
{
    public class FinancialMaster
    {

        public int Id { get; set; }

        [Display(Name = "Financial Year")]
        [Required, Range(1, int.MaxValue, ErrorMessage = "* Please select Financial Year")]
        public string Year { get; set; }

        [Required(ErrorMessage ="* Please enter Work Done")]
        [Display(Name = "Work Done")]
        public decimal ?WorkDone { get; set; }
        [Required(ErrorMessage = "* Please enter Net Profit")]
        [Display(Name = "Net Profit")]
        public decimal ?NetProfit { get; set; }
        [Required(ErrorMessage = "* Please enter Cummulative Reserves")]
        [Display(Name = "Cummulative Reserves")]
        public decimal? CumulativeReservers { get; set; }

        public string FinancialYearText { get; set; }



    }
}
