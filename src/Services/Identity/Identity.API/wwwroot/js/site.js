$(function () {

    // Обработчик в jquery нужно в первую очередь указывать, а толшько потом уже click

    handler = {

        toogleFaEye: function () {
            let parent = $(this).parent('.form-group');
            let ctrl = parent.children('input').first();

            let type = ctrl.attr("type") === 'password' ? 'text' : 'password';

            ctrl.attr("type", type);

            // https://www.w3schools.com/jquery/html_toggleclass.asp#:~:text=The%20toggleClass()%20method%20toggles,This%20creates%20a%20toggle%20effect.
            $(this).toggleClass("fa-solid fa-eye");
            $(this).toggleClass("fa fa-eye-slash");
        }
    };

    $('#togglePassword').click(handler.toogleFaEye);
    $('#togglePasswordConfirm').click(handler.toogleFaEye);

});
