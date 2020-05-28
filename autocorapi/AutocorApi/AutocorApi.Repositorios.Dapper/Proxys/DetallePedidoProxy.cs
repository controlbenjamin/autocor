using AutocorApi.Entidades;

namespace AutocorApi.Repositorios.Dapper.Proxys
{
    internal class DetallePedidoProxy : DetallePedido
    {
        private Producto _producto;

        public override Producto Producto
        {
            get
            {
                if(_producto == null)
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