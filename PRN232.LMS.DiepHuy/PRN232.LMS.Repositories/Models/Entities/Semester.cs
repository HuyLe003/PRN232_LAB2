namespace PRN232.LMS.Repositories.Models.Entities
{
    public class Semester
    {
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}