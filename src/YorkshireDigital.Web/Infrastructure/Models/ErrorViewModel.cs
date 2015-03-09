namespace YorkshireDigital.Web.Infrastructure.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Nancy.Validation;

    public class ErrorViewModel
    {
        public List<FieldErrorViewModel> Errors { get; set; }

        public ErrorViewModel(IEnumerable<KeyValuePair<string, IList<ModelValidationError>>> errors)
        {
            Errors = errors.Select(x => new FieldErrorViewModel(x.Key, x.Value)).ToList();
        }

        public ErrorViewModel()
        {
            Errors = new List<FieldErrorViewModel>();
        }

        public void AddError(string field, IList<ModelValidationError> errors)
        {
            
        }
    }
}