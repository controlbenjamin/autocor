using System.Collections.Generic;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Cache;

namespace AutocorApi.Repositorios.Dapper.Proxys
{
    internal class ProductoProxy : Producto
    {
        private bool? _tieneEquivalencia;
        private Marca _marca;
        private TipoAuto _tipoAuto;
        private Rubro _rubro;
        private IEnumerable<ParametroProducto> _parametros;

        public override IEnumerable<ParametroProducto> Parametros
        {
            get
            {
                if (base.Parametros == null)
                {
                    if (_parametros == null && CodigoPieza != null)
                    {
                        using (var repo = new RepositorioProductosCache())
                        {
                            _parametros = repo.ObtenerParametrosProducto(CodigoPieza);
                        }
                    }

                    base.Parametros = _parametros;
                    return base.Parametros;
                }

                return base.Parametros;
            }
            set
            {
                base.Parametros = value;
            }
        }

        public override Marca Marca
        {
            get
            {
                if (_marca == null && CodigoMarca != null)
                {
                    using (var repo = new RepositorioMarcasCache())
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

        public override TipoAuto TipoAuto
        {
            get
            {
                if (_tipoAuto == null)
                {
                    using (var repo = new RepositorioTiposAutoCache())
                    {
                        _tipoAuto = repo.BuscarPorCodigo(CodigoTipoAuto);
                    }
                }

                return _tipoAuto;
            }
            set
            {
                _tipoAuto = value;
            }
        }

        public override Rubro Rubro
        {
            get
            {
                if (_rubro == null)
                {
                    using (var repo = new RepositorioRubrosCache())
                    {
                        _rubro = repo.BuscarPorCodigo(CodigoRubro);
                    }
                }

                return _rubro;
            }
            set
            {
                _rubro = value;
            }
        }

        public override bool TieneEquivalencias
        {
            get
            {
                if (!_tieneEquivalencia.HasValue)
                {
                    using (var repo = new RepositorioProductosCache())
                    {
                        _tieneEquivalencia = repo.VerificarExistenciaDeProductosEquivalentes(CodigoPieza);
                        base.TieneEquivalencias = _tieneEquivalencia.Value;
                    }
                }

                return base.TieneEquivalencias;
            }
            set
            {
                _tieneEquivalencia = value;
                base.TieneEquivalencias = value;
            }
        }
    }
}