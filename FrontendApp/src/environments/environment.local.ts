export const environment = {
  production: false,
  apiUrl: '/api',  // Usar proxy para evitar CORS en desarrollo
  apiBaseUrl: 'http://localhost:5000',
  environmentName: 'Local Development (API Local + Web Local)',
  useProxy: true,
  corsEnabled: true,
  debugMode: true
};
