import React, { useState, useEffect } from 'react';
import LoginButton from './components/LoginButton';
import LFG from './components/LFG';

function App() {
  const [user, setUser] = useState(null);

  useEffect(() => {
    fetch('/auth/status')
      .then(res => res.json())
      .then(data => {
        if (data.authenticated) {
          setUser(data.user);
        }
      });
  }, []);

  return (
    <div className="bg-primary text-text-primary min-h-screen">
      <header className="bg-secondary p-4 flex justify-between items-center">
        <h1 className="text-3xl text-accent">Guardián Definitivo</h1>
        {user ? <p>Bienvenido, {user.bungieNetUser.displayName}</p> : <LoginButton />}
      </header>
      <main className="p-4">
        {user && <LFG />}
        {!user && <p>Inicia sesión para ver el LFG.</p>}
      </main>
    </div>
  );
}

export default App;
