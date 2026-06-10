using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _service;
        private readonly ILogger<EnrollmentsController> _logger;

        public EnrollmentsController(IEnrollmentService service, ILogger<EnrollmentsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get paginated list of enrollments
        /// </summary>
        [HttpGet(Name = "GetEnrollments")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> GetEnrollments(
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

                var (enrollments, total) = await _service.GetEnrollmentsAsync(queryParams);

                var response = new PaginatedResponse<EnrollmentDto>
                {
                    Data = enrollments,
                    Pagination = new PaginationMetadata
                    {
                        Page = page,
                        PageSize = pageSize,
                        TotalItems = total,
                        TotalPages = (total + pageSize - 1) / pageSize
                    }
                };

                return Ok(ApiResponse<PaginatedResponse<EnrollmentDto>>.CreateSuccess(response, "Enrollments retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving enrollments");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get enrollment by ID with Route Constraint
        /// </summary>
        [HttpGet("{id:int}", Name = "GetEnrollmentById")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> GetEnrollmentById(
            [FromRoute] int id,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                var enrollment = await _service.GetEnrollmentByIdAsync(id);
                if (enrollment == null)
                    return NotFound(ApiResponse<string>.CreateFailure($"Enrollment with ID {id} not found"));

                return Ok(ApiResponse<EnrollmentDto>.CreateSuccess(enrollment));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving enrollment {id}");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Create a new enrollment
        /// </summary>
        [HttpPost(Name = "CreateEnrollment")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> CreateEnrollment(
            [FromBody] CreateEnrollmentRequest request,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.CreateFailure("Invalid request data", new List<string>(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))));

                var enrollment = await _service.CreateEnrollmentAsync(request);
                return CreatedAtRoute("GetEnrollmentById", new { id = enrollment.EnrollmentId }, ApiResponse<EnrollmentDto>.CreateSuccess(enrollment, "Enrollment created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating enrollment");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update an existing enrollment
        /// </summary>
        [HttpPut("{id:int}", Name = "UpdateEnrollment")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> UpdateEnrollment(
            [FromRoute] int id,
            [FromBody] UpdateEnrollmentRequest request,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.CreateFailure("Invalid request data", new List<string>(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))));

                var enrollment = await _service.UpdateEnrollmentAsync(id, request);
                if (enrollment == null)
                    return NotFound(ApiResponse<string>.CreateFailure($"Enrollment with ID {id} not found"));

                return Ok(ApiResponse<EnrollmentDto>.CreateSuccess(enrollment));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating enrollment {id}");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Delete an enrollment
        /// </summary>
        [HttpDelete("{id:int}", Name = "DeleteEnrollment")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> DeleteEnrollment(
            [FromRoute] int id,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                var result = await _service.DeleteEnrollmentAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.CreateFailure($"Enrollment with ID {id} not found"));

                return Ok(ApiResponse<string>.CreateSuccess("", "Enrollment deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting enrollment {id}");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }
    }
}