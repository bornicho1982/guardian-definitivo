const express = require('express');
const passport = require('passport');
const router = express.Router();

// Iniciar la autenticación con Bungie
router.get('/bungie', passport.authenticate('bungie'));

// Callback de Bungie
router.get('/callback',
  passport.authenticate('bungie', { failureRedirect: '/' }),
  (req, res) => {
    // Redireccionar al frontend
    res.redirect('http://localhost:5173');
  }
);

// Obtener el estado de la autenticación
router.get('/status', (req, res) => {
  if (req.isAuthenticated()) {
    res.json({ authenticated: true, user: req.user });
  } else {
    res.json({ authenticated: false });
  }
});

// Cerrar sesión
router.get('/logout', (req, res) => {
  req.logout();
  res.redirect('/');
});

module.exports = router;
