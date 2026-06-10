using System.ComponentModel.DataAnnotations;

namespace PRN232.LMS.Services.Models
{
    /// <summary>
    /// User Response DTO (for API output)
    /// </summary>
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }

    /// <summary>
    /// User Request DTO (for API input - Create)
    /// </summary>
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Username không được để trống")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username phải từ 3 đến 50 ký tự")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password không được để trống")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password phải từ 6 đến 100 ký tự")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role không được để trống")]
        public string Role { get; set; } // Admin, Student, Teacher
    }

    /// <summary>
    /// User Request DTO (for API input - Update)
    /// </summary>
    public class UpdateUserRequest
    {
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username phải từ 3 đến 50 ký tự")]
        public string? Username { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password phải từ 6 đến 100 ký tự")]
        public string? Password { get; set; }

        public string? Role { get; set; }
    }
}