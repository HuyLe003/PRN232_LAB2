using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Repositories.Models.Entities;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        public async Task<StudentDto> GetStudentByIdAsync(int id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
                return null;

            return MapToDto(student);
        }

        public async Task<StudentDetailDto> GetStudentByIdWithEnrollmentsAsync(int id)
        {
            var student = await _repository.GetStudentWithEnrollmentsAsync(id);
            if (student == null)
                return null;

            return MapToDetailDto(student);
        }

        public async Task<(List<StudentDto> Items, int Total)> GetStudentsAsync(QueryParameters queryParams)
        {
            var (students, total) = await _repository.GetPagedAsync(
                queryParams.Page,
                queryParams.PageSize,
                queryParams.Search,
                queryParams.Sort
            );

            var dtos = students.Select(MapToDto).ToList();
            return (dtos, total);
        }

        public async Task<StudentDto> CreateStudentAsync(CreateStudentRequest request)
        {
            var student = new Student
            {
                FullName = request.FullName,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth
            };

            await _repository.AddAsync(student);
            return MapToDto(student);
        }

        public async Task<StudentDto> UpdateStudentAsync(int id, UpdateStudentRequest request)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
                return null;

            student.FullName = request.FullName;
            student.Email = request.Email;
            student.DateOfBirth = request.DateOfBirth;

            await _repository.UpdateAsync(student);
            return MapToDto(student);
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private StudentDto MapToDto(Student student)
        {
            return new StudentDto
            {
                StudentId = student.StudentId,
                FullName = student.FullName,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth
            };
        }

        private StudentDetailDto MapToDetailDto(Student student)
        {
            return new StudentDetailDto
            {
                StudentId = student.StudentId,
                FullName = student.FullName,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth,
                Enrollments = student.Enrollments?.Select(e => new EnrollmentDto
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
