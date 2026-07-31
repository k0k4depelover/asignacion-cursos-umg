// src/services/api.js
// URL del backend (ajústala según corresponda)
const API_BASE_URL = 'http://localhost:5172/api';

// ============================================================
// Función base para peticiones HTTP con autenticación
// ============================================================
export const apiFetch = async (endpoint, options = {}) => {
  const token = localStorage.getItem('accessToken');
  const headers = {
    'Content-Type': 'application/json',
    ...(token && { Authorization: `Bearer ${token}` }),
    ...options.headers,
  };

  const response = await fetch(`${API_BASE_URL}${endpoint}`, {
    ...options,
    headers,
  });

  // Si el token expiró, cerrar sesión y redirigir al login
  if (response.status === 401) {
    logout();
    window.location.href = '/login';
    throw new Error('Sesión expirada. Inicia sesión nuevamente.');
  }

  return response;
};

// ============================================================
// Login
// ============================================================
export const login = async (email, password) => {
  const response = await fetch(`${API_BASE_URL}/auth/login`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ email, password }),
  });

  const data = await response.json();

  if (!response.ok) {
    throw new Error(data.message || 'Error en el inicio de sesión');
  }

  // Guardar tokens y datos del usuario en localStorage
  localStorage.setItem('accessToken', data.accessToken);
  localStorage.setItem('refreshToken', data.refreshToken);
  localStorage.setItem('user', JSON.stringify(data.user));

  return data;
};

// ============================================================
// Logout
// ============================================================
export const logout = () => {
  localStorage.removeItem('accessToken');
  localStorage.removeItem('refreshToken');
  localStorage.removeItem('user');
};

// ============================================================
// Obtener usuario actual desde localStorage
// ============================================================
export const getCurrentUser = () => {
  const user = localStorage.getItem('user');
  return user ? JSON.parse(user) : null;
};

// ============================================================
// Obtener token
// ============================================================
export const getToken = () => localStorage.getItem('accessToken');

// ============================================================
// Helpers para métodos HTTP comunes
// ============================================================
export const get = (endpoint) => apiFetch(endpoint, { method: 'GET' });

export const post = (endpoint, body) =>
  apiFetch(endpoint, {
    method: 'POST',
    body: JSON.stringify(body),
  });

export const put = (endpoint, body) =>
  apiFetch(endpoint, {
    method: 'PUT',
    body: JSON.stringify(body),
  });

export const del = (endpoint) =>
  apiFetch(endpoint, { method: 'DELETE' });