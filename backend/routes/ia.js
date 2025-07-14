const express = require('express');
const router = express.Router();
const ghostAssistant = require('../../ia/ghost.engine');

router.post('/recommend-build', async (req, res) => {
  try {
    const { prompt } = req.body;
    const recommendation = await ghostAssistant.getBuildRecommendation(prompt);
    res.json({ recommendation });
  } catch (error) {
    res.status(500).json({ error: 'Error al obtener la recomendación de build' });
  }
});

module.exports = router;
