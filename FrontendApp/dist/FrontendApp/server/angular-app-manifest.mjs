
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
  baseHref: '/',
  locale: undefined,
  routes: undefined,
  entryPointToBrowserMapping: {},
  assets: {
    'index.csr.html': {size: 6669, hash: 'c1fc181a9ec5dcfecf8bf00309f6e782063edb7be9f589061b984d8d66b9d6db', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1127, hash: '0d79e71e73e54c8dc2d7ed458c77fc4daf832cfbb0e2efae5b9c3aeeb90ba193', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'styles-A57KNJ62.css': {size: 239557, hash: 'uhKmQxJ2kZg', text: () => import('./assets-chunks/styles-A57KNJ62_css.mjs').then(m => m.default)}
  },
};
