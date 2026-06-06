using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _service;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentService service, ILogger<StudentsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get paginated list of students with search, sort, and filter support
        /// </summary>
        [HttpGet]
        [Produces("application/json", "application/xml")] 
        public async Task<IActionResult> GetStudents(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sort = null,
            [FromQuery] string? fields = null,
            [FromQuery] string? expand = null,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                var queryParams = new QueryParameters
                {
                    Page = page,
                    PageSize = pageSize,
                    Search = search,
                    Sort = sort,
                    Fields = fields,
                    Expand = expand
                };

                var (students, total) = await _service.GetStudentsAsync(queryParams);

                var response = new PaginatedResponse<StudentDto>
                {
                    Data = students,
                    Pagination = new PaginationMetadata
                    {
                        Page = page,
                        PageSize = pageSize,
                        TotalItems = total,
                        TotalPages = (total + pageSize - 1) / pageSize
                    }
                };

                return Ok(ApiResponse<PaginatedResponse<StudentDto>>.CreateSuccess(response, "Students retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving students");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred while retrieving students", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get student by ID
        /// </summary>
        [HttpGet("{id}")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> GetStudentById(
            [FromRoute] int id,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                var student = await _service.GetStudentByIdAsync(id);
                if (student == null)
                    return NotFound(ApiResponse<string>.CreateFailure($"Student with ID {id} not found"));

                return Ok(ApiResponse<StudentDto>.CreateSuccess(student));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving student {id}");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred while retrieving the student", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Create a new student
        /// </summary>
        [HttpPost]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> CreateStudent(
            [FromBody] CreateStudentRequest request,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.CreateFailure("Invalid request data", new List<string>(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))));

                var student = await _service.CreateStudentAsync(request);
                return CreatedAtAction(nameof(GetStudentById), new { id = student.StudentId }, ApiResponse<StudentDto>.CreateSuccess(student, "Student created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating student");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred while creating the student", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update an existing student
        /// </summary>
        [HttpPut("{id}")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> UpdateStudent(
            [FromRoute] int id,
            [FromBody] UpdateStudentRequest request,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.CreateFailure("Invalid request data", new List<string>(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))));

                var student = await _service.UpdateStudentAsync(id, request);
                if (student == null)
                    return NotFound(ApiResponse<string>.CreateFailure($"Student with ID {id} not found"));

                return Ok(ApiResponse<StudentDto>.CreateSuccess(student, "Student updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating student {id}");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred while updating the student", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Delete a student
        /// </summary>
        [HttpDelete("{id}")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> DeleteStudent(
            [FromRoute] int id,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                var result = await _service.DeleteStudentAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.CreateFailure($"Student with ID {id} not found"));

                return Ok(ApiResponse<string>.CreateSuccess("", "Student deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting student {id}");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred while deleting the student", new List<string> { ex.Message }));
            }
        }
    }
}