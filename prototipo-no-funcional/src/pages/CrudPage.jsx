import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { createResourceClient, ApiError } from "../api/client";
import { schema } from "../entities/schema";
import CrudTable from "../components/CrudTable";
import CrudForm from "../components/CrudForm";
import "../styles/Crud.css";

const listCache = {};

async function fetchEntityList(entityKey) {
  if (listCache[entityKey]) return listCache[entityKey];
  const client = createResourceClient(schema[entityKey].resource);
  const data = await client.list();
  listCache[entityKey] = data;
  return data;
}

export default function CrudPage() {
  const { entityKey } = useParams();
  const entity = schema[entityKey];
  const client = createResourceClient(entity.resource);
  const fkFields = entity.fields.filter((f) => f.type === "select" && f.fk);

  const [rows, setRows] = useState([]);
  const [resolvedRows, setResolvedRows] = useState([]);
  const [fkOptions, setFkOptions] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [editing, setEditing] = useState(null);
  const [submitting, setSubmitting] = useState(false);
  const [banner, setBanner] = useState(null);

  async function loadFkOptions() {
    const options = {};
    for (const field of fkFields) {
      const fkEntity = schema[field.fk];
      try {
        const list = await fetchEntityList(field.fk);
        const idKey = fkEntity.idField;
        options[field.name] = list.map((item) => ({
          value: item[idKey],
          label: fkEntity.displayField ? String(item[fkEntity.displayField]) : "#" + item[idKey],
        }));
      } catch {
        options[field.name] = [];
      }
    }
    return options;
  }

  function resolveRowsForDisplay(data, options) {
    return data.map((row) => {
      const resolved = { ...row };
      for (const field of fkFields) {
        const opts = options[field.name] || [];
        const match = opts.find((o) => o.value === row[field.name]);
        if (match) resolved[field.name] = match.label;
      }
      return resolved;
    });
  }

  async function load() {
    setLoading(true);
    setError("");
    try {
      delete listCache[entityKey];
      const data = await client.list();
      const options = await loadFkOptions();
      setRows(data);
      setFkOptions(options);
      setResolvedRows(resolveRowsForDisplay(data, options));
    } catch (err) {
      if (err instanceof ApiError) {
        setError(err.message);
      } else {
        setError("No se pudo conectar con la API.");
      }
    }
    setLoading(false);
  }

  useEffect(() => {
    setEditing(null);
    setBanner(null);
    load();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [entityKey]);

  const handleCreate = () => setEditing({});
  const handleEdit = (row) => setEditing(row);
  const handleCancel = () => setEditing(null);

  const handleDelete = async (row) => {
    const id = entity.idField ? row[entity.idField] : null;
    if (id === null) {
      setBanner({ type: "error", text: "Esta entidad no tiene un identificador único disponible aún." });
      return;
    }
    if (!window.confirm("¿Eliminar este registro de " + entity.label.toLowerCase() + "?")) return;
    try {
      await client.remove(id);
      setBanner({ type: "success", text: "Registro eliminado." });
      load();
    } catch (err) {
      setBanner({
        type: "error",
        text: err instanceof ApiError ? err.message : "No se pudo eliminar el registro.",
      });
    }
  };

  const handleSubmit = async (values) => {
    setSubmitting(true);
    try {
      const isEditing = editing && entity.idField && editing[entity.idField] !== undefined;
      if (isEditing) {
        await client.update(editing[entity.idField], values);
        setBanner({ type: "success", text: "Registro actualizado." });
      } else {
        await client.create(values);
        setBanner({ type: "success", text: "Registro creado." });
      }
      setEditing(null);
      load();
    } catch (err) {
      setBanner({
        type: "error",
        text: err instanceof ApiError ? err.message : "No se pudo guardar el registro.",
      });
    }
    setSubmitting(false);
  };

  return (
    <div className="crud-page">
      <div className="crud-page-header">
        <div>
          <h1 className="crud-page-title">{entity.labelPlural}</h1>
          <p className="crud-page-subtitle">Administra los registros de {entity.labelPlural.toLowerCase()}.</p>
        </div>
        <button className="btn-primary" onClick={handleCreate}>
          + Nuevo
        </button>
      </div>

      {banner && (
        <div className={"crud-banner crud-banner--" + banner.type}>
          {banner.text}
          <button className="crud-banner-close" onClick={() => setBanner(null)}>
            ×
          </button>
        </div>
      )}

      <CrudTable
        entity={entity}
        rows={rows}
        resolvedRows={resolvedRows}
        loading={loading}
        error={error}
        onEdit={handleEdit}
        onDelete={handleDelete}
      />

      {editing !== null && (
        <CrudForm
          entity={entity}
          initialData={Object.keys(editing).length ? editing : null}
          options={fkOptions}
          onSubmit={handleSubmit}
          onCancel={handleCancel}
          submitting={submitting}
        />
      )}
    </div>
  );
}
