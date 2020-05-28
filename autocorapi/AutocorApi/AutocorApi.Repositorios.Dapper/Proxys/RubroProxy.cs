using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Cache;
using System.Collections.Generic;

namespace AutocorApi.Repositorios.Dapper.Proxys
{
    internal class RubroProxy : Rubro
    {
        private IEnumerable<string> _listaParametros;

        public override IEnumerable<string> ListaParametros
        {
            get
            {
                if (_listaParametros == null)
                {
                    using (var repo = new RepositorioRubrosCache())
                    {
                        _listaParametros = repo.ObtenerParametrosPorRubro(Codigo);
                    }
                }

                return _listaParametros;
            }
            set
            {
                _listaParametros = value;
            }
        }
    }
}