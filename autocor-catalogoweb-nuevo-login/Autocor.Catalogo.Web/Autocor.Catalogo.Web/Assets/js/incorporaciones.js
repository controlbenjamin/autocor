var _productoSeleccionado = null;

$(document).ready(function () {

    inicializarKeynavigator(true, false);

    $('.table-productos tbody tr:first').addClass('activo').click();

    $('.btnAgregarCarrito').on('click', function () {
        var cantidad = $('.txtCantidadCarrito').val();
        var unidadVenta = $('.txtCantidadCarrito').data('unidadVenta');

        if (isNaN(cantidad) || cantidad < 1) {
            notificarWarning('Ingrese una cantidad válida');
            return;
        }

        var codigoPieza = $('#tabla-Productos-Incorporados tbody tr.activo').data('producto');

        agregarAlCarrito(cantidad, codigoPieza, true, unidadVenta, function () {
            notificarSuccess('Producto agregado al carrito');
        });
    });

    $('.contenedorMobilCarrito').on('click', '.btnAgregarCarritoMovil', function () {
        var codigoProducto = $('.card-producto').data('producto');
        var unidadVenta = $(this).data('unidadVenta');

        if (!codigoProducto) {
            notificarWarning('No se ha especificado el producto');
            return;
        }

        var htmlOpciones = '';

        for (var i = 1; i < 5; i++) {
            var unidad = ' unidades';
            var unidades = i * unidadVenta;

            if (unidades === 1) unidad = ' unidad';

            htmlOpciones += '<li data-unidad-venta="' + unidadVenta + '" data-unidades="' + unidades + '">' + unidades + unidad + '</li>';
        }

        htmlOpciones += '<li data-unidad-venta="' + unidadVenta + '" data-unidades="mas">Más...</li>';

        var $modelCant = $('#modelCant');
        $modelCant.find('#codigoProductoSelectUnidades').val(codigoProducto);
        $modelCant.find('.selectUnidades').html(htmlOpciones);
        $modelCant.modal('show');
    });

    $('#modelCant').on('click', '.selectUnidades li', function () {
        var codigoProducto = $('#codigoProductoSelectUnidades').val();
        var unidadVenta = $(this).data('unidadVenta');
        var unidades = $(this).data('unidades');

        if (unidades === 'mas') {
            input('Ingrese cantidad', function (value) {
                var cantidad = parseInt(value);

                if (isNaN(cantidad) || cantidad < 1) {
                    notificarWarning('Ingrese una cantidad válida');
                    return;
                }
                agregarAlCarrito(cantidad, codigoProducto, true, unidadVenta, function () {
                    notificarSuccess('Producto agregado al carrito');
                    $('#modelCant').modal('hide');
                });
            }, 'number');
            return;
        }

        if (unidades <= 0) {
            // TODO
            notificarWarning('Ingrese una cantidad válida');
            return;
        }

        agregarAlCarrito(unidades, codigoProducto, true, unidadVenta, function () {
            notificarSuccess('Producto agregado al carrito');
            $('#modelCant').modal('hide');
        });
    });

    $('#detalle-producto').on('click', '.img-producto', function () {
        let $imgProducto = $(this);
        let imgSrc = $imgProducto.attr('src');
        let imgAlt = $imgProducto.attr('alt');

        var $modalProducto = $('#modal-imagen-producto');
        var $imgModalProdcuto = $modalProducto.find('img');
        $imgModalProdcuto.attr('src', imgSrc);
        $imgModalProdcuto.attr('alt', imgAlt);
        $modalProducto.fadeIn();
    });

    $(".cerrar").click(function () {

        $(".modal").fadeOut(300);

    });

    var $table = $('table.headerFixed');

    $table.floatThead({
        scrollContainer: function ($table) {
            return $table.closest('.wrapper');
        }
    });

});

function CargarProductos($tr) {
    let rubro = $tr.data('rubro');

    let urlProductosIncorporaciones = baseUrl('/Incorporaciones/ObtenerProductos');

    $.get(urlProductosIncorporaciones, { codigoRubro: rubro }, function (response) {
        var source = $("#template-producto").html();
        var template = Handlebars.compile(source);
        var html = template(response);
        $('#tbody-productos').html(html);

        source = $("#template-mobilContent").html();
        template = Handlebars.compile(source);
        html = template(response);
        $('#mobilContent').html(html);

        inicializarKeynavigator(false, true);
    });
}

function CargarProductos2($tr) {
    let rubro = $tr.data('rubro');

    let urlProductosIncorporaciones = baseUrl('/Incorporaciones/ObtenerProductos');

    $.get(urlProductosIncorporaciones, { codigoRubro: rubro }, function (response) {

        var source = $("#template-mobilContent").html();
        var template = Handlebars.compile(source);

        var html = template(response);
        $('#mobilContent').html(html);
        inicializarKeynavigator(false, true);
    });
}

function CargarProductoSeleccionado(producto) {
    let codPieza = producto;

    let urlProductosIncorporaciones = baseUrl('/Incorporaciones/ObtenerProductoSeleccionado');

    $.get(urlProductosIncorporaciones, { codigoPieza: codPieza }, function (response) {
        // verificamos si el producto ya fue cargado (para evitar agregar los parámetros manuales)
        if (!response.GrupoParametros) {
            // agregar código artículo y nro original como parámetros manuales
            response.Parametros.unshift({ Parametro: 'Nro. Original', Valor: response.NumeroOriginal || '---' });
            response.Parametros.unshift({ Parametro: 'Cod. Articulo', Valor: response.CodigoPieza });
        }

        let cantidadPorColumna = 2;

        let divisionParametros = [];

        for (var i = 0; i < response.Parametros.length; i += cantidadPorColumna) {
            divisionParametros.push(response.Parametros.slice(i, i + cantidadPorColumna));
        }

        response.GrupoParametros = divisionParametros;

        response.TieneStock = response.Stock > 3;

        var source = $("#template-detalle-producto").html();
        var template = Handlebars.compile(source);

        var html = template(response);
        $('#detalle-producto').html(html);

        // actualizar input de cantidad
        var $txtCantidadCarrito = $('.txtCantidadCarrito');
        $txtCantidadCarrito.val(response.UnidadVenta);
        $txtCantidadCarrito.attr('step', response.UnidadVenta);
        $txtCantidadCarrito.attr('min', response.UnidadVenta);
        $txtCantidadCarrito.data('unidadVenta', response.UnidadVenta);
        $('.unidadVenta').html(response.UnidadVenta);

        $('.botoneraCarritoIncorporacion').show();
    });
}

//function CargarProductoRubro(producto) {
//}

function inicializarKeynavigator(rubros, productos) {
    if (rubros) {
        $('table#tabla-Rubro-Incorporaciones > tbody tr').keynavigator({
            cycle: false,
            activeClass: 'activo',
            onAfterActive: function ($tr) {
                CargarProductos($tr);
                //CargarProductos2($tr);
            }
        });
    }

    if (productos) {
        $('table#tabla-Productos-Incorporados > tbody tr').keynavigator({
            cycle: false,
            activeClass: 'activo',
            onAfterActive: function ($tr) {
                _productoSeleccionado = $tr.data('producto');
                //CargarProductoRubro(_productoSeleccionado);
                CargarProductoSeleccionado(_productoSeleccionado);
            }
        });
    }
}