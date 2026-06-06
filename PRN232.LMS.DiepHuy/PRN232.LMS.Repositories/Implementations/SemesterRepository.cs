using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Repositories.Models.Entities;

namespace PRN232.LMS.Repositories.Implementations
{
    public class SemesterRepository : Repository<Semester>, ISemesterRepository
    {
        public SemesterRepository(DatabaseContext context) : base(context) { }

        public async Task<Semester> GetSemesterWithCoursesAsync(int id)
        {
            return await _dbSet
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.SemesterId == id);
        }

        protected override IQueryable<Semester> ApplySearch(IQueryable<Semester> query, string search)
        {
            if (string.IsNullOrEmpty(search))
                return query;

            return query.Where(s => s.SemesterName.Contains(search));
        }

        protected override IQueryable<Semester> ApplySort(IQueryable<Semester> query, string sort)
        {
            if (string.IsNullOrEmpty(sort))
                return query.OrderBy(s => s.SemesterId);

            var sortFields = sort.Split(',');
            foreach (var field in sortFields)
            {
                var isDescending = field.StartsWith("-");
                var fieldName = isDescending ? field.Substring(1) : field;

                query = fieldName.ToLower() switch
                {
                    "semestername" => isDescending ? query.OrderByDescending(s => s.SemesterName) : query.OrderBy(s => s.SemesterName),
                    "startdate" => isDescending ? query.OrderByDescending(s => s.StartDate) : query.OrderBy(s => s.StartDate),
                    "enddate" => isDescending ? query.OrderByDescending(s => s.EndDate) : query.OrderBy(s => s.EndDate),
                    _ => query.OrderBy(s => s.SemesterId)
                };
            }

            return query;
        }
    }
}
