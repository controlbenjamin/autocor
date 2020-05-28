using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Cache;

namespace AutocorApi.Repositorios.Dapper.Proxys
{
    internal class PedidoProxy : Pedido
    {
        private ClienteBase _cliente;
        private EstadoPedido _estadoPedido;

        public override ClienteBase Cliente
        {
            get
            {
                if (_cliente == null)
                {
                    using (var repo = new Cache.RepositorioClientesCache())
                    {
                        _cliente = repo.BuscarPorCodigo(CodigoCliente);
                    }
                }

                return _cliente;
            }

            set => _cliente = value;
        }

        public override EstadoPedido Estado
        {
            get
            {
                if(_estadoPedido == null)
                {
                    using (var repo = new RepositorioEstadosPedidos())
                    {
                        _estadoPedido = repo.ObtenerPorId(IdEstadoPedido);
                    }
                }

                return _estadoPedido;
            }
            set => _estadoPedido = value;
        }
    }
}