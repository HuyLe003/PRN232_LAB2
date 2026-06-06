using PRN232.LMS.Services.Models;
using PRN232.LMS.Repositories.Models;

namespace PRN232.LMS.Services.Interfaces
{
    /// <summary>
    /// Service interface for Student business logic
    /// </summary>
    public interface IStudentService
    {
        Task<StudentDto?> GetStudentByIdAsync(int id);
        Task<StudentDetailDto?> GetStudentByIdWithEnrollmentsAsync(int id);
        Task<(List<StudentDto> Items, int Total)> GetStudentsAsync(QueryParameters queryParams);
        Task<StudentDto> CreateStudentAsync(CreateStudentRequest request);
        Task<StudentDto?> UpdateStudentAsync(int id, UpdateStudentRequest request);
        Task<bool> DeleteStudentAsync(int id);
    }

    /// <summary>
    /// Service interface for Course business logic
    /// </summary>
    public interface ICourseService
    {
        Task<CourseDto?> GetCourseByIdAsync(int id);
        Task<CourseDetailDto?> GetCourseByIdWithDetailsAsync(int id);
        Task<(List<CourseDto> Items, int Total)> GetCoursesAsync(QueryParameters queryParams);
        Task<List<CourseDto>> GetCoursesBySemesterAsync(int semesterId);
        Task<CourseDto> CreateCourseAsync(CreateCourseRequest request);
        Task<CourseDto?> UpdateCourseAsync(int id, UpdateCourseRequest request);
        Task<bool> DeleteCourseAsync(int id);
    }

    /// <summary>
    /// Service interface for Enrollment business logic
    /// </summary>
    public interface IEnrollmentService
    {
        Task<EnrollmentDto?> GetEnrollmentByIdAsync(int id);
        Task<EnrollmentDetailDto?> GetEnrollmentByIdWithDetailsAsync(int id);
        Task<(List<EnrollmentDto> Items, int Total)> GetEnrollmentsAsync(QueryParameters queryParams);
        Task<List<EnrollmentDto>> GetEnrollmentsByStudentAsync(int studentId);
        Task<List<EnrollmentDto>> GetEnrollmentsByCourseAsync(int courseId);
        Task<(List<EnrollmentDetailDto> Items, int Total)> GetEnrollmentsByCourseAsync(int courseId, QueryParameters queryParams);
        Task<EnrollmentDto> CreateEnrollmentAsync(CreateEnrollmentRequest request);
        Task<EnrollmentDto?> UpdateEnrollmentAsync(int id, UpdateEnrollmentRequest request);
        Task<bool> DeleteEnrollmentAsync(int id);
    }

    /// <summary>
    /// Service interface for Semester business logic
    /// </summary>
    public interface ISemesterService
    {
        Task<SemesterDto?> GetSemesterByIdAsync(int id);
        Task<SemesterDetailDto?> GetSemesterByIdWithCoursesAsync(int id);
        Task<(List<SemesterDto> Items, int Total)> GetSemestersAsync(QueryParameters queryParams);
        Task<SemesterDto> CreateSemesterAsync(CreateSemesterRequest request);
        Task<SemesterDto?> UpdateSemesterAsync(int id, UpdateSemesterRequest request);
        Task<bool> DeleteSemesterAsync(int id);
    }

    /// <summary>
    /// Service interface for Subject business logic
    /// </summary>
    public interface ISubjectService
    {
        Task<SubjectDto?> GetSubjectByIdAsync(int id);
        Task<(List<SubjectDto> Items, int Total)> GetSubjectsAsync(QueryParameters queryParams);
        Task<SubjectDto> CreateSubjectAsync(CreateSubjectRequest request);
        Task<SubjectDto?> UpdateSubjectAsync(int id, UpdateSubjectRequest request);
        Task<bool> DeleteSubjectAsync(int id);
    }
}
