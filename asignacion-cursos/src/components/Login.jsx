import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { login } from '../services/api';
import { useAuth } from '../context/AuthContext';
import '../styles/Login.css';

export default function Login() {
    const { login: authLogin } = useAuth();
    const navigate = useNavigate();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setLoading(true);

        if (!email || !password) {
            setError('Todos los campos son obligatorios.');
            setLoading(false);
            return;
        }

        try {
            const response = await login(email, password);
            authLogin(response.user);
            // Redirigir al dashboard después del login
            navigate('/dashboard', { replace: true });
        } catch (err) {
            const message = err.message;
            if (message.includes('Credenciales inválidas')) {
                setError('Usuario o contraseña incorrectos.');
            } else if (message.includes('inactiva')) {
                setError('La cuenta está inactiva. Contacta al administrador.');
            } else if (message.includes('Sesión expirada')) {
                setError('Tu sesión ha expirado. Inicia sesión nuevamente.');
            } else {
                setError('Ocurrió un error inesperado. Intenta más tarde.');
            }
        } finally {
            setLoading(false);
        }
    };

    return (
        <form onSubmit={handleSubmit} className="login-container">
            <h2 className="login-title">Iniciar Sesión</h2>

            {error && <div className="login-error">{error}</div>}

            <input
                type="email"
                placeholder="Correo electrónico"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className="login-input"
                disabled={loading}
                required
            />

            <input
                type="password"
                placeholder="Contraseña"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="login-input"
                disabled={loading}
                required
            />

            <button type="submit" className="login-button" disabled={loading}>
                {loading ? 'Ingresando...' : 'Entrar'}
            </button>
        </form>
    );
}