import { request } from "../api/client";

const TOKEN_KEY = "auth_token";
const USER_KEY = "auth_user";

export async function login(email, password) {
  const data = await request("/Auth/login", {
    method: "POST",
    body: JSON.stringify({ email, password }),
  });

  const user = {
    id: data.user.id,
    nombre: data.user.nombre,
    email: data.user.email,
    rol: data.user.rol,
  };

  localStorage.setItem(TOKEN_KEY, data.accessToken);
  localStorage.setItem(USER_KEY, JSON.stringify(user));

  return { user, token: data.accessToken };
}

export function logout() {
  localStorage.removeItem(TOKEN_KEY);
  localStorage.removeItem(USER_KEY);
}

export function getToken() {
  return localStorage.getItem(TOKEN_KEY);
}

export function getCurrentUser() {
  const raw = localStorage.getItem(USER_KEY);
  if (!raw) return null;
  try {
    return JSON.parse(raw);
  } catch {
    return null;
  }
}
