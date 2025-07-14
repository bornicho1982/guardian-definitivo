const axios = require('axios');
require('dotenv').config();

async function getProfile(req, res) {
  try {
    const bungieMembershipId = req.user.membership_id;
    const response = await axios.get(`https://www.bungie.net/Platform/Destiny2/254/Profile/${bungieMembershipId}/?components=100`, {
      headers: {
        'X-API-Key': process.env.BUNGIE_API_KEY,
        'Authorization': `Bearer ${req.user.access_token}`
      }
    });
    res.json(response.data.Response);
  } catch (error) {
    res.status(500).json({ error: 'Error al obtener el perfil del jugador' });
  }
}

module.exports = { getProfile };
