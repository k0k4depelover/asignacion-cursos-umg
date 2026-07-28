import { useState } from "react";
import "../styles/Crud.css";

function emptyValue(field) {
  if (field.type === "boolean") return false;
  return "";
}

function buildInitialState(entity, initialData) {
  const state = {};
  for (const field of entity.fields) {
    if (initialData && initialData[field.name] !== undefined && initialData[field.name] !== null) {
      state[field.name] = field.type === "date"
        ? String(initialData[field.name]).slice(0, 10)
        : initialData[field.name];
    } else {
      state[field.name] = emptyValue(field);
    }
  }
  return state;
}

export default function CrudForm({ entity, initialData, options, onSubmit, onCancel, submitting }) {
  const [values, setValues] = useState(() => buildInitialState(entity, initialData));
  const [validationError, setValidationError] = useState("");

  const isEditing = Boolean(initialData);

  const handleChange = (field, raw) => {
    let value = raw;
    if (field.type === "number") value = raw === "" ? "" : Number(raw);
    if (field.type === "decimal") value = raw === "" ? "" : Number(raw);
    if (field.type === "select" && field.fk) value = raw === "" ? "" : Number(raw);
    setValues((prev) => ({ ...prev, [field.name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    for (const field of entity.fields) {
      if (field.required && (values[field.name] === "" || values[field.name] === undefined)) {
        setValidationError(`El campo "${field.label}" es obligatorio.`);
        return;
      }
    }
    setValidationError("");
    onSubmit(values);
  };

  return (
    <div className="modal-overlay">
      <div className="modal-box modal-box--form">
        <h3 className="modal-title">
          {isEditing ? `Editar ${entity.label.toLowerCase()}` : `Nuevo ${entity.label.toLowerCase()}`}
        </h3>

        {validationError && <div className="modal-warning">{validationError}</div>}

        <form onSubmit={handleSubmit} className="crud-form">
          {entity.fields.map((field) => (
            <div className="crud-form-field" key={field.name}>
              <label>
                {field.label}
                {field.required && <span className="required-mark"> *</span>}
              </label>

              {field.type === "boolean" ? (
                <input
                  type="checkbox"
                  checked={Boolean(values[field.name])}
                  onChange={(e) => handleChange(field, e.target.checked)}
                />
              ) : field.type === "select" && field.options ? (
                <select value={values[field.name]} onChange={(e) => handleChange(field, e.target.value)}>
                  <option value="">Seleccione...</option>
                  {field.options.map((opt) => (
                    <option key={opt} value={opt}>
                      {opt}
                    </option>
                  ))}
                </select>
              ) : field.type === "select" && field.fk ? (
                <select value={values[field.name]} onChange={(e) => handleChange(field, e.target.value)}>
                  <option value="">Seleccione...</option>
                  {(options?.[field.name] || []).map((opt) => (
                    <option key={opt.value} value={opt.value}>
                      {opt.label}
                    </option>
                  ))}
                </select>
              ) : field.type === "date" ? (
                <input
                  type="date"
                  value={values[field.name]}
                  onChange={(e) => handleChange(field, e.target.value)}
                />
              ) : field.type === "time" ? (
                <input
                  type="time"
                  value={values[field.name]}
                  onChange={(e) => handleChange(field, e.target.value)}
                />
              ) : field.type === "number" || field.type === "decimal" ? (
                <input
                  type="number"
                  step={field.type === "decimal" ? "0.01" : "1"}
                  value={values[field.name]}
                  onChange={(e) => handleChange(field, e.target.value)}
                />
              ) : (
                <input
                  type="text"
                  value={values[field.name]}
                  onChange={(e) => handleChange(field, e.target.value)}
                />
              )}
            </div>
          ))}

          <div className="modal-actions">
            <button type="button" className="btn-secondary" onClick={onCancel} disabled={submitting}>
              Cancelar
            </button>
            <button type="submit" className="btn-primary" disabled={submitting}>
              {submitting ? "Guardando..." : "Guardar"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
