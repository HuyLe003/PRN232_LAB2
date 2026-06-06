using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.Models
{
    /// <summary>
    /// Semester Response DTO (for API output)
    /// </summary>
    public class SemesterDto
    {
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    /// <summary>
    /// Semester Request DTO (for API input)
    /// </summary>
    public class CreateSemesterRequest
    {
        [Required(ErrorMessage = "Tên học kỳ không được để trống")]
        [StringLength(100, ErrorMessage = "Tên học kỳ không vượt quá 100 ký tự")]
        public string SemesterName { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        public DateTime EndDate { get; set; }
    }

    public class UpdateSemesterRequest
    {
        [Required(ErrorMessage = "Tên học kỳ không được để trống")]
        [StringLength(100, ErrorMessage = "Tên học kỳ không vượt quá 100 ký tự")]
        public string SemesterName { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        public DateTime EndDate { get; set; }
    }

    /// <summary>
    /// Semester with related courses (expanded view)
    /// </summary>
    public class SemesterDetailDto : SemesterDto
    {
        public List<CourseDto> Courses { get; set; }
    }
}
