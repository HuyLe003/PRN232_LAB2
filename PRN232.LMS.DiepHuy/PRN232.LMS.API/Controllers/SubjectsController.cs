// ========== SubjectsController.cs ==========
using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models;
using PRN232.LMS.Repositories.Models;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _service;
        private readonly ILogger<SubjectsController> _logger;

        public SubjectsController(ISubjectService service, ILogger<SubjectsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get paginated list of subjects
        /// </summary>
        [HttpGet(Name = "GetSubjects")]
        [Produces("application/json", "application/xml")] 
        public async Task<IActionResult> GetSubjects(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sort = null,
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
                    Sort = sort
                };

                var (subjects, total) = await _service.GetSubjectsAsync(queryParams);

                var response = new PaginatedResponse<SubjectDto>
                {
                    Data = subjects,
                    Pagination = new PaginationMetadata
                    {
                        Page = page,
                        PageSize = pageSize,
                        TotalItems = total,
                        TotalPages = (total + pageSize - 1) / pageSize
                    }
                };

                return Ok(ApiResponse<PaginatedResponse<SubjectDto>>.CreateSuccess(response, "Subjects retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving subjects");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get subject by ID with Route Constraint
        /// </summary>
        [HttpGet("{id:int}", Name = "GetSubjectById")]
        [Produces("application/json", "application/xml")] 
        public async Task<IActionResult> GetSubjectById(
            [FromRoute] int id,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                var subject = await _service.GetSubjectByIdAsync(id);
                if (subject == null)
                    return NotFound(ApiResponse<string>.CreateFailure($"Subject with ID {id} not found"));

                return Ok(ApiResponse<SubjectDto>.CreateSuccess(subject));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving subject {id}");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Create a new subject
        /// </summary>
        [HttpPost(Name = "CreateSubject")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> CreateSubject(
            [FromBody] CreateSubjectRequest request,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.CreateFailure("Invalid request data", new List<string>(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))));

                var subject = await _service.CreateSubjectAsync(request);
                return CreatedAtRoute("GetSubjectById", new { id = subject.SubjectId }, ApiResponse<SubjectDto>.CreateSuccess(subject, "Subject created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating subject");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update an existing subject
        /// </summary>
        [HttpPut("{id:int}", Name = "UpdateSubject")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> UpdateSubject(
            [FromRoute] int id,
            [FromBody] UpdateSubjectRequest request,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.CreateFailure("Invalid request data", new List<string>(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))));

                var subject = await _service.UpdateSubjectAsync(id, request);
                if (subject == null)
                    return NotFound(ApiResponse<string>.CreateFailure($"Subject with ID {id} not found"));

                return Ok(ApiResponse<SubjectDto>.CreateSuccess(subject));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating subject {id}");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Delete a subject
        /// </summary>
        [HttpDelete("{id:int}", Name = "DeleteSubject")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> DeleteSubject(
            [FromRoute] int id,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                var result = await _service.DeleteSubjectAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.CreateFailure($"Subject with ID {id} not found"));

                return Ok(ApiResponse<string>.CreateSuccess("", "Subject deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting subject {id}");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred", new List<string> { ex.Message }));
            }
        }
    }
}