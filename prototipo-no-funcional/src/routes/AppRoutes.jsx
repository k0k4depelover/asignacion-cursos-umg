import { Routes, Route, Navigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import Login from '../components/Login';
import DashboardRouter from '../components/dashboards/DashboardRouter';
import ProtectedRoute from '../components/ProtectedRoute';
import RoleGuard from '../components/RoleGuard';
import AccessDenied from '../components/AccessDenied';

// Importar los dashboards específicos
import DashboardAdministrador from '../components/dashboards/DashboardAdministrador';
import DashboardEstudiante from '../components/dashboards/DashboardEstudiante';
import DashboardCatedratico from '../components/dashboards/DashboardCatedratico';

const AppRoutes = () => {
    const { user, loading } = useAuth();

    if (loading) {
        return <div className="loading-screen">Cargando...</div>;
    }

    return (
        <Routes>
            <Route path="/login" element={<Login />} />
            <Route path="/acceso-denegado" element={<AccessDenied />} />

            {/* Rutas protegidas con roles específicos */}
            <Route
                path="/dashboard/admin/*"
                element={
                    <ProtectedRoute>
                        <RoleGuard allowedRoles={['Administrador', 'Admin']}>
                            <DashboardAdministrador />
                        </RoleGuard>
                    </ProtectedRoute>
                }
            />

            <Route
                path="/dashboard/estudiante/*"
                element={
                    <ProtectedRoute>
                        <RoleGuard allowedRoles={['Estudiante']}>
                            <DashboardEstudiante />
                        </RoleGuard>
                    </ProtectedRoute>
                }
            />

            <Route
                path="/dashboard/catedratico/*"
                element={
                    <ProtectedRoute>
                        <RoleGuard allowedRoles={['Catedratico', 'Profesor']}>
                            <DashboardCatedratico />
                        </RoleGuard>
                    </ProtectedRoute>
                }
            />

            {/* Ruta principal del dashboard que redirige según rol */}
            <Route
                path="/dashboard"
                element={
                    <ProtectedRoute>
                        <DashboardRouter />
                    </ProtectedRoute>
                }
            />

            <Route path="/" element={<Navigate to="/dashboard" replace />} />
            <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
    );
};

export default AppRoutes;