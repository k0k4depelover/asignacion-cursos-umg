using Asignacion.Web.Models.DTOs.Auth;
using System.Threading.Tasks;

namespace Asignacion.Web.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
        Task<bool> LogoutAsync(int userId);
        Task<bool> ValidateTokenAsync(string token);
    }
}