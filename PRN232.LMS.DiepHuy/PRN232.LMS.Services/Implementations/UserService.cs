using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models.Entities;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            return user == null ? null : MapToDto(user);
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var user = await _repository.GetByUsernameAsync(username);
            return user == null ? null : MapToDto(user);
        }

        /// <summary>
        /// Get all users with pagination and search
        /// </summary>
        public async Task<(List<UserDto>, int)> GetUsersAsync(QueryParameters queryParams)
        {
            var (users, total) = await _repository.GetPagedAsync(
                queryParams.Page,
                queryParams.PageSize,
                queryParams.Search,
                queryParams.Sort
            );

            var userDtos = users.Select(MapToDto).ToList();
            return (userDtos, total);
        }

        /// <summary>
        /// Create new user
        /// </summary>
        public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
        {
            // Hash password (simplified - in production use BCrypt or similar)
            var hashedPassword = HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = hashedPassword,
                Role = request.Role
            };

            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();

            return MapToDto(user);
        }

        /// <summary>
        /// Update user
        /// </summary>
        public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserRequest request)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                return null;

            if (!string.IsNullOrEmpty(request.Username))
                user.Username = request.Username;

            if (!string.IsNullOrEmpty(request.Password))
                user.PasswordHash = HashPassword(request.Password);

            if (!string.IsNullOrEmpty(request.Role))
                user.Role = request.Role;

            _repository.Update(user);
            await _repository.SaveChangesAsync();

            return MapToDto(user);
        }

        /// <summary>
        /// Delete user
        /// </summary>
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                return false;

            _repository.Delete(user);
            await _repository.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Map User entity to UserDto
        /// </summary>
        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Role = user.Role
            };
        }

        /// <summary>
        /// Simple password hashing (in production use BCrypt or ASP.NET Identity)
        /// </summary>
        private string HashPassword(string password)
        {
            // Simplified hash - in production use: 
            // BCrypt.Net.BCrypt.HashPassword(password)
            // or ASP.NET Core Identity PasswordHasher
            
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}