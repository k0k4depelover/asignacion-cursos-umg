import "../styles/Crud.css";

function formatCell(value, field) {
  if (value === null || value === undefined || value === "") return "—";
  if (field.type === "boolean") return value ? "Sí" : "No";
  if (field.type === "date") return String(value).slice(0, 10);
  return String(value);
}

export default function CrudTable({ entity, rows, resolvedRows, loading, error, onEdit, onDelete }) {
  const columns = entity.fields.filter((f) => !f.hideInTable);

  if (loading) {
    return <div className="crud-state">Cargando {entity.labelPlural.toLowerCase()}...</div>;
  }

  if (error) {
    return <div className="crud-state crud-state--error">{error}</div>;
  }

  if (!rows.length) {
    return (
      <div className="crud-state">
        <span className="empty-icon">📋</span>
        <p>No hay registros de {entity.labelPlural.toLowerCase()} todavía.</p>
      </div>
    );
  }

  return (
    <div className="crud-table-wrap">
      <table className="crud-table">
        <thead>
          <tr>
            {columns.map((f) => (
              <th key={f.name}>{f.label}</th>
            ))}
            <th className="crud-table-actions-col">Acciones</th>
          </tr>
        </thead>
        <tbody>
          {rows.map((row, idx) => {
            const resolved = resolvedRows?.[idx] || row;
            const key = entity.idField ? row[entity.idField] : idx;
            return (
              <tr key={key}>
                {columns.map((f) => (
                  <td key={f.name}>{formatCell(resolved[f.name], f)}</td>
                ))}
                <td className="crud-table-actions-col">
                  <button className="btn-secondary btn-sm" onClick={() => onEdit(row)}>
                    Editar
                  </button>
                  <button className="btn-danger btn-sm" onClick={() => onDelete(row)}>
                    Eliminar
                  </button>
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}
