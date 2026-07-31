using Asignacion.Web.Config;
using Asignacion.Web.Data;
using Asignacion.Web.Models;
using Asignacion.Web.Models.DTOs.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Asignacion.Web.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            AppDbContext context,
            IOptions<JwtSettings> jwtSettings,
            ILogger<AuthService> logger)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            try
            {
                // 1. Buscar usuario por correo_login (propiedad: CorreoLoginUsuario)
                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.CorreoLoginUsuario == request.Email);

                if (usuario == null)
                {
                    _logger.LogWarning("Intento de login con correo inexistente: {Email}", request.Email);
                    throw new UnauthorizedAccessException("Credenciales invalidas.");
                }

                // 2. Validar estado del usuario
                if (usuario.EstadoUsuario != "activo")
                {
                    _logger.LogWarning("Usuario inactivo intento iniciar sesion: {Email}", request.Email);
                    throw new UnauthorizedAccessException("La cuenta esta inactiva. Contacta al administrador.");
                }

                // 3. Verificar contrasena con BCrypt (nombre completo)
                if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.ContrasenaHash))
                {
                    _logger.LogWarning("Contrasena incorrecta para: {Email}", request.Email);
                    throw new UnauthorizedAccessException("Credenciales invalidas.");
                }

                // 4. Generar tokens
                var accessToken = GenerateJwtToken(usuario);
                var refreshToken = GenerateRefreshToken();

                _logger.LogInformation("Login exitoso para: {Email}", request.Email);

                // 5. Construir respuesta
                return new LoginResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresIn = _jwtSettings.ExpirationMinutes * 60,
                    User = new UserInfoDto
                    {
                        Id = usuario.IdUsuario,
                        Nombre = usuario.NombreUsuario,
                        Email = usuario.CorreoLoginUsuario,
                        Rol = usuario.Rol?.NombreRol ?? "Usuario"
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en login para: {Email}", request.Email);
                throw;
            }
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            // Pendiente de implementación cuando se agreguen las columnas de refresh token
            _logger.LogWarning("Refresh token solicitado pero no implementado aun.");
            throw new NotImplementedException("El refresco de token aun no esta disponible.");
        }

        public async Task<bool> LogoutAsync(int userId)
        {
            // Pendiente de implementación cuando se agreguen las columnas de refresh token
            _logger.LogWarning("Logout solicitado pero no implementado aun.");
            throw new NotImplementedException("El logout aun no esta disponible.");
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return await Task.FromResult(true);
            }
            catch
            {
                return false;
            }
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.CorreoLoginUsuario),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.Rol?.NombreRol ?? "Usuario"),
                new Claim("id_rol", usuario.IdRol.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}