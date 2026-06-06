namespace PRN232.LMS.Repositories.Models.Entities
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}