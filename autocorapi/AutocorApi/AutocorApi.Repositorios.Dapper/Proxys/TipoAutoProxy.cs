using AutocorApi.Entidades;

namespace AutocorApi.Repositorios.Dapper.Proxys
{
    internal class TipoAutoProxy : TipoAuto
    {
        private Marca _marca;

        public override Marca Marca
        {
            get
            {
                if (_marca == null && CodigoMarca != null)
                {
                    using (var repo = new RepositorioMarcas())
                    {
                        _marca = repo.BuscarPorCodigo(CodigoMarca);
                    }
                }

                return _marca;
            }
            set
            {
                _marca = value;
            }
        }
    }
}