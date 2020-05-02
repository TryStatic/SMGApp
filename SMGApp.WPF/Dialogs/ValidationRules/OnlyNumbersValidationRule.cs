using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace SMGApp.WPF.Dialogs.ValidationRules
{
    public class OnlyNumbersValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null) return ValidationResult.ValidResult;
            if (!(value is string str)) return new ValidationResult(false, "ΤΟ ΠΕΔΙΟ ΠΡΕΠΕΙ ΝΑ ΠΕΡΙΕΧΕΙ ΜΟΝΟ ΑΡΙΘΜΟΥΣ");
            if (string.IsNullOrEmpty(str)) return ValidationResult.ValidResult;
            if(string.IsNullOrWhiteSpace(str)) return new ValidationResult(false, "ΤΟ ΠΕΔΙΟ ΠΡΕΠΕΙ ΝΑ ΠΕΡΙΕΧΕΙ ΜΟΝΟ ΑΡΙΘΜΟΥΣ");
            if (!IsDigitsOnly(str)) return new ValidationResult(false, "ΤΟ ΠΕΔΙΟ ΠΡΕΠΕΙ ΝΑ ΠΕΡΙΕΧΕΙ ΜΟΝΟ ΑΡΙΘΜΟΥΣ");
            return ValidationResult.ValidResult;
        }

        private static bool IsDigitsOnly(string str) => str != null && str.All(c => c >= '0' && c <= '9');
    }


    public class AtLeastTenCharactersValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null) return ValidationResult.ValidResult;
            if (!(value is string str)) return new ValidationResult(false, "ΤΟ ΠΕΔΙΟ ΠΡΕΠΕΙ ΝΑ ΠΕΡΙΕΧΕΙ ΤΟΥΛΑΧΙΣΤΟΝ 10 ΧΑΡΑΚΤΗΡΕΣ");
            if (string.IsNullOrEmpty(str)) return ValidationResult.ValidResult;
            if(str.Length < 10) return new ValidationResult(false, "ΤΟ ΠΕΔΙΟ ΠΡΕΠΕΙ ΝΑ ΠΕΡΙΕΧΕΙ ΤΟΥΛΑΧΙΣΤΟΝ 10 ΧΑΡΑΚΤΗΡΕΣ");
            return ValidationResult.ValidResult;
        }

    }


}