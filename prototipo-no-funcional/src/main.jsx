import { StrictMode, useState } from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';
import FakeLogin from './components/FakeLogin';
import Dashboard from './components/Dashboard';
import CourseModal from './components/CourseModal';
function Root() {
  const [user, setUser] = useState(null);

  if (!user) {
    return <FakeLogin onLogin={setUser} />;
  }

  return <Dashboard user={user} />;
}

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <Root />
  </StrictMode>,
);