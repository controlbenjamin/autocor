var iniciarRegistro = function() {
    $("#RegistroDiv").show();
    $("#mensajeDiv").hide();
}

$(document).ready(function () {
    $("#botonContinuar").click(iniciarRegistro);
});

