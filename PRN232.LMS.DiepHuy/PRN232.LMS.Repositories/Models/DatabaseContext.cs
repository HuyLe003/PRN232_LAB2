using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Models.Entities;

namespace PRN232.LMS.Repositories.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User
modelBuilder.Entity<User>(entity =>
{
    entity.HasKey(u => u.UserId);
    entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
    entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
    entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
});

            // Configure Semester
            modelBuilder.Entity<Semester>()
                .HasKey(s => s.SemesterId);

            // Configure Course
            modelBuilder.Entity<Course>()
                .HasKey(c => c.CourseId);
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Semester)
                .WithMany(s => s.Courses)
                .HasForeignKey(c => c.SemesterId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Subject
            modelBuilder.Entity<Subject>()
                .HasKey(s => s.SubjectId);

            // Configure Student
            modelBuilder.Entity<Student>()
                .HasKey(s => s.StudentId);

            // Configure Enrollment
            modelBuilder.Entity<Enrollment>()
                .HasKey(e => e.EnrollmentId);
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed Semesters
            modelBuilder.Entity<Semester>().HasData(
                new Semester { SemesterId = 1, SemesterName = "Spring 2024", StartDate = new DateTime(2024, 1, 15), EndDate = new DateTime(2024, 5, 15) },
                new Semester { SemesterId = 2, SemesterName = "Summer 2024", StartDate = new DateTime(2024, 6, 1), EndDate = new DateTime(2024, 8, 31) },
                new Semester { SemesterId = 3, SemesterName = "Fall 2024", StartDate = new DateTime(2024, 9, 1), EndDate = new DateTime(2024, 12, 20) },
                new Semester { SemesterId = 4, SemesterName = "Winter 2024", StartDate = new DateTime(2024, 12, 21), EndDate = new DateTime(2025, 1, 31) },
                new Semester { SemesterId = 5, SemesterName = "Spring 2025", StartDate = new DateTime(2025, 2, 1), EndDate = new DateTime(2025, 5, 31) }
            );

            // Seed Subjects
            modelBuilder.Entity<Subject>().HasData(
                new Subject { SubjectId = 1, SubjectCode = "PRN232", SubjectName = "Web Development", Credit = 3 },
                new Subject { SubjectId = 2, SubjectCode = "PRN231", SubjectName = "Desktop Development", Credit = 3 },
                new Subject { SubjectId = 3, SubjectCode = "MAT101", SubjectName = "Mathematics I", Credit = 4 },
                new Subject { SubjectId = 4, SubjectCode = "ENG101", SubjectName = "English I", Credit = 3 },
                new Subject { SubjectId = 5, SubjectCode = "DB101", SubjectName = "Database Design", Credit = 3 },
                new Subject { SubjectId = 6, SubjectCode = "CS101", SubjectName = "Computer Science Fundamentals", Credit = 4 },
                new Subject { SubjectId = 7, SubjectCode = "SYS101", SubjectName = "System Design", Credit = 3 },
                new Subject { SubjectId = 8, SubjectCode = "NET101", SubjectName = "Network Basics", Credit = 3 },
                new Subject { SubjectId = 9, SubjectCode = "SEC101", SubjectName = "Information Security", Credit = 3 },
                new Subject { SubjectId = 10, SubjectCode = "AI101", SubjectName = "Artificial Intelligence", Credit = 4 }
            );

            // Seed Courses (20 courses)
            var courses = new List<Course>();
            for (int i = 1; i <= 20; i++)
            {
                courses.Add(new Course
                {
                    CourseId = i,
                    CourseName = $"Course {i}",
                    SemesterId = ((i - 1) % 5) + 1
                });
            }
            modelBuilder.Entity<Course>().HasData(courses);

            // Seed Students (50 students)
            var students = new List<Student>();
            string[] firstNames = { "Nguyen", "Tran", "Pham", "Hoang", "Vu", "Dang", "Bui", "Dinh", "Do", "Ngo" };
            string[] lastNames = { "Van A", "Van B", "Van C", "Van D", "Van E", "Thi F", "Thi G", "Thi H", "Thi I", "Thi K" };

            for (int i = 1; i <= 50; i++)
            {
                students.Add(new Student
                {
                    StudentId = i,
                    FullName = $"{firstNames[(i - 1) % 10]} {lastNames[(i - 1) % 10]} {i}",
                    Email = $"student{i}@lms.edu.vn",
                    DateOfBirth = new DateTime(2000 + (i % 5), (i % 12) + 1, (i % 28) + 1)
                });
            }
            modelBuilder.Entity<Student>().HasData(students);

            // Seed Enrollments (500 enrollments)
            var enrollments = new List<Enrollment>();
            Random random = new Random(42); // Using seed for reproducibility
            var statuses = new[] { "Active", "Completed", "Pending", "Dropped" };

            for (int i = 1; i <= 500; i++)
            {
                int studentId = ((i - 1) % 50) + 1;
                int courseId = ((i - 1) % 20) + 1;
                
                enrollments.Add(new Enrollment
                {
                    EnrollmentId = i,
                    StudentId = studentId,
                    CourseId = courseId,
                    EnrollDate = new DateTime(2024, random.Next(1, 13), random.Next(1, 29)),
                    Status = statuses[random.Next(statuses.Length)]
                });
            }
            modelBuilder.Entity<Enrollment>().HasData(enrollments);

            modelBuilder.Entity<User>().HasData(new User
{
    UserId = 1,
    Username = "admin",
    // Mật khẩu mẫu đã hash (ví dụ chuỗi hash của "Admin@123" hoặc tạm thời để chuỗi text nếu chưa cài thư viện hash)
    PasswordHash = "AQAAAAIAAYagAAAAEJrO...", 
    Role = "Admin"
});
        }
    }
}