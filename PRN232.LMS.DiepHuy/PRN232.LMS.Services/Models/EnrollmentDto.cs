using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.Models
{
    /// <summary>
    /// Enrollment Response DTO (for API output)
    /// </summary>
    public class EnrollmentDto
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollDate { get; set; }
        public string Status { get; set; }
    }

    /// <summary>
    /// Enrollment Request DTO (for API input)
    /// </summary>
    public class CreateEnrollmentRequest
    {
        [Required(ErrorMessage = "Mã sinh viên là bắt buộc")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Mã khóa học là bắt buộc")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Ngày đăng ký là bắt buộc")]
        public DateTime EnrollDate { get; set; }

        [Required(ErrorMessage = "Trạng thái không được để trống")]
        [StringLength(50, ErrorMessage = "Trạng thái không vượt quá 50 ký tự")]
        public string Status { get; set; }
    }

   public class UpdateEnrollmentRequest
    {
        [Required(ErrorMessage = "Ngày đăng ký là bắt buộc")]
        public DateTime EnrollDate { get; set; }

        [Required(ErrorMessage = "Trạng thái không được để trống")]
        [StringLength(50, ErrorMessage = "Trạng thái không vượt quá 50 ký tự")]
        public string Status { get; set; }
    }

    /// <summary>
    /// Enrollment with related data (expanded view)
    /// </summary>
    public class EnrollmentDetailDto : EnrollmentDto
    {
        public StudentDto Student { get; set; }
        public CourseDto Course { get; set; }
    }
}
