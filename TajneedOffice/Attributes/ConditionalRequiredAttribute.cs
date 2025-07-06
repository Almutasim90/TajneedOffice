using System.ComponentModel.DataAnnotations;

namespace TajneedOffice.Attributes
{
    /// <summary>
    /// Custom validation attribute for conditional required fields
    /// </summary>
    public class ConditionalRequiredAttribute : ValidationAttribute
    {
        private readonly string _dependentProperty;
        private readonly object[] _targetValues;

        public ConditionalRequiredAttribute(string dependentProperty, params object[] targetValues)
        {
            _dependentProperty = dependentProperty;
            _targetValues = targetValues;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var dependentPropertyInfo = instance.GetType().GetProperty(_dependentProperty);
            
            if (dependentPropertyInfo == null)
            {
                return new ValidationResult($"Property {_dependentProperty} not found");
            }

            var dependentValue = dependentPropertyInfo.GetValue(instance);
            
            // Check if the dependent value matches any of the target values
            bool shouldBeRequired = _targetValues.Contains(dependentValue);

            if (shouldBeRequired && (value == null || string.IsNullOrWhiteSpace(value.ToString())))
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required");
            }

            return ValidationResult.Success;
        }
    }
} 