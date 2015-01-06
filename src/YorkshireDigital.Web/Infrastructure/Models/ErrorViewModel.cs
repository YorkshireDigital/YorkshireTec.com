namespace YorkshireDigital.Web.Infrastructure.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Nancy.Validation;

    public class ErrorViewModel
    {
        public FieldErrorViewModel[] Errors { get; set; }

        public ErrorViewModel(IEnumerable<KeyValuePair<string, IList<ModelValidationError>>> errors)
        {
            Errors = errors.Select(x => new FieldErrorViewModel(x.Key, x.Value)).ToArray();
        }
    }
}