import { useState } from "react";
import "../styles/FakeLogin.css";

export default function FakeLogin({ onLogin }) {
  const [email, setEmail] = useState("eaguila2@umg.edu.gt");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = (e) => {
    e.preventDefault();
    setLoading(true);
    setTimeout(() => {
      onLogin({ email, name: "Eduardo del Aguila" });
    }, 500);
  };

  return (
    <div className="login-page">
      <form onSubmit={handleSubmit} className="login-container">
        <h2 className="login-title">Asignación de Cursos UMG</h2>
        <p className="login-subtitle">Inicia sesión para continuar</p>

        <label className="login-label" htmlFor="email">Correo</label>
        <input
          id="email"
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="login-input"
          required
        />

        <label className="login-label" htmlFor="password">Contraseña</label>
        <input
          id="password"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className="login-input"
          required
        />

        <button type="submit" disabled={loading} className="login-button">
          {loading ? "Ingresando..." : "Entrar"}
        </button>
      </form>
    </div>
  );
}
