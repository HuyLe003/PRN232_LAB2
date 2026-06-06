using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Repositories.Models.Entities;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Implementations
{
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository _repository;

        public SemesterService(ISemesterRepository repository)
        {
            _repository = repository;
        }

        public async Task<SemesterDto> GetSemesterByIdAsync(int id)
        {
            var semester = await _repository.GetByIdAsync(id);
            if (semester == null)
                return null;

            return MapToDto(semester);
        }

        public async Task<SemesterDetailDto> GetSemesterByIdWithCoursesAsync(int id)
        {
            var semester = await _repository.GetSemesterWithCoursesAsync(id);
            if (semester == null)
                return null;

            return MapToDetailDto(semester);
        }

        public async Task<(List<SemesterDto> Items, int Total)> GetSemestersAsync(QueryParameters queryParams)
        {
            var (semesters, total) = await _repository.GetPagedAsync(
                queryParams.Page,
                queryParams.PageSize,
                queryParams.Search,
                queryParams.Sort
            );

            var dtos = semesters.Select(MapToDto).ToList();
            return (dtos, total);
        }

        public async Task<SemesterDto> CreateSemesterAsync(CreateSemesterRequest request)
        {
            var semester = new Semester
            {
                SemesterName = request.SemesterName,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            await _repository.AddAsync(semester);
            return MapToDto(semester);
        }

        public async Task<SemesterDto> UpdateSemesterAsync(int id, UpdateSemesterRequest request)
        {
            var semester = await _repository.GetByIdAsync(id);
            if (semester == null)
                return null;

            semester.SemesterName = request.SemesterName;
            semester.StartDate = request.StartDate;
            semester.EndDate = request.EndDate;

            await _repository.UpdateAsync(semester);
            return MapToDto(semester);
        }

        public async Task<bool> DeleteSemesterAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private SemesterDto MapToDto(Semester semester)
        {
            return new SemesterDto
            {
                SemesterId = semester.SemesterId,
                SemesterName = semester.SemesterName,
                StartDate = semester.StartDate,
                EndDate = semester.EndDate
            };
        }

        private SemesterDetailDto MapToDetailDto(Semester semester)
        {
            return new SemesterDetailDto
            {
                SemesterId = semester.SemesterId,
                SemesterName = semester.SemesterName,
                StartDate = semester.StartDate,
                EndDate = semester.EndDate,
                Courses = semester.Courses?.Select(c => new CourseDto
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    SemesterId = c.SemesterId
                }).ToList() ?? new List<CourseDto>()
            };
        }
    }
}
