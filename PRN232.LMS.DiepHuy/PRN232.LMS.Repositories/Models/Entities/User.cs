using System;

namespace PRN232.LMS.Repositories.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // Ví dụ: "Admin", "Student", "Teacher"
    }
}