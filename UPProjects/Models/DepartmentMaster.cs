using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class DepartmentMaster
    {
        public int ID { get; set; }
        [Required(ErrorMessage ="* Please enter department name.")]
        public string DepartmentName { get; set; }
    }
}
