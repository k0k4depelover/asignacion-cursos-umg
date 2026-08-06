import { Link } from "react-router-dom";
import { navGroups, schema } from "../entities/schema";
import "../styles/Crud.css";

export default function Home() {
  return (
    <div className="crud-page">
      <div className="crud-page-header">
        <div>
          <h1 className="crud-page-title">Panel de administración</h1>
          <p className="crud-page-subtitle">Elige un módulo para gestionar sus registros.</p>
        </div>
      </div>

      {navGroups.map((group) => (
        <div key={group.title} className="home-group">
          <h3 className="home-group-title">{group.title}</h3>
          <div className="home-grid">
            {group.items.map((key) => (
              <Link key={key} to={`modulo/${key}`} className="home-card">
                <span className="home-card-title">{schema[key].labelPlural}</span>
                <span className="home-card-arrow">→</span>
              </Link>
            ))}
          </div>
        </div>
      ))}
    </div>
  );
}
