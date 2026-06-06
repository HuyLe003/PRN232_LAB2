using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _service;
        private readonly IEnrollmentService _enrollmentService;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ICourseService service, IEnrollmentService enrollmentService, ILogger<CoursesController> logger)
        {
            _service = service;
            _enrollmentService = enrollmentService;
            _logger = logger;
        }

        /// <summary>
        /// Get paginated list of courses
        /// </summary>
        [HttpGet]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> GetCourses(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sort = null,
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
                    Expand = expand
                };

                var (courses, total) = await _service.GetCoursesAsync(queryParams);

                var response = new PaginatedResponse<CourseDto>
                {
                    Data = courses,
                    Pagination = new PaginationMetadata
                    {
                        Page = page,
                        PageSize = pageSize,
                        TotalItems = total,
                        TotalPages = (total + pageSize - 1) / pageSize
                    }
                };

                return Ok(ApiResponse<PaginatedResponse<CourseDto>>.CreateSuccess(response, "Courses retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving courses");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get course by ID
        /// </summary>
        [HttpGet("{id}")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> GetCourseById(
            [FromRoute] int id,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                var course = await _service.GetCourseByIdAsync(id);
                if (course == null)
                    return NotFound(ApiResponse<string>.CreateFailure($"Course with ID {id} not found"));

                return Ok(ApiResponse<CourseDto>.CreateSuccess(course));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving course {id}");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Create a new course
        /// </summary>
        [HttpPost]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> CreateCourse(
            [FromBody] CreateCourseRequest request,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.CreateFailure("Invalid request data", new List<string>(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))));

                var course = await _service.CreateCourseAsync(request);
                return CreatedAtAction(nameof(GetCourseById), new { id = course.CourseId }, ApiResponse<CourseDto>.CreateSuccess(course, "Course created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating course");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update an existing course
        /// </summary>
        [HttpPut("{id}")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> UpdateCourse(
            [FromRoute] int id,
            [FromBody] UpdateCourseRequest request,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.CreateFailure("Invalid request data", new List<string>(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))));

                var course = await _service.UpdateCourseAsync(id, request);
                if (course == null)
                    return NotFound(ApiResponse<string>.CreateFailure($"Course with ID {id} not found"));

                return Ok(ApiResponse<CourseDto>.CreateSuccess(course));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating course {id}");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Delete a course
        /// </summary>
        [HttpDelete("{id}")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> DeleteCourse(
            [FromRoute] int id,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                var result = await _service.DeleteCourseAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.CreateFailure($"Course with ID {id} not found"));

                return Ok(ApiResponse<string>.CreateSuccess("", "Course deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting course {id}");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }
    }
}