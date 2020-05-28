var _cargarPagina = true;

var _productoSeleccionado = null;
var _productoEquivalencia = null; // producto al cual se le están consultando las equivalencias

var _calculadora = new CalculadoraPrecio();
var _precio = true; // si es true se está mostrando el precio de venta, sino el costo neto

var bloqueadorMarca = false;
var bloqueadorTipoAuto = false;
var ocultarBody = false;

$(document).ready(function () {
    helperJson();
    helperPrecio();
    ObtenerMarcasPorRubro();

    var configCandado = obtenerCandado();
    // console.log(configCandado);
    bloqueadorMarca = configCandado.marca;
    bloqueadorTipoAuto = configCandado.tipo;

    var configOcultar = obtenerOcultarBody();
    // console.log(configOcultar);
    ocultarBody = configOcultar.ocultar;

    ocultarTbody();

    configurarCandado();

    Handlebars.registerHelper('formatOferta', function (producto) {
        if (producto.TieneOferta)
            return 'oferta';
        return '';
    });

    $(document).keydown(function (e) {
        var y = $('.contenedor-tabla-productos').scrollTop();

        switch (e.which) {
            case 38: // up
                $('.contenedor-tabla-productos').scrollTop(y - 25);
                break;
            case 39: // right
                break;
            case 40: // down
                $('.contenedor-tabla-productos').scrollTop(y + 25);
                break;
            default: return; // exit this handler for other keys
        }
    });

    inicializarKeynavigator();

    seleccionarPrimero();

    //bloqueado momentaneamente...// $('.table-productos tbody tr:first').addClass('activo').click();

    $('#detalle-producto').on('click', '.btnCambiar', function () {
        actualizarIndicadorPrecio();
    });

    $('#formFiltros').on('click', '.btnBloquearMarca', function () {
        if (bloqueadorMarca === false) {
            bloqueadorMarca = true;
            $('#candado').removeClass('icon-lock-open');
            $('#candado').addClass('icon-lock');
            actualizarCandados(bloqueadorMarca, bloqueadorTipoAuto);
        } else if (bloqueadorMarca === true) {
            bloqueadorMarca = false;
            $('#candado').addClass('icon-lock-open');
            $('#candado').removeClass('icon-lock');
            actualizarCandados(bloqueadorMarca, bloqueadorTipoAuto);
        }
    });

    $('#formFiltros').on('click', '.btnBloquearTipoAuto', function () {
        if (bloqueadorTipoAuto === false) {
            bloqueadorTipoAuto = true;
            $('#candadoTipo').removeClass('icon-lock-open');
            $('#candadoTipo').addClass('icon-lock');
            actualizarCandados(bloqueadorMarca, bloqueadorTipoAuto);
        }
        else
            if (bloqueadorTipoAuto === true) {
                bloqueadorTipoAuto = false;
                $('#candadoTipo').addClass('icon-lock-open');
                $('#candadoTipo').removeClass('icon-lock');
                actualizarCandados(bloqueadorMarca, bloqueadorTipoAuto);
            }
    });

    $(".target").change(function () {
        if (bloqueadorMarca === false) {
            $.ajax({
                data: data,
                url: baseUrl('Catalogo/Index'),
                type: 'post',
                beforeSend: function () {
                }
            }).done(function (response) {
                if (typeof callback === 'function') {
                    callback();
                }
            });
        }
    });

    //<-- combos busqueda -->

    $('#formFiltros').on('click', '.btnBusca', function () {
        //$('#formFiltros').submit();
        //ocultarBody = true;
        //actualizarOcultarBody(ocultarBody)
        buscar();
    });

    $('.txtProducto').keypress(function (e) {
        if (e.which === 13) {
            ocultarBody = true;
            actualizarOcultarBody(ocultarBody);
        }
    });

    $('#CodigoRubro').change(function () {
        ObtenerMarcasPorRubro();
    });

    $('.cboMarca').change(function () {

        var codigoMarca = $('.cboMarca').val();

        let url = baseUrl('/Catalogo/TipoAutoPorMarca');

        if (bloqueadorTipoAuto === false) {
            $('.cboTipo option').remove();

            $.ajax({
                type: 'POST',
                url: url,

                dataType: 'json',

                data: { codigoMarca: codigoMarca },

                //success: function (states) {

                //    $.each(states, function (i, state) {
                //        $('.cboTipo').append('<option value=' + state.Value + ">" + state.Text + "</option>");
                //    });
                //},
                //error: function (ex) {
                //    alert('error al recibir marcas' + ex);
                //    console.log(ex);
                //}
            }).done(function () {
                $.each(states, function (i, state) {
                    $('.cboTipo').append('<option value=' + state.Value + ">" + state.Text + "</option>");
                });
            });

            return false;
        }
    });

    $('.cboFiltros').on('change', function () {
        // alert("buscar");
        buscar();
    });

    //fin combos busqueda
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

    $('.cerrar').click(function () {
        $(".modal").fadeOut(300);
    });

    $('#detalle-producto').on('click', '.btnAgregarCarrito', function () {
        var cantidad = $('.txtCantidadCarrito').val();
        var unidadVenta = $(this).data('unidadVenta');

        // console.log(unidadVenta);

        if (isNaN(cantidad) || cantidad < 1) {
            notificarWarning('Error: Ingrese una cantidad válida');
            return;
        }

        agregarAlCarrito(cantidad, _productoSeleccionado.CodigoPieza, true, unidadVenta, function () {
            notificarSuccess('Producto agregado al carrito');
        });

    });

    $('#detalle-producto').on('click', '.btnEquivalencia', function () {

        if (_productoEquivalencia) {

            // sacar las equivalencias
            $('.btnEquivalencia').text('Ver Equivalencias');
            $('#equivalenciasTableBody').hide();
            $('#productosTableBody').show();

            // volver a seleccionar el producto
            _productoSeleccionado = _productoEquivalencia;
            _productoEquivalencia = null;

            mostrarPrecio();

        } else {
            // cargar las equivalencias
            $('.btnEquivalencia').text('Volver a la Búsqueda');

            _productoEquivalencia = _productoSeleccionado;
            obtenerProductoEquivalentes();

            //_productoEquivalencia = null;
        }

        //if (equivalencia === true) {
        //    $('.btnEquivalencia').text('Volver a la Búsqueda');
        //    obtenerProductoEquivalentes();
        //    equivalencia = false;
        //}
        //else
        //    if (equivalencia === false) {
        //        $('.btnEquivalencia').text('Ver Equivalencias');
        //        $('#equivalenciasTableBody').hide();
        //        $('#productosTableBody').show();
        //        equivalencia = true;
        //    }
    });

    var $table = $('table.headerFixed');

    $table.floatThead({
        scrollContainer: function ($table) {
            return $table.closest('.wrapper');
        }
    });

    $('.contenedor-tabla-productos').on('scroll', function () {

        if (_productoEquivalencia) {
            return;
        }

        // console.log('Cargando página siguiente...');

        var $contenedor = $('.contenedor-tabla-productos')[0];

        var diferencia = 200;
        var top = $contenedor.scrollTop;
        var height = $contenedor.scrollHeight;
        var triggerOn = 470;

        //console.log(height - top + diferencia);

        if (height - top + diferencia < triggerOn) {
            if (_cargarPagina && hayPaginaSiguiente()) {
                cargarPaginaSiguiente();
                _cargarPagina = false;
            }
        }
    });

});

