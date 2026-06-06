using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Repositories.Models.Entities;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Implementations
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _repository;

        public SubjectService(ISubjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<SubjectDto> GetSubjectByIdAsync(int id)
        {
            var subject = await _repository.GetByIdAsync(id);
            if (subject == null)
                return null;

            return MapToDto(subject);
        }

        public async Task<(List<SubjectDto> Items, int Total)> GetSubjectsAsync(QueryParameters queryParams)
        {
            var (subjects, total) = await _repository.GetPagedAsync(
                queryParams.Page,
                queryParams.PageSize,
                queryParams.Search,
                queryParams.Sort
            );

            var dtos = subjects.Select(MapToDto).ToList();
            return (dtos, total);
        }

        public async Task<SubjectDto> CreateSubjectAsync(CreateSubjectRequest request)
        {
            var subject = new Subject
            {
                SubjectCode = request.SubjectCode,
                SubjectName = request.SubjectName,
                Credit = request.Credit
            };

            await _repository.AddAsync(subject);
            return MapToDto(subject);
        }

        public async Task<SubjectDto> UpdateSubjectAsync(int id, UpdateSubjectRequest request)
        {
            var subject = await _repository.GetByIdAsync(id);
            if (subject == null)
                return null;

            subject.SubjectCode = request.SubjectCode;
            subject.SubjectName = request.SubjectName;
            subject.Credit = request.Credit;

            await _repository.UpdateAsync(subject);
            return MapToDto(subject);
        }

        public async Task<bool> DeleteSubjectAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private SubjectDto MapToDto(Subject subject)
        {
            return new SubjectDto
            {
                SubjectId = subject.SubjectId,
                SubjectCode = subject.SubjectCode,
                SubjectName = subject.SubjectName,
                Credit = subject.Credit
            };
        }
    }
}
