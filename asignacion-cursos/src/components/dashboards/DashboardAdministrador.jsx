import { useAuth } from '../../context/AuthContext';
import '../../styles/Dashboard.css';

const DashboardAdministrador = () => {
    const { user, logout } = useAuth();

    return (
        <div className="dashboard-container">
            <div className="dashboard-header">
                <div>
                    <h1 className="dashboard-title">Panel de Administrador</h1>
                    <p className="dashboard-subtitle">
                        {user?.nombre} · {user?.email} · Rol: {user?.rol}
                    </p>
                </div>
                <button onClick={logout} className="btn-logout">
                    Cerrar sesión
                </button>
            </div>

            <div className="dashboard-card">
                <h3>Gestión del sistema</h3>
                <p>Aquí puedes administrar usuarios, cursos, periodos, etc.</p>
                {/* Agregar componentes específicos para administrador */}
                <div className="admin-grid">
                    <div className="admin-card">Usuarios</div>
                    <div className="admin-card">Cursos</div>
                    <div className="admin-card">Periodos</div>
                    <div className="admin-card">Reportes</div>
                </div>
            </div>
        </div>
    );
};

export default DashboardAdministrador;