using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.Models
{
    /// <summary>
    /// Subject Response DTO (for API output)
    /// </summary>
    public class SubjectDto
    {
        public int SubjectId { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public int Credit { get; set; }
    }

    /// <summary>
    /// Subject Request DTO (for API input)
    /// </summary>
    public class CreateSubjectRequest
    {
        [Required(ErrorMessage = "Subject code is required")]
        [StringLength(20, ErrorMessage = "Subject code cannot exceed 20 characters")]
        public string SubjectCode { get; set; }

        [Required(ErrorMessage = "Subject name is required")]
        [StringLength(100, ErrorMessage = "Subject name cannot exceed 100 characters")]
        public string SubjectName { get; set; }

        [Required(ErrorMessage = "Credit is required")]
        [Range(1, 10, ErrorMessage = "Credit must be between 1 and 10")]
        public int Credit { get; set; }
    }

    public class UpdateSubjectRequest
    {
        [Required(ErrorMessage = "Subject code is required")]
        [StringLength(20, ErrorMessage = "Subject code cannot exceed 20 characters")]
        public string SubjectCode { get; set; }

        [Required(ErrorMessage = "Subject name is required")]
        [StringLength(100, ErrorMessage = "Subject name cannot exceed 100 characters")]
        public string SubjectName { get; set; }

        [Required(ErrorMessage = "Credit is required")]
        [Range(1, 10, ErrorMessage = "Credit must be between 1 and 10")]
        public int Credit { get; set; }
    }
}
