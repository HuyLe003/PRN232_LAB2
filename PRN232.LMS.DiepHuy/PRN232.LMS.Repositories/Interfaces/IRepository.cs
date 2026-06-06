using PRN232.LMS.Repositories.Models.Entities;
using PRN232.LMS.Repositories.Models;
using System.Threading.Tasks;

namespace PRN232.LMS.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        // Hàm đặc thù dùng riêng cho Authentication để tìm tài khoản khi Login
        Task<User?> GetByUsernameAsync(string username);
    }
    /// <summary>
    /// Generic repository interface for all entities
    /// </summary>
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<(List<T> Items, int Total)> GetPagedAsync(int page, int pageSize, string? search = null, string? sort = null);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task SaveChangesAsync();
    }

    /// <summary>
    /// Repository interface for Student entity
    /// </summary>
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student> GetStudentWithEnrollmentsAsync(int id);
        Task<List<Student>> SearchStudentsAsync(string searchTerm, int page, int pageSize);
    }

    /// <summary>
    /// Repository interface for Course entity
    /// </summary>
    public interface ICourseRepository : IRepository<Course>
    {
        Task<Course> GetCourseWithDetailsAsync(int id);
        Task<List<Course>> GetCoursesBySemesterAsync(int semesterId);
    }

    /// <summary>
    /// Repository interface for Enrollment entity
    /// </summary>
    public interface IEnrollmentRepository : IRepository<Enrollment>
    {
        Task<Enrollment> GetEnrollmentWithDetailsAsync(int id);
        Task<List<Enrollment>> GetEnrollmentsByStudentAsync(int studentId);
        Task<List<Enrollment>> GetEnrollmentsByCourseAsync(int courseId);
    }

    /// <summary>
    /// Repository interface for Semester entity
    /// </summary>
    public interface ISemesterRepository : IRepository<Semester>
    {
        Task<Semester> GetSemesterWithCoursesAsync(int id);
    }

    /// <summary>
    /// Repository interface for Subject entity
    /// </summary>
    public interface ISubjectRepository : IRepository<Subject>
    {
        Task<Subject> GetBySubjectCodeAsync(string code);
    }
}
