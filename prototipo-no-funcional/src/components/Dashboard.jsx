import { useState } from "react";
import CourseModal from "./CourseModal";
import "../styles/Dashboard.css";

export default function Dashboard({ user }) {
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
        <h1 className="dashboard-title">Bienvenido, {user.name} 👋</h1>
        <p className="dashboard-subtitle">Panel de asignación de cursos</p>
      </div>

      <div className="dashboard-card">
        <div className="dashboard-card-header">
          <h3 className="dashboard-card-title">Cursos asignados</h3>
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
}