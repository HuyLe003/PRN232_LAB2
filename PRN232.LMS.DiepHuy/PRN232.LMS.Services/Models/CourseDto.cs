using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.Models
{
    /// <summary>
    /// Course Response DTO (for API output)
    /// </summary>
    public class CourseDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int SemesterId { get; set; }
    }

    /// <summary>
    /// Course Request DTO (for API input)
    /// Validation: Required, StringLength, Range, RegularExpression
    /// </summary>
    public class CreateCourseRequest
    {
        [Required(ErrorMessage = "Tên khóa học không được để trống")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Tên khóa học phải từ 3 đến 200 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-().áàảãạăằắẳẵặâầấẩẫậèéẻẽẹêềếểễệìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵđ]+$", 
            ErrorMessage = "Tên khóa học chứa ký tự không hợp lệ")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "ID học kỳ không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "ID học kỳ phải lớn hơn 0")]
        public int SemesterId { get; set; }
    }

    public class UpdateCourseRequest
    {
        [Required(ErrorMessage = "Tên khóa học không được để trống")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Tên khóa học phải từ 3 đến 200 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-().áàảãạăằắẳẵặâầấẩẫậèéẻẽẹêềếểễệìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵđ]+$", 
            ErrorMessage = "Tên khóa học chứa ký tự không hợp lệ")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "ID học kỳ không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "ID học kỳ phải lớn hơn 0")]
        public int SemesterId { get; set; }
    }

    /// <summary>
    /// Course with related data (expanded view)
    /// </summary>
    public class CourseDetailDto : CourseDto
    {
        public SemesterDto Semester { get; set; }
        public List<EnrollmentDto> Enrollments { get; set; }
    }
}