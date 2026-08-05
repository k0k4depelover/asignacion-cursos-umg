import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';
import '../styles/AccessDenied.css';

const AccessDenied = () => {
    const { logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    return (
        <div className="access-denied-container">
            <div className="access-denied-card">
                <span className="access-denied-icon">🚫</span>
                <h1>Acceso Denegado</h1>
                <p>No tienes permisos para acceder a esta sección.</p>
                <button onClick={handleLogout} className="btn-primary">
                    Cerrar sesión
                </button>
            </div>
        </div>
    );
};

export default AccessDenied;