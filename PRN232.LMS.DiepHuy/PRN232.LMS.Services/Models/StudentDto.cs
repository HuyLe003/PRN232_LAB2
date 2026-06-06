using System.ComponentModel.DataAnnotations;
using PRN232.LMS.Services.Validations;

namespace PRN232.LMS.Services.Models
{
    /// <summary>
    /// Student Response DTO (for API output)
    /// </summary>
    public class StudentDto
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string StudentCode { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    /// <summary>
    /// Student Request DTO (for API input)
    /// Validation Attributes: Required, StringLength, EmailAddress, Phone, RegularExpression
    /// </summary>
    public class CreateStudentRequest
    {
        [Required(ErrorMessage = "Tên sinh viên không được để trống")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Tên sinh viên phải từ 2 đến 100 ký tự")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng (phải là số)")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Mã sinh viên không được để trống")]
        [RegularExpression(@"^(SE|CE|GD|IT)\d{5}$", 
            ErrorMessage = "Mã sinh viên phải theo định dạng FPTU: SE19886, CE18793, GD20001, IT20123")]
        [StudentCodeValidation(ErrorMessage = "Mã sinh viên không hợp lệ")]
        public string StudentCode { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        public DateTime DateOfBirth { get; set; }
    }

    public class UpdateStudentRequest
    {
        [Required(ErrorMessage = "Tên sinh viên không được để trống")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Tên sinh viên phải từ 2 đến 100 ký tự")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng (phải là số)")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Mã sinh viên không được để trống")]
        [RegularExpression(@"^(SE|CE|GD|IT)\d{5}$", 
            ErrorMessage = "Mã sinh viên phải theo định dạng FPTU: SE19886, CE18793, GD20001, IT20123")]
        [StudentCodeValidation(ErrorMessage = "Mã sinh viên không hợp lệ")]
        public string StudentCode { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        public DateTime DateOfBirth { get; set; }
    }

    /// <summary>
    /// Student with enrollments (expanded view)
    /// </summary>
    public class StudentDetailDto : StudentDto
    {
        public List<EnrollmentDto> Enrollments { get; set; }
    }
}