function seleccionarPrimero() {
    if ($('.table-productos tbody tr').length > 0) {
        $('.table-productos tbody tr').first().click();
    }
}

function buscar() {
    $('#formFiltros').submit();
    ocultarBody = true;
    actualizarOcultarBody(ocultarBody);
}

function ocultarTbody() {
    if (ocultarBody === false) {
        $('#productosTableBody').hide();
    } else
        if (ocultarBody === true) {
            $('#productosTableBody').show();
        }
}

function ObtenerMarcasPorRubro() {

    var codigoRubro = $('#CodigoRubro').val();
    var codigoMarca = $('#CodigoMarca').val();

    if ($('#CodigoRubro').val() === '') {
        return;
    }

    if (bloqueadorMarca === false) {
        $('.cboMarca option').remove();

        $.ajax({
            type: 'post',
            url: baseUrl('/Catalogo/MarcasPorRubro'),
            dataType: 'json',
            data: {
                codigoRubro: codigoRubro
            },
            success: function (states) {

                if (states && states.length > 0) {

                    states.forEach((item, index) => {
                        var selectedText = codigoMarca == item.Value ? 'selected' : '';
                        var option = `<option value="${item.Value}" ${selectedText}>${item.Text}</option>`;
                        $('.cboMarca').append(option);
                    });

                }
            }
        });

        return false;
    }
}

