using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Repositories.Models.Entities;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repository;
        private readonly ISemesterRepository _semesterRepository;

        public CourseService(ICourseRepository repository, ISemesterRepository semesterRepository)
        {
            _repository = repository;
            _semesterRepository = semesterRepository;
        }

        public async Task<CourseDto> GetCourseByIdAsync(int id)
        {
            var course = await _repository.GetByIdAsync(id);
            if (course == null)
                return null;

            return MapToDto(course);
        }

        public async Task<CourseDetailDto> GetCourseByIdWithDetailsAsync(int id)
        {
            var course = await _repository.GetCourseWithDetailsAsync(id);
            if (course == null)
                return null;

            return MapToDetailDto(course);
        }

        public async Task<(List<CourseDto> Items, int Total)> GetCoursesAsync(QueryParameters queryParams)
        {
            var (courses, total) = await _repository.GetPagedAsync(
                queryParams.Page,
                queryParams.PageSize,
                queryParams.Search,
                queryParams.Sort
            );

            var dtos = courses.Select(MapToDto).ToList();
            return (dtos, total);
        }

        public async Task<List<CourseDto>> GetCoursesBySemesterAsync(int semesterId)
        {
            var courses = await _repository.GetCoursesBySemesterAsync(semesterId);
            return courses.Select(MapToDto).ToList();
        }

        public async Task<CourseDto> CreateCourseAsync(CreateCourseRequest request)
        {
            var course = new Course
            {
                CourseName = request.CourseName,
                SemesterId = request.SemesterId
            };

            await _repository.AddAsync(course);
            return MapToDto(course);
        }

        public async Task<CourseDto> UpdateCourseAsync(int id, UpdateCourseRequest request)
        {
            var course = await _repository.GetByIdAsync(id);
            if (course == null)
                return null;

            course.CourseName = request.CourseName;
            course.SemesterId = request.SemesterId;

            await _repository.UpdateAsync(course);
            return MapToDto(course);
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private CourseDto MapToDto(Course course)
        {
            return new CourseDto
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                SemesterId = course.SemesterId
            };
        }

        private CourseDetailDto MapToDetailDto(Course course)
        {
            return new CourseDetailDto
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                SemesterId = course.SemesterId,
                Semester = course.Semester != null ? new SemesterDto
                {
                    SemesterId = course.Semester.SemesterId,
                    SemesterName = course.Semester.SemesterName,
                    StartDate = course.Semester.StartDate,
                    EndDate = course.Semester.EndDate
                } : null,
                Enrollments = course.Enrollments?.Select(e => new EnrollmentDto
                {
                    EnrollmentId = e.EnrollmentId,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    EnrollDate = e.EnrollDate,
                    Status = e.Status
                }).ToList() ?? new List<EnrollmentDto>()
            };
        }
    }
}
