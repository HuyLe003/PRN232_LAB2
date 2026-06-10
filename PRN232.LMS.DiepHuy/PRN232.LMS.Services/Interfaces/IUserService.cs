using System.Threading.Tasks;
using PRN232.LMS.Services.Models;

namespace PRN232.LMS.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<(List<UserDto>, int)> GetUsersAsync(QueryParameters queryParams);
        Task<UserDto> CreateUserAsync(CreateUserRequest request);
        Task<UserDto?> UpdateUserAsync(int id, UpdateUserRequest request);
        Task<bool> DeleteUserAsync(int id);
    }
}