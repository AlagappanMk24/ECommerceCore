using ECommerceCore.Domain.Exceptions;
using System.Text;
using System.Text.RegularExpressions;

namespace ECommerceCore.Domain.ValueObjects
{
    public class RegexValidation
    {
    }
    public sealed record Email
    {
        public string Value { get; }
        public string Domain { get; }

        private static readonly string[] _allowedDomains =
        {
            "gmail.com", "yahoo.com", "outlook.com", "icloud.com",
            "company.com", "edu.in", "mail.ru", "qq.com", "163.com"
         };

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidEmailException(value, "Email cannot be empty");

            var formatted = value.Trim().ToLowerInvariant();

            if (!Regex.IsMatch(formatted, @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$"))
                throw new InvalidEmailException(value);

            Domain = formatted.Split('@')[1];

            if (!_allowedDomains.Contains(Domain))
                throw new InvalidEmailException(value, $"We don't accept emails from {Domain}");

            Value = formatted;
        }

        public override string ToString() => Value;
    }
    public sealed record PhoneNumber
    {
        public string Value { get; }
        public string CountryCode { get; }
        public string NationalNumber { get; }

        private static readonly Dictionary<string, Regex> _countryPatterns = new()
        {
            // Country Code => Regex Pattern
            ["US"] = new Regex(@"^(\+1)?[2-9]\d{2}[2-9](?!11)\d{6}$"),  // +12125551212 or 2125551212
            ["IN"] = new Regex(@"^(\+91|0)?[6-9]\d{9}$"),                // +919876543210 or 9876543210
            ["CA"] = new Regex(@"^(\+1)?[2-9]\d{2}[2-9](?!11)\d{6}$"),  // Same as US
            ["AU"] = new Regex(@"^(\+61|0)?4\d{8}$"),                    // +61412345678 or 0412345678
            ["UK"] = new Regex(@"^(\+44|0)?7\d{9}$"),                   // +447912345678 or 07912345678
            ["RU"] = new Regex(@"^(\+7|8)?[3489]\d{9}$"),              // +79123456789 or 89123456789
            ["CN"] = new Regex(@"^(\+86)?(1[3-9]\d{9}|[2-9]\d{1,2}\d{7,8})$")  // +8613812345678 or 13812345678 (mobile) or 02112345678 (landline)
        };
        public PhoneNumber(string nationalNumber, string countryCode)
        {
            if (string.IsNullOrWhiteSpace(nationalNumber))
                throw new InvalidPhoneException("Phone number cannot be empty");

            if (string.IsNullOrWhiteSpace(countryCode))
                throw new InvalidPhoneException("Country code is required");

            CountryCode = countryCode.ToUpper();
            Value = FormatNumber(nationalNumber, CountryCode);
            NationalNumber = nationalNumber;

            if (!_countryPatterns.TryGetValue(CountryCode, out var regex) || !regex.IsMatch(Value))
                throw new InvalidPhoneException($"Invalid {CountryCode} phone number format");
        }
        //public PhoneNumber(string value, string countryCode = null)
        //{
        //    if (string.IsNullOrWhiteSpace(value))
        //        throw new InvalidPhoneException(value, "Phone number cannot be empty");

        //    // Format the number (remove all non-digit characters)
        //    var digitsOnly = new string(value.Where(char.IsDigit).ToArray());

        //    // If country code is specified, use that
        //    if (!string.IsNullOrEmpty(countryCode))
        //    {
        //        CountryCode = countryCode.ToUpper();
        //        Value = FormatNumber(digitsOnly, CountryCode);
        //    }
        //    else
        //    {
        //        // Auto-detect country code
        //        Value = FormatNumber(digitsOnly);
        //        CountryCode = DetectCountryCode(Value);
        //    }

        //    // Validate
        //    if (!_countryPatterns.TryGetValue(CountryCode, out var regex) || !regex.IsMatch(Value))
        //        throw new InvalidPhoneException(Value, CountryCode);

        //    // Extract national number (without country code)
        //    NationalNumber = ExtractNationalNumber(Value, CountryCode);
        //}

        private static string FormatNumber(string digitsOnly, string countryCode = null)
        {
            return countryCode?.ToUpper() switch
            {
                "US" or "CA" => digitsOnly.Length == 10 ? $"+1{digitsOnly}" : digitsOnly,
                "IN" => digitsOnly.Length == 10 ? $"+91{digitsOnly}" : digitsOnly,
                "AU" => digitsOnly.Length == 9 ? $"+61{digitsOnly}" : digitsOnly,
                "UK" => digitsOnly.Length == 10 ? $"+44{digitsOnly.Substring(1)}" : digitsOnly,
                "RU" => digitsOnly.Length == 10 ? $"+7{digitsOnly.Substring(1)}" : digitsOnly,
                "CN" => digitsOnly.Length == 11 ? $"+86{digitsOnly}" :
                          digitsOnly.Length >= 8 && digitsOnly.Length <= 11 ? $"+86{digitsOnly}" :
                          digitsOnly,
                _ => digitsOnly.StartsWith("+") ? digitsOnly : $"+{digitsOnly}"
            };
        }

        private static string DetectCountryCode(string formattedNumber)
        {
            return formattedNumber switch
            {
                _ when formattedNumber.StartsWith("+1") => "US", // Default to US for +1
                _ when formattedNumber.StartsWith("+91") => "IN",
                _ when formattedNumber.StartsWith("+61") => "AU",
                _ when formattedNumber.StartsWith("+44") => "UK",
                _ when formattedNumber.StartsWith("+7") => "RU",
                _ when formattedNumber.StartsWith("+86") => "CN",
                _ => throw new InvalidPhoneException(formattedNumber, "Unsupported country code")
            };
        }

        private static string ExtractNationalNumber(string formattedNumber, string countryCode)
        {
            return countryCode switch
            {
                "US" or "CA" => formattedNumber.StartsWith("+1") ? formattedNumber[2..] : formattedNumber,
                "IN" => formattedNumber.StartsWith("+91") ? formattedNumber[3..] : formattedNumber,
                "AU" => formattedNumber.StartsWith("+61") ? formattedNumber[3..] : formattedNumber,
                "UK" => formattedNumber.StartsWith("+44") ? formattedNumber[3..] : formattedNumber,
                "RU" => formattedNumber.StartsWith("+7") ? formattedNumber[2..] : formattedNumber,
                "CN" => formattedNumber.StartsWith("+86") ? formattedNumber[3..] : formattedNumber,
                _ => formattedNumber
            };
        }

        public override string ToString() => Value;
    }
}
