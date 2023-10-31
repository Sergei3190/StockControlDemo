#Мы используем другой экземпляр NGINX (не путать с прокси-сервером) для размещения файлов компонентов
#Angular и прослушивания определенного порта — по умолчанию это порт 80.

FROM node:18-alpine as build
WORKDIR /app

RUN npm install -g @angular/cli

COPY ./package.json .
RUN npm install
COPY . .
#ARG apiBaseUri
#ARG configuration=production
#RUN npm run build -- --output-path=./dist/out --configuration $configuration
RUN npm run build

FROM nginx as runtime
# обслуживание файла по умолчанию
COPY --from=build /app/dist/wep-stock-control-spa /usr/share/nginx/html
#создадим путь, по которому будет лежать наше веб приложение (не вышел вариант)
# RUN mkdir /var/www
# COPY --from=build /app/dist/wep-stock-control-spa /var/www/app

# Когда контейнер запустится, замените env.js значениями из переменных среды.
#  envsubst эта команда может создать новый env.js файл на основе шаблона и заменить заполнители переменными среды.
CMD ["/bin/sh",  "-c",  "envsubst < /usr/share/nginx/html/assets/env.template.js > /usr/share/nginx/html/assets/env.js && exec nginx -g 'daemon off;'"]
#CMD ["/bin/sh",  "-c",  "envsubst < /var/www/app/assets/env.template.js > /var/www/app/assets/env.js && exec nginx -g 'daemon off;'"]
