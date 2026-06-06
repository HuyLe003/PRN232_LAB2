namespace PRN232.LMS.Repositories.Models.Entities
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int SemesterId { get; set; }

        public Semester Semester { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}