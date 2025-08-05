using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Api.Validators
{
    public class MyEmailDomainAttribute : ValidationAttribute
    {
        private readonly string[] _domains;
        public MyEmailDomainAttribute(params string[] domains) // ["google.com", "microsoft.com"]
        {
            _domains = domains;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is string)
            {
                var email = value.ToString();

                // maher@microsoft.com   ===> ["maher","microsoft.com"]

                var parts = email.Split('@');

                var domain = parts.Last();

                if(_domains.Contains(domain))
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult($"Email Address Domain must be one of the following {string.Join(", ", _domains)}");
            }

            return new ValidationResult("Invalid Email Address");
        }
    }
}
