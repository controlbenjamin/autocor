using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutocorApi.Repositorios;
using AutocorApi.Servicios.Dto;
using AutoMapper;

namespace AutocorApi.Servicios.Core.Implementation
{
    public class ServicioDescargas : IServicioDescargas
    {
        private IRepositorioDescargas _repoDescargas;

        public ServicioDescargas(IRepositorioDescargas repoDescargas)
        {
            this._repoDescargas = repoDescargas;
        }

        public IEnumerable<DescargaDto> ObtenerDescargas()
        {
            var descargas = _repoDescargas.ObtenerDescargas();
            return Mapper.Map<IEnumerable<DescargaDto>>(descargas);
        }

        public DescargaDto ObtenerPorId(int idDescarga)
        {
            var descarga = _repoDescargas.ObtenerPorId(idDescarga);
            return Mapper.Map<DescargaDto>(descarga);
        }
    }
}
