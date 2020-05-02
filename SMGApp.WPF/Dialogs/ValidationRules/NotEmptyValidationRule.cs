using System.Globalization;
using System.Windows.Controls;

namespace SMGApp.WPF.Dialogs.ValidationRules
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null) return new ValidationResult(false, "ΤΟ ΠΕΔΙΟ ΔΕΝ ΜΠΟΡΕΙ ΝΑ ΕΙΝΑΙ ΚΕΝΟ");
            if (!(value is string str)) return new ValidationResult(false, "ΤΟ ΠΕΔΙΟ ΔΕΝ ΜΠΟΡΕΙ ΝΑ ΕΙΝΑΙ ΚΕΝΟ");
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str)) return new ValidationResult(false, "ΤΟ ΠΕΔΙΟ ΔΕΝ ΜΠΟΡΕΙ ΝΑ ΕΙΝΑΙ ΚΕΝΟ");
            return ValidationResult.ValidResult;
        }
    }
}
