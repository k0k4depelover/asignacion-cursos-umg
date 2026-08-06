import { NavLink, Outlet, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { navGroups, schema } from "../entities/schema";
import "../styles/AppLayout.css";

export default function AppLayout() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return (
    <div className="app-shell">
      <aside className="app-sidebar">
        <div className="app-sidebar-brand">
          <span className="app-sidebar-logo">🎓</span>
          <span>Asignación de Cursos</span>
        </div>

        <nav className="app-nav">
          <NavLink to="." end className={({ isActive }) => (isActive ? "app-nav-link app-nav-link--active" : "app-nav-link")}>
            Inicio
          </NavLink>

          {navGroups.map((group) => (
            <div className="app-nav-group" key={group.title}>
              <div className="app-nav-group-title">{group.title}</div>
              {group.items.map((key) => (
                <NavLink
                  key={key}
                  to={`modulo/${key}`}
                  className={({ isActive }) => (isActive ? "app-nav-link app-nav-link--active" : "app-nav-link")}
                >
                  {schema[key].labelPlural}
                </NavLink>
              ))}
            </div>
          ))}
        </nav>
      </aside>

      <div className="app-main">
        <header className="app-topbar">
          <span>Bienvenido, {user?.nombre}</span>
          <button className="btn-secondary btn-sm" onClick={handleLogout}>
            Cerrar sesión
          </button>
        </header>
        <main className="app-content">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
