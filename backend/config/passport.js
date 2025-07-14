const passport = require('passport');
const OAuth2Strategy = require('passport-oauth2');
require('dotenv').config();

passport.use('bungie', new OAuth2Strategy({
    authorizationURL: 'https://www.bungie.net/en/OAuth/Authorize',
    tokenURL: 'https://www.bungie.net/platform/app/oauth/token/',
    clientID: process.env.BUNGIE_CLIENT_ID,
    clientSecret: process.env.BUNGIE_CLIENT_SECRET,
    callbackURL: `https://localhost:${process.env.HTTPS_PORT}/auth/callback`
  },
  (accessToken, refreshToken, profile, cb) => {
    // Aquí se manejaría la lógica del usuario
    return cb(null, profile);
  }
));

passport.serializeUser((user, done) => {
  done(null, user);
});

passport.deserializeUser((user, done) => {
  done(null, user);
});

module.exports = passport;
