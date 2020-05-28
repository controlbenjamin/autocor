$.ajaxSetup({
    error: function (jqXHR, textStatus, errorThrown) {

        if (textStatus === 'abort' || jqXHR.responseText === '')
            return;
        notificarError(textStatus);
    }
});

function getToken() {
    return $('input[type=hidden][name=__RequestVerificationToken]').val();
}

function tokenize(obj) {
    obj.__RequestVerificationToken = getToken();
}

function serializeQueryString(obj, prefix) {
    var str = [], p;
    for (p in obj) {
        if (obj.hasOwnProperty(p)) {
            var k = prefix ? prefix + "[" + p + "]" : p, v = obj[p];
            str.push((v !== null && typeof v === "object") ?
              serialize(v, k) :
              encodeURIComponent(k) + "=" + encodeURIComponent(v));
        }
    }
    return str.join("&");
}

function baseUrl(url, routeValues) {
    var _baseUrl = document.getElementById('_baseUrl').value;

    url = url || '';

    if (url !== '' && url.charAt(0) === '/')
        url = url.replace('/', '');

    var resultUrl = _baseUrl + url;

    if (routeValues) {
        resultUrl += '?' + serializeQueryString(routeValues)
    }

    return resultUrl;
}

function convertJsonDate(jsonDate) {
    return new Date(parseInt(jsonDate.substr(6)));
}

function checkMensaje() {
    var mensaje = $('#_mensaje').val();

    if (!mensaje) {
        return;
    }

    mensaje = JSON.parse(mensaje);
    notificarMensajeWeb(mensaje);
}

function setImageDefault(img) {
    img.setAttribute("src", "http://www.autocor.com.ar/autocor/usuarios/img_productos/comodin.jpg");
}

/**
 * Actualiza un parámetro en una url con query string
 * @param {string} uri - La url
 * @param {string} key - El parámetro
 * @param {string} value - El nuevo valor del parámetro
 */

function updateQueryStringParameter(uri, key, value) {
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = uri.indexOf('?') !== -1 ? "&" : "?";
    if (uri.match(re)) {
        return uri.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
        return uri + separator + key + "=" + value;
    }
}

function obtenerPrecio() {
    // buscar en localstorage
    var precio = null;
    var precioString = localStorage.getItem('autocor-precio');

    if (precioString === null) {
        // buscar mediante ajax
        $.ajax({
            url: baseUrl("MiCuenta/ObtenerConfiguracionCliente"),
            method: 'get',
            async: false
        }).done(function (response) {
            localStorage.setItem('autocor-precio', JSON.stringify(response));
            precio = response;
        });
    } else {
        precio = JSON.parse(precioString);
    }

    return precio;
}

function obtenerCandado() {
    var candadoValores = sessionStorage.getItem('candados');

    if (!candadoValores) {
        return {
            marca: '',
            tipo: ''
        };
    }

    return JSON.parse(candadoValores);
}

function obtenerOcultarBody() {
    var ocultarValor = sessionStorage.getItem('ocultarTabla');

    if (!ocultarValor) {
        return { ocultar: false };
    }

    return JSON.parse(ocultarValor);
}

function actualizarPrecio(beneficio, descuento, iva) {
    localStorage.setItem('autocor-precio', JSON.stringify({ beneficio: beneficio, descuento: descuento, iva: iva }));
}

function actualizarCandados(marca, tipo) {
    sessionStorage.setItem('candados', JSON.stringify({ marca: marca, tipo: tipo }));
}

function generarCandado() {
    sessionStorage.setItem('candados', JSON.stringify({ marca: false, tipo: false }));
}

function generarOcultarBody() {
    sessionStorage.setItem('ocultarTabla', JSON.stringify({ ocultar: false }));
}

function actualizarOcultarBody(ocultar) {
    sessionStorage.setItem('ocultarTabla', JSON.stringify({ocultar:ocultar}));
}

//helpers handlebars//

function helperFormatoFecha() {
    Handlebars.registerHelper('formatDate', function (date) {
        return convertJsonDate(date).toLocaleDateString();
    });
}

function helperJson() {
    Handlebars.registerHelper('json', function (obj) {
        return JSON.stringify(obj);
    });
}

function helperPrecio() {
    Handlebars.registerHelper('formatPrecio', function (precio) {
        var precioVal = Number(precio) || 0;
        return '$ ' + precioVal.toFixed(2);
    });
}

//---Configuracion de mensajes---//

function notificar(mensaje, options) {
    options = options || {};

    var overhangOptions = {
        message: mensaje,
        overlay: true,
        duration: 2
    };

    overhangOptions = Object.assign(overhangOptions, options);

    $('body').overhang(overhangOptions);
}

function notificarMensajeWeb(mensajeWeb) {
    var texto = mensajeWeb.Texto;
    var tipo = mensajeWeb.CssContext;

    if(tipo === 'danger'){
        tipo = 'error';
    }

    notificar(texto, { type: tipo });

}

function notificarSuccess(mensaje) {
    notificar(mensaje, {
        type: 'success'
    });
}

function notificarError(mensaje) {
    notificar(mensaje, {
        type: 'error',
        closeConfirm: true,
        duration: 0
    });
}

function notificarInfo(mensaje) {
    notificar(mensaje, {
        type: 'info'
    });
}

function notificarWarning(mensaje) {
    notificar(mensaje, {
        type: 'warn'
    });
}

function confirm(mensaje,callback) {
    $("body").overhang({
        type: "confirm",
        primary: "#40D47E",
        accent: "#27AE60",
        yesColor: "#3498DB",
        message: mensaje,
        overlay: true,
        callback: callback
    });
}

function input(mensaje, callback, type) {

    type = type || '';

    $('body').overhang({
        type: 'prompt',
        primary: '#337ab7',
        accent: '#286090',
        message: mensaje,
        overlay: true,
        inputType:  type,
        callback: callback
    });
}

function getIntl() {
    return new Intl.NumberFormat('es-AR', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
}

function formatNumber(number) {
    return getIntl().format(number);
}

// clases

function CalculadoraPrecio() {
    this.configuracion = obtenerPrecio();
}

CalculadoraPrecio.prototype.calcularPrecio = function (precioLista) {
    var multiplicador = (1 + this.configuracion.beneficio / 100) * (1 - this.configuracion.descuento / 100);

    if (this.configuracion.iva) {
        multiplicador = multiplicador * 1.21;
    }

    return precioLista * multiplicador;
};

CalculadoraPrecio.prototype.calcularPrecioNeto = function (precioLista) {
    return precioLista * (1 - this.configuracion.descuento / 100);
};

//$(document).on('blur', '.txtCantidadCarrito', function (ev) {
//    let unidadVenta = $(this).data('unidadVenta');
//    let cantidad = $(this).val();

//    if (cantidad % unidadVenta !== 0) {
//        notificarWarning('Cantidad no válida. La unidad de venta es ' + unidadVenta);
//        $(this).val(unidadVenta);
//    }
//});