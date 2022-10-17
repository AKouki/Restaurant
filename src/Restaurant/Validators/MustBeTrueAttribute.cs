using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Validators
{
    public class MustBeTrueAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string DefaultMessage = "The value must be True.";

        public void AddValidation(ClientModelValidationContext context)
        {
            var localizer = context.ActionContext.HttpContext.RequestServices.GetService<IStringLocalizer<SharedResource>>();

            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-mustbetrue", localizer?[ErrorMessageString] ?? DefaultMessage);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var localizer = validationContext.GetService<IStringLocalizer<SharedResource>>();
            var localizedErrorMessage = localizer?[ErrorMessageString] ?? DefaultMessage;

            if (value == null || value.GetType() != typeof(bool))
                return new ValidationResult(localizedErrorMessage);

            if (value.GetType() != typeof(bool))
                return new ValidationResult(localizedErrorMessage);

            return (bool)value == true ? 
                ValidationResult.Success :
                new ValidationResult(localizedErrorMessage);
        }
    }
}
