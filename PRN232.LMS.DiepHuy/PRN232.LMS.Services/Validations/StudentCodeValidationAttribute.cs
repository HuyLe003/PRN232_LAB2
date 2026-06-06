using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.Validations
{
    /// <summary>
    /// CUSTOM VALIDATION ATTRIBUTE for FPTU Student Code
    /// 
    /// Format: [PREFIX][YEAR][NUMBER]
    /// - PREFIX: SE (Software Engineering), CE (Cloud Engineering), GD (Game Design), IT (IT)
    /// - YEAR: 2-digit year (18-29 = 2018-2029)
    /// - NUMBER: 2-digit number (01-99)
    /// 
    /// Examples: SE19886, CE18793, GD20001, IT20123
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StudentCodeValidationAttribute : ValidationAttribute
    {
        private static readonly string[] ValidPrefixes = { "SE", "CE", "GD", "IT" };

        public StudentCodeValidationAttribute()
        {
            ErrorMessage = "Mã sinh viên phải theo định dạng FPTU (ví dụ: SE19886, CE18793)";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            var studentCode = value.ToString();

            // Check length
            if (string.IsNullOrEmpty(studentCode) || studentCode.Length != 7)
            {
                ErrorMessage = "Mã sinh viên phải có đúng 7 ký tự";
                return false;
            }

            // Check format: [PREFIX][YY][NN]
            try
            {
                var prefix = studentCode.Substring(0, 2);
                var year = studentCode.Substring(2, 2);
                var number = studentCode.Substring(4, 2);

                // Validate prefix
                if (!ValidPrefixes.Contains(prefix))
                {
                    ErrorMessage = $"Prefix phải là một trong: {string.Join(", ", ValidPrefixes)}";
                    return false;
                }

                // Validate year (18-29 = 2018-2029)
                if (!int.TryParse(year, out var yearInt))
                {
                    ErrorMessage = "Năm phải là số";
                    return false;
                }

                if (yearInt < 18 || yearInt > 29)
                {
                    ErrorMessage = "Năm phải từ 18 đến 29 (2018-2029)";
                    return false;
                }

                // Validate number (01-99)
                if (!int.TryParse(number, out var numberInt))
                {
                    ErrorMessage = "Số thứ tự phải là số";
                    return false;
                }

                if (numberInt < 1 || numberInt > 99)
                {
                    ErrorMessage = "Số thứ tự phải từ 01 đến 99";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Mã sinh viên không hợp lệ: {ex.Message}";
                return false;
            }
        }
    }
}