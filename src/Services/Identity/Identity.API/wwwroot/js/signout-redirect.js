window.addEventListener("load", function () { // нужно для отправки пользователя по указанной ссылке возврата в приложения после выхода из системы
    var a = document.querySelector("a.PostLogoutRedirectUri");
    if (a) {
        window.location = a.href;
    }
});
