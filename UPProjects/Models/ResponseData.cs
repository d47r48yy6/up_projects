using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    public class ResponseData
    {
        public bool ResultStatus { get; set; }
        public string ResultMessage { get; set; }
        public object Data { get; set; }
    }
}
