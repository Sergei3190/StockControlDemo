(function(window) {
    console.log('env.js', window);
    window["env"] = window["env"] || {};
    window["env"]["apiBaseUri"] = "//localhost:7001"; // значение по умолчанию
    window["env"]["useHash"] = false; // значение по умолчанию для докера нужен true, чтобы прокси срабатывал при перезагрузке страницы
})(this);