function cargarDatosProducto(producto) {

    // console.log('Producto cargado', producto);

    // verificamos si el producto ya fue cargado (para evitar agregar los parámetros manuales)
    if (!producto.GrupoParametros) {
        // agregar código artículo y nro original como parámetros manuales
        producto.Parametros.unshift({ Parametro: 'Nro. Original', Valor: producto.NumeroOriginal || '---' });
        producto.Parametros.unshift({ Parametro: 'Cod. Articulo', Valor: producto.CodigoPieza });
    }

    let cantidadPorColumna = 2;

    let divisionParametros = [];

    for (var i = 0; i < producto.Parametros.length; i += cantidadPorColumna) {
        divisionParametros.push(producto.Parametros.slice(i, i + cantidadPorColumna));
    }

    producto.GrupoParametros = divisionParametros;
    producto.TieneStock = producto.Stock > 3;

    var source = $("#template-detalle-producto").html();
    var template = Handlebars.compile(source);

    var html = template(producto);
    $('#detalle-producto').html(html);

    if (producto.TieneEquivalencias === true) {
        $('#detalle-producto .btnEquivalencia').addClass('btn-danger');
    }
    else {
        $('#detalle-producto .btnEquivalencia').prop('disabled', !producto.TieneEquivalencias);
        $('#detalle-producto .btnEquivalencia').removeClass('btn-danger', !producto.TieneEquivalencias);
    }

}

function cargarStock(codigoPieza) {
    var url = baseUrl('/Catalogo/ObtenerStock');
    $.getJSON(url, { codigoPieza: codigoPieza }, function (data, txtStatus, jqxhr) {
        // setar stock
        var $stockDisp = $('.stockDisponible');
        var $stockNoDisp = $('.stockNoDisponible');

        if (data.EstadoStock === 'EN_STOCK') {
            $stockDisp.removeClass('gris').addClass('verde');
            $stockNoDisp.removeClass('amarillo').addClass('gris');
        }
        else if (data.EstadoStock === 'STOCK_MINIMO') {
            $stockDisp.removeClass('verde').addClass('gris');
            $stockNoDisp.removeClass('gris').addClass('amarillo');
        }

    });
}

function inicializarKeynavigator(e) {
    $('table#tablaProducto > tbody tr').keynavigator({
        cycle: false,
        activeClass: 'activo',
        onAfterActive: function ($tr) {
            var nuevoProductoSeleccionado = $tr.data('producto');           

            console.log('fddfsf');

            if (_productoSeleccionado === null || _productoSeleccionado.CodigoPieza !== nuevoProductoSeleccionado.CodigoPieza) {
                _productoSeleccionado = nuevoProductoSeleccionado;
                cargarDatosProducto(_productoSeleccionado);
                cargarStock(_productoSeleccionado.CodigoPieza);
                mostrarPrecio();
            }
        }
    });
}

function inicializarKeynavigatorEquivalencias() {
    $('table#tablaProducto > #equivalenciasTableBody tr').keynavigator({
        cycle: false,
        activeClass: 'activo',
        onAfterActive: function ($tr) {
            var nuevoProductoSeleccionado = $tr.data('producto');

            if (_productoSeleccionado === null || _productoSeleccionado.CodigoPieza !== nuevoProductoSeleccionado.CodigoPieza) {
                _productoSeleccionado = nuevoProductoSeleccionado;
                cargarDatosProducto(_productoSeleccionado);
                cargarStock(_productoSeleccionado.CodigoPieza);
                $('.btnEquivalencia').text('Volver a la Búsqueda');
                mostrarPrecio();
            }
        }
    });
}

