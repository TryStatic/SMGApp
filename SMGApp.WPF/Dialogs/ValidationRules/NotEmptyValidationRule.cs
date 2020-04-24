using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace SMGApp.WPF.Dialogs.ValidationRules
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null) return new ValidationResult(false, "Field is required.");
            if (!(value is string str)) return new ValidationResult(false, "Field is required.");
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str)) return new ValidationResult(false, "Field is required.");
            return ValidationResult.ValidResult;
        }
    }
}
