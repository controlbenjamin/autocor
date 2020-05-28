var _productoSeleccionado = null;
var prueba = "@hayProductos";
$(document).ready(function () {

    validarBotones();
    inicializarKeynavigator();

    $('.table-productos tbody tr:first').addClass('activo').click();

    $('.btnCantidad').on('click', function () {

        input('Ingrese nueva cantidad', function (value) {

            var cantidad = parseInt(value);

            if (isNaN(cantidad) || cantidad < 1) {
                notificarWarning('Ingrese una cantidad válida');
                return;
            }

            var $tr = $('#tablaCarrito tbody tr.activo');

            if (!$tr) {
                return;
            }

            var codigoPieza = $tr.data('producto');
            var unidadVenta = $tr.data('unidadVenta');

            //if (cantidad % unidadVenta !== 0) {
            //    notificarWarning('Debe ingresar una cantidad acorde a la unidad de venta (' + unidadVenta + ')');
            //    return;
            //}

            if (cantidad % unidadVenta !== 0) {
                cantidad = (parseInt(cantidad / unidadVenta) + 1) * unidadVenta;
                const mensaje = 'Este artículo tiene una unidad de venta (UV) de ' + unidadVenta + '. Se agregarán ' + cantidad + ' unidades. \n ¿Desea continuar?';
                confirm(mensaje, function (res) {
                    if (res === true) {
                        modificarCantidad(cantidad, codigoPieza, false, function () {
                            $('#tablaCarrito tbody tr[data-producto=' + codigoPieza + '] td.col-cantidad').html(cantidad);
                            window.location.reload();
                        });
                    } else {
                        return;
                    }
                });
            } else {
                modificarCantidad(cantidad, codigoPieza, false, function () {
                    $('#tablaCarrito tbody tr[data-producto=' + codigoPieza + '] td.col-cantidad').html(cantidad);
                    window.location.reload();
                });
            }

            //modificarCantidad(cantidad, codigoPieza, false, function () {
            //    $('#tablaCarrito tbody tr[data-producto=' + codigoPieza + '] td.col-cantidad').html(cantidad);
            //    window.location.reload();
            //});
        });

    });

    $('.btnEliminar').on('click', function () {
        var codigoPieza = $('#tablaCarrito tbody tr.activo').data('producto');
        EliminarItemCarrito(codigoPieza, function () {
            $('#tablaCarrito tbody tr.activo').remove();
            var cant = $('#tablaCarrito tbody tr').length;

        });
    });

    $('.btnVaciarCarrito').on('click', function () {
        VaciarCarrito(function () {
            $('#tablaCarrito tbody tr').remove();
            notificarSuccess('Su carrito se vació exitosamente');
            prueba = "False";
            validarBotones();
        });
    });

    var $table = $('table.headerFixed');

    $table.floatThead({
        scrollContainer: function ($table) {
            return $table.closest('.wrapper');
        }
    });

    $('.btnRealizarPedido').on('click', function () {
        var observacion = $('#txtObservacion').val();
        $('#observaciones').val(observacion);
        $('#formPedido').submit();
        validarBotones();
    });

    //funcionalidad botones mobil
    $('.contenedorMobilCarrito').on('click', '.btnModificarCantidad', function () {
        var $cardProducto = $(this).closest('.card-producto');
        var codigoPieza = $cardProducto.data('producto');
        var unidadVenta = $cardProducto.data('unidadVenta');

        input('Ingrese cantidad', function (value) {

            var cantidad = parseInt(value);

            if (isNaN(cantidad) || cantidad < 1) {
                notificarWarning('Ingrese una cantidad válida');
                return;
            }

            //if (cantidad % unidadVenta !== 0) {
            //    notificarWarning('Debe ingresar una cantidad acorde a la unidad de venta (' + unidadVenta + ')');
            //    return;
            //}

            if (cantidad % unidadVenta !== 0) {
                cantidad = (parseInt(cantidad / unidadVenta) + 1) * unidadVenta;
                const mensaje = 'Este artículo tiene una unidad de venta (UV) de ' + unidadVenta + '. Se agregarán ' + cantidad + ' unidades. \n ¿Desea continuar?';
                confirm(mensaje, function (res) {
                    if (res === true) {
                        modificarCantidad(cantidad, codigoPieza, false, function () {
                            window.location.reload();
                            notificarSuccess('Cantidad modificada');
                        });
                    } else {
                        return;
                    }
                });
            } else {
                modificarCantidad(cantidad, codigoPieza, false, function () {
                    window.location.reload();
                    notificarSuccess('Cantidad modificada');
                });
            }

            //modificarCantidad(cantidad, codigoProducto, false, function () {
            //    window.location.reload();
            //    notificarSuccess('Cantidad modificada');
            //});
        });
    });

    $('.contenedorMobilCarrito').on('click', '.btnEliminarItem', function () {
        var codigoProducto = $(this).closest('.card-producto').data('producto');
        EliminarItemCarrito(codigoProducto, function () {
            window.location.reload();
        });
    });
});

//function CargarProductoRubro(producto) {

//}

function validarBotones() {
    if (prueba === "False") {
        $('.btnCantidad').attr("disabled", true);
        $('.btnEliminar').attr("disabled", true);
        $('.btnVaciarCarrito').attr("disabled", true);
        $('.btnRealizarPedido').attr("disabled", true);
        $('#txtObservacion').attr("disabled", true);
        $('.mensajeInfo').css('display', 'block');
        $('.tablaCarrito').css('display', 'none');
        $('.txtTotal').val('$ 0,00');
    }
}

function inicializarKeynavigator() {
    $('table#tablaCarrito > tbody tr').keynavigator({
        cycle: false,
        activeClass: 'activo',
        onAfterActive: function ($tr) {
            _productoSeleccionado = $tr.data('producto');
            //CargarProductoRubro(_productoSeleccionado);
        }
    });
}

function EliminarItemCarrito(codigoProducto, callback) {
    $("body").overhang({
        type: "confirm",
        primary: "#40D47E",
        accent: "#27AE60",
        yesColor: "#3498DB",
        message: "Está seguro que desea eliminar?",
        overlay: true,
        callback: function (value) {
            var confirmacion = value ? "Si" : "No";
            if (confirmacion === "Si") {
                var data = {
                    codigoPieza: codigoProducto,
                };


                tokenize(data);

                $.ajax({
                    data: data,
                    url: baseUrl('Carrito/EliminarItemCarrito'),
                    type: 'post',
                    beforeSend: function () {
                    }
                }).done(function (response) {
                    notificarSuccess('Producto eliminado');

                    if (typeof callback == 'function') {
                        callback();
                    }
                });
            };
            return;
        }
    });
}

function VaciarCarrito(callback) {
    var data = {};
    tokenize(data);
    $.ajax({
        data: data,
        url: baseUrl('Carrito/VaciarCarrito'),
        type: 'post',
        beforeSend: function () {
        }
    }).done(function (response) {
        if (typeof callback == 'function') {
            callback();
            window.location.reload();
        }
    });
}