using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Repositories.Models.Entities;

namespace PRN232.LMS.Repositories.Implementations
{
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(DatabaseContext context) : base(context) { }

        public async Task<Enrollment> GetEnrollmentWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.EnrollmentId == id);
        }

        public async Task<List<Enrollment>> GetEnrollmentsByStudentAsync(int studentId)
        {
            return await _dbSet
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course)
                .ToListAsync();
        }

        public async Task<List<Enrollment>> GetEnrollmentsByCourseAsync(int courseId)
        {
            return await _dbSet
                .Where(e => e.CourseId == courseId)
                .Include(e => e.Student)
                .ToListAsync();
        }

        protected override IQueryable<Enrollment> ApplySearch(IQueryable<Enrollment> query, string search)
        {
            if (string.IsNullOrEmpty(search))
                return query;

            return query.Where(e => e.Status.Contains(search));
        }

        protected override IQueryable<Enrollment> ApplySort(IQueryable<Enrollment> query, string sort)
        {
            if (string.IsNullOrEmpty(sort))
                return query.OrderBy(e => e.EnrollmentId);

            var sortFields = sort.Split(',');
            foreach (var field in sortFields)
            {
                var isDescending = field.StartsWith("-");
                var fieldName = isDescending ? field.Substring(1) : field;

                query = fieldName.ToLower() switch
                {
                    "enrolldate" => isDescending ? query.OrderByDescending(e => e.EnrollDate) : query.OrderBy(e => e.EnrollDate),
                    "status" => isDescending ? query.OrderByDescending(e => e.Status) : query.OrderBy(e => e.Status),
                    "studentid" => isDescending ? query.OrderByDescending(e => e.StudentId) : query.OrderBy(e => e.StudentId),
                    "courseid" => isDescending ? query.OrderByDescending(e => e.CourseId) : query.OrderBy(e => e.CourseId),
                    _ => query.OrderBy(e => e.EnrollmentId)
                };
            }

            return query;
        }
    }
}
