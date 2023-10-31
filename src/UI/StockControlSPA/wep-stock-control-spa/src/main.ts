import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import { enableProdMode } from '@angular/core';

// получаем корневой урл spa/api
export function getBaseUrl() {
  return environment.apiBaseUri.endsWith('/') ? environment.apiBaseUri : `${environment.apiBaseUri}/`;
}

// создаём провайдера для внедрения базового урла в последующие модули/компоненты
// передаём в конструктор @Inject('BASE_URL') baseUrl: string
const providers = [
  { provide: 'API_BASE_URL', useFactory: getBaseUrl, deps: [] }
];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic(providers).bootstrapModule(AppModule)
  .catch(err => console.log(err));
