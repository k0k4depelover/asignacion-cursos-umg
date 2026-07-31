import { useAuth } from '../../context/AuthContext';
import '../../styles/Dashboard.css';

const DashboardCatedratico = () => {
    const { user, logout } = useAuth();

    return (
        <div className="dashboard-container">
            <div className="dashboard-header">
                <div>
                    <h1 className="dashboard-title">Panel de Catedrático</h1>
                    <p className="dashboard-subtitle">
                        {user?.nombre} · {user?.email} · Rol: {user?.rol}
                    </p>
                </div>
                <button onClick={logout} className="btn-logout">
                    Cerrar sesión
                </button>
            </div>

            <div className="dashboard-card">
                <h3>Gestión de cursos impartidos</h3>
                <p>Aquí puedes ver tus secciones, calificar estudiantes, etc.</p>
                {/* Agregar componentes específicos para catedrático */}
                <div className="profesor-grid">
                    <div className="profesor-card">Mis secciones</div>
                    <div className="profesor-card">Calificaciones</div>
                    <div className="profesor-card">Asistencia</div>
                </div>
            </div>
        </div>
    );
};

export default DashboardCatedratico;