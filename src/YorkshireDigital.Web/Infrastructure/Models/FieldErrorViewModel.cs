namespace YorkshireDigital.Web.Infrastructure.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Nancy.Validation;

    public class FieldErrorViewModel
    {
        public FieldErrorViewModel(string name, IEnumerable<ModelValidationError> errors)
        {
            Name = name;
            Errors = errors.Select(x => x.ErrorMessage).Distinct().ToArray();
        }

        public FieldErrorViewModel(string name, string error)
        {
            Name = name;
            Errors = new[] { error };
        }

        public string Name { get; set; }
        public string[] Errors { get; set; }
    }
}