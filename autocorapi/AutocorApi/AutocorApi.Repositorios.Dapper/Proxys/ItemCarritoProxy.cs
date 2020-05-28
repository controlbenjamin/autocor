using AutocorApi.Entidades;

namespace AutocorApi.Repositorios.Dapper.Proxys
{
    internal class ItemCarritoProxy : ItemCarrito
    {
        private Cliente _cliente;
        private Producto _producto;

        public override Cliente Cliente
        {
            get
            {
                if(_cliente == null)
                {
                    using (var repo = new Cache.RepositorioClientesCache())
                    {
                        _cliente = repo.BuscarPorCodigo(CodigoCliente);
                    }
                }

                return _cliente;
            }
            set { _cliente = value; }
        }

        public override Producto Producto
        {
            get
            {
                if (_producto == null)
                {
                    using (var repo = new Cache.RepositorioProductosCache())
                    {
                        _producto = repo.BuscarPorCodigo(base.CodigoPieza);
                    }
                }

                return _producto;
            }
            set { _producto = value; }
        }
    }
}