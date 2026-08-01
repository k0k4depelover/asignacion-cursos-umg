using Asignacion.Web.Config;
using Asignacion.Web.Data;
using Asignacion.Web.Models;
using Asignacion.Web.Models.DTOs.Auth;
using BCrypt.Net;
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
    /// <summary>
    /// Implementación del servicio de autenticación con JWT y BCrypt
    /// </summary>
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

        /// <summary>
        /// Autentica a un usuario con correo y contraseña
        /// </summary>
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            try
            {
                // 1. Buscar usuario por correo (incluye rol)
                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.CorreoLogin == request.Email);

                if (usuario == null)
                {
                    _logger.LogWarning("Intento de login con correo inexistente: {Email}", request.Email);
                    throw new UnauthorizedAccessException("Credenciales inválidas.");
                }

                // 2. Validar estado del usuario
                if (usuario.EstadoUsuario != "activo")
                {
                    _logger.LogWarning("Usuario inactivo intentó iniciar sesión: {Email}", request.Email);
                    throw new UnauthorizedAccessException("La cuenta está inactiva. Contacta al administrador.");
                }

                // 3. Verificar contraseña con BCrypt
                if (!BCrypt.Verify(request.Password, usuario.ContrasenaHash))
                {
                    _logger.LogWarning("Contraseña incorrecta para: {Email}", request.Email);
                    throw new UnauthorizedAccessException("Credenciales inválidas.");
                }

                // 4. (Opcional) Actualizar último login si la columna existe en la BD
                // await UpdateLastLoginAsync(usuario.IdUsuario);

                // 5. Generar tokens
                var accessToken = GenerateJwtToken(usuario);
                var refreshToken = GenerateRefreshToken();

                // 6. Guardar refresh token en BD (cuando existan las columnas)
                // await SaveRefreshTokenAsync(usuario.IdUsuario, refreshToken);

                _logger.LogInformation("Login exitoso para: {Email}", request.Email);

                // 7. Construir respuesta
                return new LoginResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresIn = _jwtSettings.ExpirationMinutes * 60,
                    User = new UserInfoDto
                    {
                        Id = usuario.IdUsuario,
                        Nombre = usuario.NombreUsuario,
                        Email = usuario.CorreoLogin,
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

        /// <summary>
        /// Renueva el token de acceso usando un refresh token
        /// </summary>
        public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            // Implementación pendiente (requiere columnas de refresh token en BD)
            // Se puede implementar cuando se agreguen las columnas
            _logger.LogWarning("Refresh token solicitado pero no implementado aún.");
            throw new NotImplementedException("El refresco de token aún no está disponible.");
        }

        /// <summary>
        /// Cierra la sesión del usuario (invalida refresh token)
        /// </summary>
        public async Task<bool> LogoutAsync(int userId)
        {
            // Implementación pendiente (requiere columna refresh token)
            // var usuario = await _context.Usuarios.FindAsync(userId);
            // if (usuario == null) return false;
            // usuario.RefreshToken = null;
            // usuario.RefreshTokenExpiry = null;
            // await _context.SaveChangesAsync();
            // return true;
            _logger.LogWarning("Logout solicitado pero no implementado aún.");
            throw new NotImplementedException("El logout aún no está disponible.");
        }

        /// <summary>
        /// Valida un token JWT (firma, expiración, emisor, audiencia)
        /// </summary>
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

        // =====================================================================
        // MÉTODOS PRIVADOS
        // =====================================================================

        /// <summary>
        /// Genera un token JWT con los claims del usuario
        /// </summary>
        private string GenerateJwtToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.CorreoLogin),
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

        /// <summary>
        /// Genera un refresh token aleatorio
        /// </summary>
        private string GenerateRefreshToken()
        {
            // Usar un GUID como base para el refresh token
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        // =====================================================================
        // MÉTODOS PENDIENTES (cuando se agreguen columnas a la BD)
        // =====================================================================

        // private async Task UpdateLastLoginAsync(int userId)
        // {
        //     var usuario = await _context.Usuarios.FindAsync(userId);
        //     if (usuario != null)
        //     {
        //         // usuario.UltimoLogin = DateTime.UtcNow;
        //         // await _context.SaveChangesAsync();
        //     }
        // }

        // private async Task SaveRefreshTokenAsync(int userId, string refreshToken)
        // {
        //     var usuario = await _context.Usuarios.FindAsync(userId);
        //     if (usuario != null)
        //     {
        //         // usuario.RefreshToken = refreshToken;
        //         // usuario.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
        //         // await _context.SaveChangesAsync();
        //     }
        // }
    }
}