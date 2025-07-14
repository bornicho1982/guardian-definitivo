const express = require('express');
const session = require('express-session');
const passport = require('./config/passport');
const { connectDB } = require('./config/db');
const http = require('http');
const { Server } = require("socket.io");
require('dotenv').config();

const app = express();
const server = http.createServer(app);
const io = new Server(server);
const port = process.env.PORT || 8000;

// Conectar a la base de datos
connectDB();

// Middlewares
app.use(session({
  secret: process.env.SESSION_SECRET,
  resave: false,
  saveUninitialized: false
}));
app.use(passport.initialize());
app.use(passport.session());

// Rutas
app.get('/', (req, res) => {
  res.send('¡Hola, mundo!');
});

const authRoutes = require('./routes/auth');
app.use('/api/player', playerRoutes);

const iaRoutes = require('./routes/ia');
app.use('/api/ia', iaRoutes);


io.on('connection', (socket) => {
  console.log('a user connected');

  socket.on('disconnect', () => {
    console.log('user disconnected');
  });

  socket.on('chat message', (msg) => {
    io.emit('chat message', msg);
  });

  // WebRTC signaling
  socket.on('offer', (offer) => {
    socket.broadcast.emit('offer', offer);
  });

  socket.on('answer', (answer) => {
    socket.broadcast.emit('answer', answer);
  });

  socket.on('candidate', (candidate) => {
    socket.broadcast.emit('candidate', candidate);
  });
});

server.listen(port, () => {
  console.log(`El servidor está escuchando en http://localhost:${port}`);
});
