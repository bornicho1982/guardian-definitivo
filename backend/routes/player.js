const express = require('express');
const router = express.Router();
const { getProfile } = require('../controllers/playerController');

// Middleware para proteger rutas
function ensureAuthenticated(req, res, next) {
  if (req.isAuthenticated()) {
    return next();
  }
  res.status(401).json({ error: 'No autenticado' });
}

router.get('/profile', ensureAuthenticated, getProfile);

module.exports = router;
