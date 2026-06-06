using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Repositories.Models.Entities;

namespace PRN232.LMS.Repositories.Implementations
{
    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {
        public SubjectRepository(DatabaseContext context) : base(context) { }

        public async Task<Subject> GetBySubjectCodeAsync(string code)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.SubjectCode == code);
        }

        protected override IQueryable<Subject> ApplySearch(IQueryable<Subject> query, string search)
        {
            if (string.IsNullOrEmpty(search))
                return query;

            return query.Where(s => s.SubjectCode.Contains(search) || s.SubjectName.Contains(search));
        }

        protected override IQueryable<Subject> ApplySort(IQueryable<Subject> query, string sort)
        {
            if (string.IsNullOrEmpty(sort))
                return query.OrderBy(s => s.SubjectId);

            var sortFields = sort.Split(',');
            foreach (var field in sortFields)
            {
                var isDescending = field.StartsWith("-");
                var fieldName = isDescending ? field.Substring(1) : field;

                query = fieldName.ToLower() switch
                {
                    "subjectcode" => isDescending ? query.OrderByDescending(s => s.SubjectCode) : query.OrderBy(s => s.SubjectCode),
                    "subjectname" => isDescending ? query.OrderByDescending(s => s.SubjectName) : query.OrderBy(s => s.SubjectName),
                    "credit" => isDescending ? query.OrderByDescending(s => s.Credit) : query.OrderBy(s => s.Credit),
                    _ => query.OrderBy(s => s.SubjectId)
                };
            }

            return query;
        }
    }
}
