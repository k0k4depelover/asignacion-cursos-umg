import { useState } from "react";
import { cursos_disponibles } from "../data/cursosDisponibles";
import "../styles/CourseModal.css";

const MAX_CURSOS = 5;

export default function CourseModal({ onClose, onAssign, cursosYaAsignados = [] }) {
  const [seleccionados, setSeleccionados] = useState([]);

  const totalSiAcepta = cursosYaAsignados.length + seleccionados.length;
  const hayTraslape = totalSiAcepta > MAX_CURSOS;

  const toggleCurso = (curso) => {
    setSeleccionados((prev) => {
      const yaEsta = prev.some((c) => c.id === curso.id);
      if (yaEsta) return prev.filter((c) => c.id !== curso.id);
      if (prev.length >= MAX_CURSOS) return prev; // bloquea al llegar al límite local
      return [...prev, curso];
    });
  };

  return (
    <div className="modal-overlay">
      <div className="modal-box">
        <h3 className="modal-title">Selecciona los cursos</h3>
        <p className="modal-counter">
          Seleccionados: <strong>{seleccionados.length}</strong> / {MAX_CURSOS}
        </p>

        {hayTraslape && (
          <div className="modal-warning">
            ⚠️ Superás el límite de {MAX_CURSOS} cursos. Deseleccioná alguno para continuar.
          </div>
        )}

        <ul className="modal-list">
          {cursos_disponibles.map((curso) => {
            const estaSeleccionado = seleccionados.some((c) => c.id === curso.id);
            const estaAsignado = cursosYaAsignados.some((c) => c.id === curso.id);
            const bloqueado = !estaSeleccionado && seleccionados.length >= MAX_CURSOS;

            return (
              <li
                key={curso.id}
                className={`modal-item ${estaAsignado ? "modal-item--assigned" : ""} ${bloqueado ? "modal-item--blocked" : ""}`}
              >
                <label className="modal-label">
                  <input
                    type="checkbox"
                    checked={estaSeleccionado}
                    disabled={estaAsignado || bloqueado}
                    onChange={() => toggleCurso(curso)}
                  />
                  <span>{curso.nombre}</span>
                  {estaAsignado && <span className="badge-assigned">Ya asignado</span>}
                </label>
              </li>
            );
          })}
        </ul>

        <div className="modal-actions">
          <button className="btn-secondary" onClick={onClose}>Cancelar</button>
          <button
            className="btn-primary"
            disabled={seleccionados.length === 0 || hayTraslape}
            onClick={() => onAssign(seleccionados)}
          >
            Aceptar
          </button>
        </div>
      </div>
    </div>
  );
}