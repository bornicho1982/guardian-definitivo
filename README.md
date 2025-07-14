# Guardián Definitivo

Una aplicación web diseñada específicamente para jugadores de Destiny 2 que quieren mejorar su experiencia de juego con herramientas avanzadas.

## Funcionalidades Principales

- **Autenticación con Bungie**: Iniciar sesión con cuenta de Destiny 2.
- **Inventario Completo**: Ver, comparar y organizar armas, armaduras, etc.
- **Actividades Rotativas**: Xur, mazmorras, nightfall, crucible.
- **Gestión de Builds**: Guardar, editar y compartir configuraciones de personaje.
- **Sistema LFG (Looking For Group)**: Crear grupos por actividad + chat integrado.
- **Chat de Voz tipo Discord**: Usando WebRTC y Socket.IO.
- **Asistente Ghost (IA Local)**: Recomendaciones de builds e información estratégica.
- **Notificaciones Personalizadas**: Alertas cuando aparezca un ítem deseado.
- **Perfil del Jugador**: Ver datos del usuario y sus personajes.

## Tecnologías Utilizadas

- **Frontend**: React + TypeScript + Tailwind CSS
- **Backend**: Node.js + Express + Socket.IO
- **Base de Datos**: MongoDB
- **Motor de IA Local**: Phi-3-mini (Microsoft) + ONNX
- **Chat de Voz**: WebRTC + Socket.IO

## Estructura del Proyecto

```
guardian-definitivo/
│
├── frontend/
│   ├── public/
│   ├── src/
│   │   ├── components/
│   │   ├── utils/
│   │   └── App.jsx
│   └── index.html
│
├── backend/
│   ├── routes/
│   ├── controllers/
│   ├── config/
│   ├── models/
│   └── server.js
│
├── ia/
│   └── ghost.engine.js
│
├── public/
├── config/ssl/
├── .env
├── package.json
└── README.md
```
