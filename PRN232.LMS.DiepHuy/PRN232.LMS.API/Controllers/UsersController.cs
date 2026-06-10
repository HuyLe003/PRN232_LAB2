using Microsoft.AspNetCore.Mvc;
using PRN232.LMS.API.Models;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService service, ILogger<UsersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get paginated list of users
        /// </summary>
        [HttpGet(Name = "GetUsers")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> GetUsers(
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

                var (users, total) = await _service.GetUsersAsync(queryParams);

                var response = new PaginatedResponse<UserDto>
                {
                    Data = users,
                    Pagination = new PaginationMetadata
                    {
                        Page = page,
                        PageSize = pageSize,
                        TotalItems = total,
                        TotalPages = (total + pageSize - 1) / pageSize
                    }
                };

                return Ok(ApiResponse<PaginatedResponse<UserDto>>.CreateSuccess(response, "Users retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred while retrieving users", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get user by ID with Route Constraint
        /// </summary>
        [HttpGet("{id:int}", Name = "GetUserById")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> GetUserById(
            [FromRoute] int id,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                var user = await _service.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound(ApiResponse<string>.CreateFailure($"User with ID {id} not found"));

                return Ok(ApiResponse<UserDto>.CreateSuccess(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user {id}");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred while retrieving the user", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        [HttpGet("by-username/{username}", Name = "GetUserByUsername")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> GetUserByUsername(
            [FromRoute] string username,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                var user = await _service.GetUserByUsernameAsync(username);
                if (user == null)
                    return NotFound(ApiResponse<string>.CreateFailure($"User with username '{username}' not found"));

                return Ok(ApiResponse<UserDto>.CreateSuccess(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with username '{username}'");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred while retrieving the user", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        [HttpPost(Name = "CreateUser")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserRequest request,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.CreateFailure("Invalid request data", new List<string>(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))));

                var user = await _service.CreateUserAsync(request);
                return CreatedAtRoute("GetUserById", new { id = user.UserId }, ApiResponse<UserDto>.CreateSuccess(user, "User created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred while creating the user", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        [HttpPut("{id:int}", Name = "UpdateUser")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> UpdateUser(
            [FromRoute] int id,
            [FromBody] UpdateUserRequest request,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<string>.CreateFailure("Invalid request data", new List<string>(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))));

                var user = await _service.UpdateUserAsync(id, request);
                if (user == null)
                    return NotFound(ApiResponse<string>.CreateFailure($"User with ID {id} not found"));

                return Ok(ApiResponse<UserDto>.CreateSuccess(user, "User updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user {id}");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred while updating the user", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        [HttpDelete("{id:int}", Name = "DeleteUser")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> DeleteUser(
            [FromRoute] int id,
            [FromHeader(Name = "X-Request-Id")] string? requestId = null)
        {
            try
            {
                // Log request ID if provided (Header Binding)
                if (!string.IsNullOrEmpty(requestId))
                    _logger.LogInformation($"Request ID: {requestId}");

                var result = await _service.DeleteUserAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.CreateFailure($"User with ID {id} not found"));

                return Ok(ApiResponse<string>.CreateSuccess("", "User deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user {id}");
                return StatusCode(500, ApiResponse<string>.CreateFailure("An error occurred while deleting the user", new List<string> { ex.Message }));
            }
        }
    }
}