using pax.blazor.survey.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pax.blazor.survey.Models
{
    public class AnswerForm
    {
        [CustomValidation(typeof(AnswerForm), nameof(AnswerForm.ValidateAnswer))]
        public string Answer { get; set; }
        public bool BoolAnswer { get; set; }
        public static ValidationResult ValidateAnswer(string answer, ValidationContext vc)
        {
            if (String.IsNullOrEmpty(answer))
                return ValidationResult.Success;

            // ; not allowed
            return !answer.Contains(';')
                ? ValidationResult.Success
                : new ValidationResult(SurveyData.CustomAnswerFormValidationMessage, new[] { vc.MemberName });
        }
    }

    public class ValidateEachItemAttribute : ValidationAttribute
    {
        protected readonly List<ValidationResult> validationResults = new List<ValidationResult>();

        public override bool IsValid(object value)
        {
            var list = value as IEnumerable;
            if (list == null) return true;

            var isValid = true;

            foreach (var item in list)
            {
                var validationContext = new ValidationContext(item);
                var isItemValid = Validator.TryValidateObject(item, validationContext, validationResults, true);
                isValid &= isItemValid;
            }
            return isValid;
        }
    }


}
