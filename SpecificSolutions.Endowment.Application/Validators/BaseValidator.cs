using FluentValidation;

namespace SpecificSolutions.Endowment.Application.Validators
{
    public abstract class BaseValidator<T> : AbstractValidator<T>
    {
        protected BaseValidator()
        {
            // Common validation rules can be added here
        }

        /// <summary>
        /// Validates if a string is a valid GUID
        /// </summary>
        protected bool BeValidGuid(string guidString)
        {
            if (string.IsNullOrWhiteSpace(guidString))
                return false;
            return Guid.TryParse(guidString, out _);
        }

        /// <summary>
        /// Validates if a string contains only Arabic and English letters, numbers, and spaces
        /// </summary>
        protected bool BeValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            // Allow Arabic, English letters, numbers, spaces, and common punctuation
            return System.Text.RegularExpressions.Regex.IsMatch(name, @"^[\u0600-\u06FF\u0750-\u077F\u08A0-\u08FF\uFB50-\uFDFF\uFE70-\uFEFFa-zA-Z0-9\s\-\.\(\)]+$");
        }

        /// <summary>
        /// Validates if a string is a valid phone number (Libyan format)
        /// </summary>
        protected bool BeValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            // Libyan phone number format: 09x-xxxxxxx or 02x-xxxxxxx
            return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^(09[1-5]|02[1-9])-?\d{7}$");
        }

        /// <summary>
        /// Validates if a date is not in the future
        /// </summary>
        protected bool BeNotInFuture(DateTime date)
        {
            return date <= DateTime.Today;
        }

        /// <summary>
        /// Validates if a date is not too far in the past (within 100 years)
        /// </summary>
        protected bool BeNotTooOld(DateTime date)
        {
            return date >= DateTime.Today.AddYears(-100);
        }
    }
}
