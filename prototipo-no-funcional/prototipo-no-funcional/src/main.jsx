import { StrictMode, useState } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import "./index.css";
import FakeLogin from "./components/FakeLogin";
import AppLayout from "./layout/AppLayout";
import Home from "./pages/Home";
import CrudPage from "./pages/CrudPage";

function Root() {
  const [user, setUser] = useState(null);

  if (!user) {
    return <FakeLogin onLogin={setUser} />;
  }

  return (
    <BrowserRouter>
      <Routes>
        <Route element={<AppLayout user={user} onLogout={() => setUser(null)} />}>
          <Route index element={<Home />} />
          <Route path="/modulo/:entityKey" element={<CrudPage />} />
          <Route path="*" element={<Navigate to="/" replace />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

createRoot(document.getElementById("root")).render(
  <StrictMode>
    <Root />
  </StrictMode>
);
