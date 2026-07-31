import { useState } from 'react';
import { useAuth } from '../../context/AuthContext';
import CourseModal from '../CourseModal';
import '../../styles/Dashboard.css';

const DashboardEstudiante = () => {
    const { user, logout } = useAuth();
    const [cursosAsignados, setCursosAsignados] = useState([]);
    const [mostrarModal, setMostrarModal] = useState(false);

    const handleAssign = (seleccionados) => {
        setCursosAsignados((prev) => {
            const nuevos = seleccionados.filter(
                (s) => !prev.some((c) => c.id === s.id)
            );
            return [...prev, ...nuevos];
        });
        setMostrarModal(false);
    };

    return (
        <div className="dashboard-container">
            <div className="dashboard-header">
                <div>
                    <h1 className="dashboard-title">Panel de Estudiante</h1>
                    <p className="dashboard-subtitle">
                        {user?.nombre} · {user?.email} · Rol: {user?.rol}
                    </p>
                </div>
                <button onClick={logout} className="btn-logout">
                    Cerrar sesión
                </button>
            </div>

            <div className="dashboard-card">
                <div className="dashboard-card-header">
                    <h3 className="dashboard-card-title">Mis cursos</h3>
                    <button className="btn-primary" onClick={() => setMostrarModal(true)}>
                        + Asignar cursos
                    </button>
                </div>

                {cursosAsignados.length === 0 ? (
                    <div className="empty-state">
                        <span className="empty-icon">📚</span>
                        <p>Todavía no asignaste ningún curso.</p>
                    </div>
                ) : (
                    <ul className="course-list">
                        {cursosAsignados.map((curso) => (
                            <li key={curso.id} className="course-item">
                                <span className="course-dot" />
                                {curso.nombre}
                            </li>
                        ))}
                    </ul>
                )}
            </div>

            {mostrarModal && (
                <CourseModal
                    onClose={() => setMostrarModal(false)}
                    onAssign={handleAssign}
                    cursosYaAsignados={cursosAsignados}
                />
            )}
        </div>
    );
};

export default DashboardEstudiante;