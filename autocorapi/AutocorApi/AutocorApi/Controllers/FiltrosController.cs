using AutocorApi.Servicios.Core;
using AutocorApi.Servicios.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace AutocorApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/carrito")]
    public class FiltrosController : ApiController
    {
        private IServicioRubros servicioRubros;
        private IServicioMarcas servicioMarcas;
        private IServicioTiposAuto servicioTiposAuto;

        public FiltrosController(IServicioRubros servicioRubros, IServicioMarcas servicioMarcas, IServicioTiposAuto servicioTiposAuto)
        {
            this.servicioRubros = servicioRubros;
            this.servicioMarcas = servicioMarcas;
            this.servicioTiposAuto = servicioTiposAuto;
        }

        [HttpGet]
        [ResponseType(typeof(FiltrosDto))]
        public IHttpActionResult GetFiltros(int? rubro = null, string marca = null, int? tipoAuto = null)
        {
            rubro = rubro.HasValue && rubro > 0 ? rubro: null;
            tipoAuto = tipoAuto.HasValue && tipoAuto > 0 ? tipoAuto : null;

            var rubros = new List<RubroMinDto>();
            var marcas = new List<MarcaDto>();
            var tipos = new List<TipoAutoMinDto>();

            // buscar rubros
            //if(rubro.HasValue)
            //{
            //    var auxRubro = servicioRubros.BuscarMinPorCodigo(rubro.Value);
                
            //    if(auxRubro != null)
            //    {
            //        rubros.Add(auxRubro);
            //    }
            //}
            //else
            //{
            //    rubros = servicioRubros.ObtenerRubrosMin().ToList();
            //}

            rubros = servicioRubros.ObtenerRubrosMin().ToList();

            // buscar marcas
            if (!string.IsNullOrEmpty(marca))
            {
                var auxMarca = servicioMarcas.BuscarPorCodigo(marca);

                if(auxMarca != null)
                {
                    marcas.Add(auxMarca);
                }
            }
            else
            {
                marcas = servicioMarcas.ObtenerMarcas(rubro).ToList();
            }


            // buscar tipos de auto
            if (tipoAuto.HasValue)
            {
                var auxTipo = servicioTiposAuto.BuscarMinPorCodigo(tipoAuto.Value);

                if (auxTipo != null)
                {
                    tipos.Add(auxTipo);
                }
            }
            else
            {
                tipos = servicioTiposAuto.ObtenerTiposAutoMin(codigoMarca: marca).ToList();
            }

            var filtro = new FiltrosDto()
            {
                Rubros = rubros,
                Marcas = marcas,
                TiposAuto = tipos
            };

            return Ok(filtro);
        }
    }
}
