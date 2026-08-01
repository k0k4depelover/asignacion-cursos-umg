using Asignacion.Web.Config;
using Asignacion.Web.Data;
using Asignacion.Web.Services;
using Asignacion.Web.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =====================================================================
// 1. CONFIGURACIÓN DE SERVICIOS
// =====================================================================

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// =====================================================================
// 2. CONFIGURACIÓN JWT
// =====================================================================
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);

var secret = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret no configurado en appsettings.json");
var key = Encoding.UTF8.GetBytes(secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// =====================================================================
// 3. CORS (permitir React en desarrollo)
// =====================================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5172",
                "https://localhost:5172",
                "http://localhost:5173",
                "https://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// =====================================================================
// 4. BASE DE DATOS (MySQL con Pomelo)
// =====================================================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 32))
    ));

// =====================================================================
// 5. REGISTRO DE SERVICIOS (Scoped)
// =====================================================================

// --- Servicios de autenticación ---
builder.Services.AddScoped<IAuthService, AuthService>();

// --- Servicios de negocio ---
builder.Services.AddScoped<IAsignacionService, AsignacionService>();
builder.Services.AddScoped<ICarreraService, CarreraService>();
builder.Services.AddScoped<ICatedraticoService, CatedraticoService>();
builder.Services.AddScoped<ICursoService, CursoService>();
builder.Services.AddScoped<IDetalleAsignacionService, DetalleAsignacionService>();
builder.Services.AddScoped<IEdificioService, EdificioService>();
builder.Services.AddScoped<IEstudianteService, EstudianteService>();
builder.Services.AddScoped<IFacultadService, FacultadService>();
builder.Services.AddScoped<IHorarioSeccionService, HorarioSeccionService>();
builder.Services.AddScoped<IInscripcionService, InscripcionService>();
builder.Services.AddScoped<ILaboratorioService, LaboratorioService>();
builder.Services.AddScoped<IPensumService, PensumService>();
builder.Services.AddScoped<IPensumCursoService, PensumCursoService>();
builder.Services.AddScoped<IPeriodoAcademicoService, PeriodoAcademicoService>();
builder.Services.AddScoped<IPermisoService, PermisoService>();
builder.Services.AddScoped<IRequisitoCursoService, RequisitoCursoService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<ISalonService, SalonService>();
builder.Services.AddScoped<ISeccionService, SeccionService>();
builder.Services.AddScoped<ISeccionLaboratorioService, SeccionLaboratorioService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// =====================================================================
// 6. CONSTRUCCIÓN DE LA APLICACIÓN
// =====================================================================

var app = builder.Build();

// =====================================================================
// 7. PIPELINE DE PETICIONES HTTP
// =====================================================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Orden correcto: CORS → Autenticación → Autorización → Controladores
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// =====================================================================
// 8. EJECUCIÓN
// =====================================================================

app.Run();
