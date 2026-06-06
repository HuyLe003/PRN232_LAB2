using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Repositories.Models.Entities;

namespace PRN232.LMS.Repositories.Implementations
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(DatabaseContext context) : base(context) { }

        public async Task<Course> GetCourseWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Semester)
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.CourseId == id);
        }

        public async Task<List<Course>> GetCoursesBySemesterAsync(int semesterId)
        {
            return await _dbSet
                .Where(c => c.SemesterId == semesterId)
                .ToListAsync();
        }

        protected override IQueryable<Course> ApplySearch(IQueryable<Course> query, string search)
        {
            if (string.IsNullOrEmpty(search))
                return query;

            return query.Where(c => c.CourseName.Contains(search));
        }

        protected override IQueryable<Course> ApplySort(IQueryable<Course> query, string sort)
        {
            if (string.IsNullOrEmpty(sort))
                return query.OrderBy(c => c.CourseId);

            var sortFields = sort.Split(',');
            foreach (var field in sortFields)
            {
                var isDescending = field.StartsWith("-");
                var fieldName = isDescending ? field.Substring(1) : field;

                query = fieldName.ToLower() switch
                {
                    "coursename" => isDescending ? query.OrderByDescending(c => c.CourseName) : query.OrderBy(c => c.CourseName),
                    "semesterid" => isDescending ? query.OrderByDescending(c => c.SemesterId) : query.OrderBy(c => c.SemesterId),
                    _ => query.OrderBy(c => c.CourseId)
                };
            }

            return query;
        }
    }
}
