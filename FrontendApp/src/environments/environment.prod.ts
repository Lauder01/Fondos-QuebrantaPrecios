export const environment = {
  production: true,
  apiUrl: '/api',  // Usar rewrite de Vercel para evitar CORS
  apiBaseUrl: 'https://devdemoapi1.azurewebsites.net',
  environmentName: 'Production (API Azure + Web Vercel)',
  useProxy: false,
  corsEnabled: false,
  debugMode: false
};
