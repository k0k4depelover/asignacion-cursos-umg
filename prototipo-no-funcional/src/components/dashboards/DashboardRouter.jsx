import { Navigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';

const DashboardRouter = () => {
    const { user } = useAuth();

    if (!user) return null;

    // Mapeo de roles a rutas
    const roleRouteMap = {
        Administrador: '/dashboard/admin',
        Admin: '/dashboard/admin',
        Estudiante: '/dashboard/estudiante',
        Catedratico: '/dashboard/catedratico',
        Profesor: '/dashboard/catedratico',
    };

    const route = roleRouteMap[user.rol] || '/dashboard/estudiante';

    return <Navigate to={route} replace />;
};

export default DashboardRouter;