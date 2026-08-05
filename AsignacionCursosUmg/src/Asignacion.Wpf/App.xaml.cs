using System.Windows;
using Asignacion.Data;
using Asignacion.Services.Auth;
using Asignacion.Services.Catalogo;
using Asignacion.Services.Infraestructura;
using Asignacion.Services.Matricula;
using Asignacion.Services.Personas;
using Asignacion.Services.Programacion;
using Asignacion.Services.Reportes;
using Asignacion.Wpf.Infrastructure;
using Asignacion.Wpf.ViewModels;
using Asignacion.Wpf.ViewModels.Admin;
using Asignacion.Wpf.ViewModels.Catedratico;
using Asignacion.Wpf.ViewModels.Estudiante;
using Asignacion.Wpf.ViewModels.Shell;
using Asignacion.Wpf.Views.Common;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Asignacion.Wpf;

public partial class App : Application
{
    private IServiceProvider? _provider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var connectionString = ResolverCadenaConexion();
        if (connectionString is null)
        {
            Shutdown();
            return;
        }

        _provider = ConstruirProveedorServicios(connectionString);

        var mainWindow = _provider.GetRequiredService<MainWindow>();
        var root = _provider.GetRequiredService<RootViewModel>();
        mainWindow.DataContext = root;
        MainWindow = mainWindow;
        root.MostrarLogin();
        mainWindow.Show();
    }

    private static string? ResolverCadenaConexion()
    {
        var guardado = ConexionConfigStore.Cargar();
        if (guardado is not null)
        {
            return guardado.Value.Config.ConnectionString(guardado.Value.Password);
        }

        while (true)
        {
            var viewModel = new ConexionSetupViewModel();
            var window = new ConexionSetupWindow(viewModel);
            window.ShowDialog();

            if (!window.GuardarYContinuar)
            {
                return null;
            }

            var config = viewModel.ObtenerConfig();
            ConexionConfigStore.Guardar(config, viewModel.Password);
            return config.ConnectionString(viewModel.Password);
        }
    }

    private static IServiceProvider ConstruirProveedorServicios(string connectionString)
    {
        var services = new ServiceCollection();

        services.AddDbContextFactory<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        services.AddSingleton(WeakReferenceMessenger.Default as IMessenger);
        services.AddSingleton<CurrentSessionService>();
        services.AddSingleton<Asignacion.Wpf.ViewModels.Catedratico.CatedraticoContext>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<RootViewModel>();
        services.AddTransient<INavigationService, NavigationService>();

        // Servicios de dominio
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IFacultadService, FacultadService>();
        services.AddSingleton<ICarreraService, CarreraService>();
        services.AddSingleton<IPensumService, PensumService>();
        services.AddSingleton<ICursoService, CursoService>();
        services.AddSingleton<IEdificioService, EdificioService>();
        services.AddSingleton<ISalonService, SalonService>();
        services.AddSingleton<ILaboratorioService, LaboratorioService>();
        services.AddSingleton<IPeriodoAcademicoService, PeriodoAcademicoService>();
        services.AddSingleton<ISeccionService, SeccionService>();
        services.AddSingleton<IUsuarioService, UsuarioService>();
        services.AddSingleton<IEstudianteService, EstudianteService>();
        services.AddSingleton<ICatedraticoService, CatedraticoService>();
        services.AddSingleton<IRolService, RolService>();
        services.AddSingleton<IPermisoService, PermisoService>();
        services.AddSingleton<IRolPermisoService, RolPermisoService>();
        services.AddSingleton<IInscripcionService, InscripcionService>();
        services.AddSingleton<IAsignacionService, AsignacionService>();
        services.AddSingleton<IDetalleAsignacionService, DetalleAsignacionService>();
        services.AddSingleton<IMatriculaWorkflowService, MatriculaWorkflowService>();
        services.AddSingleton<IReporteService, ReporteService>();

        // Ventanas / ViewModels raíz
        services.AddTransient<MainWindow>();
        services.AddTransient<LoginViewModel>();

        // Shells por rol
        services.AddTransient<AdminShellViewModel>();
        services.AddTransient<EstudianteShellViewModel>();
        services.AddTransient<CatedraticoShellViewModel>();

        // Admin
        services.AddTransient<FacultadListViewModel>();
        services.AddTransient<CarreraListViewModel>();
        services.AddTransient<PensumListViewModel>();
        services.AddTransient<CursoListViewModel>();
        services.AddTransient<EdificioListViewModel>();
        services.AddTransient<SalonListViewModel>();
        services.AddTransient<LaboratorioListViewModel>();
        services.AddTransient<PeriodoAcademicoListViewModel>();
        services.AddTransient<SeccionListViewModel>();
        services.AddTransient<UsuarioListViewModel>();
        services.AddTransient<EstudianteListViewModel>();
        services.AddTransient<CatedraticoListViewModel>();
        services.AddTransient<RolListViewModel>();
        services.AddTransient<PermisoListViewModel>();
        services.AddTransient<RolPermisoMatrizViewModel>();
        services.AddTransient<AdminDashboardViewModel>();

        // Estudiante
        services.AddTransient<MisCursosViewModel>();
        services.AddTransient<InscripcionWizardViewModel>();
        services.AddTransient<MiHorarioViewModel>();
        services.AddTransient<MisCalificacionesViewModel>();
        services.AddTransient<MiPagoViewModel>();

        // Catedratico
        services.AddTransient<MisSeccionesViewModel>();
        services.AddTransient<SeccionEstudiantesViewModel>();
        services.AddTransient<CalificacionesViewModel>();

        return services.BuildServiceProvider();
    }
}