function cargarPaginaSiguiente() {

    // console.log('Cargando página siguiente...');

    var pagSig = Number.parseInt($('#Pagina').val()) + 1;
    var url = updateQueryStringParameter($('.linkPaginacion').val(), 'Pagina', pagSig);
    var $loading = $('#productos-loading');

    $loading.show();

    $.get(url, function (response) {

        var source = document.getElementById("template-filas-productos").innerHTML;
        var template = Handlebars.compile(source);
        var html = template({
            productos: response.Data.Productos,
            equivalencia: false
        });

        $('#ultimaPagina').val(response.Data.UltimaPagina);

        $('table.table-productos tbody').append(html);

        $('#Pagina').val(pagSig);
        _cargarPagina = true;
        $loading.hide();
        inicializarKeynavigator();
    });

}

function obtenerProductoEquivalentes() {

    if (!_productoEquivalencia) {
        return;
    }

    //var codPieza = _productoSeleccionado.CodigoPieza;

    $.get(baseUrl('Catalogo/Equivalencias'), { codigoPieza: _productoEquivalencia.CodigoPieza }, function (response) {

        response.unshift(_productoEquivalencia);

        var source = document.getElementById("template-filas-productos").innerHTML;
        var template = Handlebars.compile(source);
        var html = template({
            productos: response,
            equivalencia: true
        });

        var $tbody = $('#equivalenciasTableBody');
        $tbody.html(html);

        $('#productosTableBody').hide();
        $('#equivalenciasTableBody').show();

        inicializarKeynavigatorEquivalencias();

        var $tbodyEquivalencia = $('.table-productos #equivalenciasTableBody');
        var $firstRowEquivalencia = $tbodyEquivalencia.find('tr:first');
        //$('.table-productos #equivalenciasTableBody tr:first').addClass('activo').click();

        // agrega fila equivalencia
        $firstRowEquivalencia.after('<tr class="filaEquivalencia"><td>&nbsp;</td><td colspan="5"><i class="icon-down-1"></i>EQUIVALENCIAS<i class="icon-down-1"></i></td></tr>');
        $firstRowEquivalencia.addClass('activo').click();
        // equivalencia = false;
        //_productoEquivalencia = null;
    });
}

function hayPaginaSiguiente() {
    return $('#ultimaPagina').val() === 'false';
}

function configurarCandado() {
    if (bloqueadorMarca === true) {
        $('#candado').removeClass('icon-lock-open');
        $('#candado').addClass('icon-lock');
    }
    else {
        $('#candado').addClass('icon-lock-open');
        $('#candado').removeClass('icon-lock');
    }

    if (bloqueadorTipoAuto === true) {
        $('#candadoTipo').removeClass('icon-lock-open');
        $('#candadoTipo').addClass('icon-lock');
    }
    else {
        $('#candadoTipo').addClass('icon-lock-open');
        $('#candadoTipo').removeClass('icon-lock');
    }
}

function mostrarPrecioVenta() {
    var precioVigente = _productoSeleccionado.PrecioVigente;
    var precioVenta = _calculadora.calcularPrecio(precioVigente);

    $('#icono').removeClass('icon-money');
    $('#icono').addClass('icon-tag-1');
    $("#txtPrec").html("Precio Venta: ");
    $('.btnCambiar').attr('title', 'Ver costo neto s/IVA');
    $('.txtPrecioProducto').val('$ ' + formatNumber(precioVenta));
}

function mostrarCostoNeto() {
    var precioVigente = _productoSeleccionado.PrecioVigente;
    var costoNeto = _calculadora.calcularPrecioNeto(precioVigente);

    $('#icono').removeClass('icon-tag-1');
    $('#icono').addClass('icon-money');
    $("#txtPrec").html("Costo Neto s/IVA: ");
    $('.btnCambiar').attr('title', 'Ver precio de venta');
    $('.txtPrecioProducto').val('$ ' + formatNumber(costoNeto));
}

function actualizarIndicadorPrecio() {
    if (_precio === true) {
        mostrarCostoNeto();
        _precio = false;

    } else {
        mostrarPrecioVenta();
        _precio = true;
    }
}

function mostrarPrecio() {
    if (_precio === true) {
        mostrarPrecioVenta();
    } else {
        mostrarCostoNeto();
    }
}