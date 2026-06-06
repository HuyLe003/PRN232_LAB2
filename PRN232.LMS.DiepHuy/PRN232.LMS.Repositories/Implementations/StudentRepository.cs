using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Repositories.Models.Entities;

namespace PRN232.LMS.Repositories.Implementations
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(DatabaseContext context) : base(context) { }

        public async Task<Student> GetStudentWithEnrollmentsAsync(int id)
        {
            return await _dbSet
                .Include(s => s.Enrollments)
                .FirstOrDefaultAsync(s => s.StudentId == id);
        }

        public async Task<List<Student>> SearchStudentsAsync(string searchTerm, int page, int pageSize)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(s => s.FullName.Contains(searchTerm) || s.Email.Contains(searchTerm));
            }

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        protected override IQueryable<Student> ApplySearch(IQueryable<Student> query, string search)
        {
            if (string.IsNullOrEmpty(search))
                return query;

            return query.Where(s => s.FullName.Contains(search) || s.Email.Contains(search));
        }

        protected override IQueryable<Student> ApplySort(IQueryable<Student> query, string sort)
        {
            if (string.IsNullOrEmpty(sort))
                return query.OrderBy(s => s.StudentId);

            var sortFields = sort.Split(',');
            foreach (var field in sortFields)
            {
                var isDescending = field.StartsWith("-");
                var fieldName = isDescending ? field.Substring(1) : field;

                query = fieldName.ToLower() switch
                {
                    "fullname" => isDescending ? query.OrderByDescending(s => s.FullName) : query.OrderBy(s => s.FullName),
                    "email" => isDescending ? query.OrderByDescending(s => s.Email) : query.OrderBy(s => s.Email),
                    "dateofbirth" => isDescending ? query.OrderByDescending(s => s.DateOfBirth) : query.OrderBy(s => s.DateOfBirth),
                    _ => query.OrderBy(s => s.StudentId)
                };
            }

            return query;
        }
    }
}
