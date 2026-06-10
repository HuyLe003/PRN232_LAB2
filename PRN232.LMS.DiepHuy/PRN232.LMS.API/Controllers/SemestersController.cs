// ========== SemestersController.cs ==========
using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SemestersController : ControllerBase
    {
        private readonly ISemesterService _service;
        private readonly ILogger<SemestersController> _logger;

        public SemestersController(ISemesterService service, ILogger<SemestersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get paginated list of semesters
        /// </summary>
        [HttpGet(Name = "GetSemesters")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> GetSemesters(
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

                var (semesters, total) = await _service.GetSemestersAsync(queryParams);

                var response = new PaginatedResponse<SemesterDto>
                {
                    Data = semesters,
                    Pagination = new PaginationMetadata
                    {
                        Page = page,
                        PageSize = pageSize,
                        TotalItems = total,
                        TotalPages = (total + pageSize - 1) / pageSize
                    }
                };

                return Ok(ApiResponse<PaginatedResponse<SemesterDto>>.CreateSuccess(response, "Semesters retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving semesters");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get semester by ID with Route Constraint
        /// </summary>
        [HttpGet("{id:int}", Name = "GetSemesterById")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> GetSemesterById(
            [FromRoute] int id,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                var semester = await _service.GetSemesterByIdAsync(id);
                if (semester == null)
                    return NotFound(ApiResponse<string>.CreateFailure($"Semester with ID {id} not found"));

                return Ok(ApiResponse<SemesterDto>.CreateSuccess(semester));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving semester {id}");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Create a new semester
        /// </summary>
        [HttpPost(Name = "CreateSemester")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> CreateSemester(
            [FromBody] CreateSemesterRequest request,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.CreateFailure("Invalid request data", new List<string>(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))));

                var semester = await _service.CreateSemesterAsync(request);
                return CreatedAtRoute("GetSemesterById", new { id = semester.SemesterId }, ApiResponse<SemesterDto>.CreateSuccess(semester, "Semester created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating semester");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update an existing semester
        /// </summary>
        [HttpPut("{id:int}", Name = "UpdateSemester")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> UpdateSemester(
            [FromRoute] int id,
            [FromBody] UpdateSemesterRequest request,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.CreateFailure("Invalid request data", new List<string>(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))));

                var semester = await _service.UpdateSemesterAsync(id, request);
                if (semester == null)
                    return NotFound(ApiResponse<string>.CreateFailure($"Semester with ID {id} not found"));

                return Ok(ApiResponse<SemesterDto>.CreateSuccess(semester));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating semester {id}");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Delete a semester
        /// </summary>
        [HttpDelete("{id:int}", Name = "DeleteSemester")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> DeleteSemester(
            [FromRoute] int id,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                var result = await _service.DeleteSemesterAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.CreateFailure($"Semester with ID {id} not found"));

                return Ok(ApiResponse<string>.CreateSuccess("", "Semester deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting semester {id}");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }
    }
}