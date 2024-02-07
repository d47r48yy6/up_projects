using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UPProjects.Models
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class FileSizeValidation : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public FileSizeValidation(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        public override bool IsValid(object value)
        {
            var file = value as IFormFile;
            if (file == null)
            {
                return false;
            }
            return file.Length <= _maxFileSize;
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(_maxFileSize.ToString());
        }

        //private readonly int _maxSize;
        ////private readonly string  _maxSize;
        //public FileSizeValidation(int maxSize)
        //{
        //    _maxSize = maxSize;
        //}


        //public override string FormatErrorMessage(string name)
        //{
        //    return "Size of File must be less than 1024KB.";
        //}
        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    var file = value as IFormFile;
        //    if (file != null)
        //    {
        //        if (file.Length > _maxSize)
        //        {
        //            return new ValidationResult("Size of File must be less than 1024KB.");
        //        }

        //    }          
        //    return ValidationResult.Success;
        //}
        //public string GetErrorMessage()
        //{
        //    return "Size of File must be less than 1024KB.";
        //}
    }
}
