
function agregarAlCarrito(cantidad, codigoProducto, acumularCantidad, unidadVenta, callback) {

    var data = {
        codigoPieza: codigoProducto,
        cantidad: cantidad,
        acumularCantidad: acumularCantidad
    };

    var guardarCarrito = function (data, callback) {
        tokenize(data);

        // verificar si está en carrito
        obtenerCarrito(function (items) {
            var existente;

            if (items && items.length > 0) {
                existente = items.find(function (el, idx) {
                    return el.CodigoPieza === data.codigoPieza;
                });
            }

            if (existente) {
                confirm('Producto ya existente en el carrito. ¿Desea agregar más unidades?', function (value) {
                    //var confirmacion = value ? "Si" : "No";
                    if (value) {
                        _agregarACarrito(data, callback);
                        return;
                    }

                    return;
                });
            }
            else {
                _agregarACarrito(data, callback);
            }

        });
    }

    if (cantidad % unidadVenta !== 0) {
        cantidad = (parseInt(cantidad / unidadVenta) + 1) * unidadVenta;
        const mensaje = 'Este artículo tiene una unidad de venta (UV) de ' + unidadVenta + '. Se agregarán ' + cantidad + ' unidades. \n ¿Desea continuar?';
        confirm(mensaje, function (res) {
            if (res === true) {
                data.cantidad = cantidad;
                guardarCarrito(data, callback);
            } else {
                return;
            }
        });
    } else {
        guardarCarrito(data, callback);
    }

}

function modificarCantidad(cantidad, codigoProducto, acumularCantidad, callback) {
    var data = {
        codigoPieza: codigoProducto,
        cantidad: cantidad,
        acumularCantidad: acumularCantidad
    };

    tokenize(data);

    $.ajax({
        data: data,
        url: baseUrl('Carrito/AgregarCarrito'),
        type: 'post',
        beforeSend: function () {
        }
    }).done(function (response) {
        if (typeof callback == 'function') {
            callback();
        }
    });
}

function obtenerCarrito(callback) {
    var url = baseUrl('/Carrito/ObtenerCarrito');
    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'json'
    }).done(function (response) {
        var items = response;
        if (callback && typeof (callback) === 'function') {
            callback(items);
        }
    });
}

function _agregarACarrito(data, callback) {
    $.ajax({
        data: data,
        url: baseUrl('Carrito/AgregarCarrito'),
        type: 'post'
    }).done(function (response) {
        if (typeof callback == 'function') {
            callback();
        }
    });
}

