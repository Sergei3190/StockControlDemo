(function(window) {
    console.log('env.template.js', window);
    window.env = window.env || {};
    // Environment variables
    // будем задавать для переменных-заполнителей (${}) значения из env файла docker-compose
    // после шаблон файла будет заменять значения для не шаблона аналогичного файла
    window["env"]["apiBaseUri"] = "${apiBaseUri}"; // значение по умолчанию
    window["env"]["useHash"] = "${useHash}";
})(this);