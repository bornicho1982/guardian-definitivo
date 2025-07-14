import React from 'react';

const LoginButton = () => {
  const handleLogin = () => {
    window.location.href = 'http://localhost:8000/auth/bungie';
  };

  return (
    <button
      onClick={handleLogin}
      className="bg-accent hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
    >
      Iniciar sesión con Bungie
    </button>
  );
};

export default LoginButton;
