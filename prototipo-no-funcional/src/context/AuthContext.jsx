import { createContext, useContext, useState, useEffect } from 'react';
import { getCurrentUser, logout as logoutService, getToken } from '../services/api';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    // Función para verificar si el token es válido
    const isTokenValid = () => {
        const token = getToken();
        if (!token) return false;

        try {
            // Decodificar el token (JWT)
            const payload = JSON.parse(atob(token.split('.')[1]));
            const exp = payload.exp * 1000; // en milisegundos
            return Date.now() < exp;
        } catch (error) {
            return false;
        }
    };

    // Restore session y validar token al cargar la app
    useEffect(() => {
        const currentUser = getCurrentUser();
        if (currentUser && isTokenValid()) {
            setUser(currentUser);
        } else {
            // Si el token expiró o no hay usuario, limpiar
            logoutService();
            setUser(null);
        }
        setLoading(false);
    }, []);

    // Efecto para verificar token periódicamente (cada 60 segundos)
    useEffect(() => {
        const interval = setInterval(() => {
            if (user && !isTokenValid()) {
                // Token expirado, cerrar sesión automáticamente
                logoutService();
                setUser(null);
                window.location.href = '/login'; // Redirigir forzado
            }
        }, 60000); // cada minuto

        return () => clearInterval(interval);
    }, [user]);

    const login = (userData) => {
        setUser(userData);
    };

    const logout = () => {
        logoutService();
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{ user, loading, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth debe usarse dentro de un AuthProvider');
    }
    return context;
};