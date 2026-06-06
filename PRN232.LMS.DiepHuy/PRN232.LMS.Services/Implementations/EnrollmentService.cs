using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Repositories.Models.Entities;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Implementations
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _repository;

        public EnrollmentService(IEnrollmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<EnrollmentDto> GetEnrollmentByIdAsync(int id)
        {
            var enrollment = await _repository.GetByIdAsync(id);
            if (enrollment == null)
                return null;

            return MapToDto(enrollment);
        }

        public async Task<EnrollmentDetailDto> GetEnrollmentByIdWithDetailsAsync(int id)
        {
            var enrollment = await _repository.GetEnrollmentWithDetailsAsync(id);
            if (enrollment == null)
                return null;

            return MapToDetailDto(enrollment);
        }

        public async Task<(List<EnrollmentDto> Items, int Total)> GetEnrollmentsAsync(QueryParameters queryParams)
        {
            var (enrollments, total) = await _repository.GetPagedAsync(
                queryParams.Page,
                queryParams.PageSize,
                queryParams.Search,
                queryParams.Sort
            );

            var dtos = enrollments.Select(MapToDto).ToList();
            return (dtos, total);
        }

        public async Task<List<EnrollmentDto>> GetEnrollmentsByStudentAsync(int studentId)
        {
            var enrollments = await _repository.GetEnrollmentsByStudentAsync(studentId);
            return enrollments.Select(MapToDto).ToList();
        }

        public async Task<List<EnrollmentDto>> GetEnrollmentsByCourseAsync(int courseId)
        {
            var enrollments = await _repository.GetEnrollmentsByCourseAsync(courseId);
            return enrollments.Select(MapToDto).ToList();
        }

        public async Task<(List<EnrollmentDetailDto> Items, int Total)> GetEnrollmentsByCourseAsync(int courseId, QueryParameters queryParams)
        {
            var enrollments = await _repository.GetEnrollmentsByCourseAsync(courseId);
            
            // Apply search and sort
            var query = enrollments.AsQueryable();
            
            // Apply sort
            if (!string.IsNullOrEmpty(queryParams.Sort))
            {
                var sortFields = queryParams.Sort.Split(',');
                foreach (var field in sortFields)
                {
                    var trimmed = field.Trim();
                    var isDescending = trimmed.StartsWith("-");
                    var fieldName = isDescending ? trimmed.Substring(1) : trimmed;
                    
                    query = fieldName.ToLower() switch
                    {
                        "enrollmentid" => isDescending ? query.OrderByDescending(e => e.EnrollmentId) : query.OrderBy(e => e.EnrollmentId),
                        "enrolldate" => isDescending ? query.OrderByDescending(e => e.EnrollDate) : query.OrderBy(e => e.EnrollDate),
                        "status" => isDescending ? query.OrderByDescending(e => e.Status) : query.OrderBy(e => e.Status),
                        _ => query
                    };
                }
            }
            
            var total = query.Count();
            
            // Apply pagination
            var paginatedEnrollments = query
                .Skip((queryParams.Page - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToList();
            
            var detailDtos = paginatedEnrollments.Select(MapToDetailDto).ToList();
            return (detailDtos, total);
        }

        public async Task<EnrollmentDto> CreateEnrollmentAsync(CreateEnrollmentRequest request)
        {
            var enrollment = new Enrollment
            {
                StudentId = request.StudentId,
                CourseId = request.CourseId,
                EnrollDate = request.EnrollDate,
                Status = request.Status
            };

            await _repository.AddAsync(enrollment);
            return MapToDto(enrollment);
        }

        public async Task<EnrollmentDto> UpdateEnrollmentAsync(int id, UpdateEnrollmentRequest request)
        {
            var enrollment = await _repository.GetByIdAsync(id);
            if (enrollment == null)
                return null;

            enrollment.EnrollDate = request.EnrollDate;
            enrollment.Status = request.Status;

            await _repository.UpdateAsync(enrollment);
            return MapToDto(enrollment);
        }

        public async Task<bool> DeleteEnrollmentAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private EnrollmentDto MapToDto(Enrollment enrollment)
        {
            return new EnrollmentDto
            {
                EnrollmentId = enrollment.EnrollmentId,
                StudentId = enrollment.StudentId,
                CourseId = enrollment.CourseId,
                EnrollDate = enrollment.EnrollDate,
                Status = enrollment.Status
            };
        }

        private EnrollmentDetailDto MapToDetailDto(Enrollment enrollment)
        {
            return new EnrollmentDetailDto
            {
                EnrollmentId = enrollment.EnrollmentId,
                StudentId = enrollment.StudentId,
                CourseId = enrollment.CourseId,
                EnrollDate = enrollment.EnrollDate,
                Status = enrollment.Status,
                Student = enrollment.Student != null ? new StudentDto
                {
                    StudentId = enrollment.Student.StudentId,
                    FullName = enrollment.Student.FullName,
                    Email = enrollment.Student.Email,
                    DateOfBirth = enrollment.Student.DateOfBirth
                } : null,
                Course = enrollment.Course != null ? new CourseDto
                {
                    CourseId = enrollment.Course.CourseId,
                    CourseName = enrollment.Course.CourseName,
                    SemesterId = enrollment.Course.SemesterId
                } : null
            };
        }
    }
}
