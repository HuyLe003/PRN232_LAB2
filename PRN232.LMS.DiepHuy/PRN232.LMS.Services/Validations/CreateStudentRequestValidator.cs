using FluentValidation;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Validations
{
    /// <summary>
    /// FluentValidation for CreateStudentRequest
    /// Implements advanced validation rules using FluentValidation library
    /// </summary>
    public class CreateStudentRequestValidator : AbstractValidator<CreateStudentRequest>
    {
        public CreateStudentRequestValidator()
        {
            // FullName validation
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Tên sinh viên không được để trống")
                .Length(2, 100)
                .WithMessage("Tên sinh viên phải từ 2 đến 100 ký tự")
                .Matches(@"^[a-zA-Z\s\u0100-\uFFFF]+$")
                .WithMessage("Tên sinh viên không được chứa số hoặc ký tự đặc biệt");

            // Email validation
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email không được để trống")
                .EmailAddress()
                .WithMessage("Email không đúng định dạng")
                .Must(x => x.EndsWith("@example.com") || x.EndsWith("@gmail.com") || x.EndsWith("@outlook.com") || x.EndsWith("@fptu.edu.vn"))
                .WithMessage("Email phải sử dụng domain hợp lệ: @example.com, @gmail.com, @outlook.com, hoặc @fptu.edu.vn");

            // PhoneNumber validation
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Số điện thoại không được để trống")
                .Matches(@"^(0|84)(\d{9,10})$")
                .WithMessage("Số điện thoại phải bắt đầu bằng 0 hoặc 84 và có 10-11 chữ số");

            // StudentCode validation - with CUSTOM RULE
            RuleFor(x => x.StudentCode)
                .NotEmpty()
                .WithMessage("Mã sinh viên không được để trống")
                .Length(7)
                .WithMessage("Mã sinh viên phải có đúng 7 ký tự")
                .Matches(@"^(SE|CE|GD|IT)\d{5}$")
                .WithMessage("Mã sinh viên phải theo định dạng FPTU: SE19886, CE18793, GD20001, IT20123")
                .Must(IsValidStudentCodeFormat)
                .WithMessage("Mã sinh viên không hợp lệ - vui lòng kiểm tra lại");

            // DateOfBirth validation
            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("Ngày sinh không được để trống")
                .LessThan(DateTime.Today)
                .WithMessage("Ngày sinh phải là ngày trong quá khứ")
                .Must(BeValidAge)
                .WithMessage("Học sinh phải từ 16 tuổi trở lên");
        }

        /// <summary>
        /// CUSTOM VALIDATION: Validate Student Code Format
        /// Checks: SE/CE/GD/IT + 2-digit year (18-29) + 2-digit number (01-99)
        /// </summary>
        private bool IsValidStudentCodeFormat(string studentCode)
        {
            if (string.IsNullOrEmpty(studentCode) || studentCode.Length != 7)
                return false;

            try
            {
                // Extract parts
                var prefix = studentCode.Substring(0, 2);
                var year = studentCode.Substring(2, 2);
                var number = studentCode.Substring(4, 2);

                // Validate prefix
                var validPrefixes = new[] { "SE", "CE", "GD", "IT" };
                if (!validPrefixes.Contains(prefix))
                    return false;

                // Validate year (18-29 = 2018-2029)
                if (!int.TryParse(year, out var yearInt) || yearInt < 18 || yearInt > 29)
                    return false;

                // Validate number (01-99)
                if (!int.TryParse(number, out var numberInt) || numberInt < 1 || numberInt > 99)
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// CUSTOM VALIDATION: Check Age >= 16 years
        /// </summary>
        private bool BeValidAge(DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > DateTime.Today.AddYears(-age))
                age--;

            return age >= 16;
        }
    }
}