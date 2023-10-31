# WepStockControlSpa

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 16.0.4.

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The application will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via a platform of your choice. To use this command, you need to first add a package that implements end-to-end testing capabilities.

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI Overview and Command Reference](https://angular.io/cli) page.

## Нюансы запуска 

1) при запуске скрипта "start-without-ssl": "ng serve --ssl false" приложение запустится по http, но обращение к серверу не будет происходить, если 
не выполнить одно глобальное действие : 

- на бэке в Identity.API в конфигурации заменить значение для  "SpaClient" на "http://localhost:1170" 

и одно из следующих действий: 
- в env.js заменить текущее значение для window["env"]["apiBaseUri"] на "https://localhost:7001";
- заменить на бэке все https на http
- в env.js заменить текущее значение для window["env"]["apiBaseUri"] на "//localhost:5001" и на бэке добавить поддержку http в дополнение к https;
- свой вариант ;-)

2) если запуск осуществляется по стандартному скрипту npm start, нужно выполнить одно из следующих действий:

- предварительно создать и установить локальный сертификат, затем указать путь к нему в angular.json options:sslKey и options:sslCert
- удалить в angular.json options:sslKey и options:sslCert