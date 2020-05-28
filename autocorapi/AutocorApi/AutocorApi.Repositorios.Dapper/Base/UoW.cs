using System;
using System.Data;
using AutocorApi.Repositorios.Base;
using AutocorApi.Repositorios.Dapper.Cache;

namespace AutocorApi.Repositorios.Dapper.Base
{
    public class UoW : IUoW
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool _disposed;

        // repositorios
        private IRepositorioClientes repositorioClientes;
        private IRepositorioMarcas repositorioMarcas;
        private IRepositorioProductos repositorioProductos;
        private IRepositorioRubros repositorioRubros;
        private IRepositorioTiposAuto repositorioTiposAuto;
        private IRepositorioDescargas rpeositorioDescargas;
        private IRepositorioEstadosPedidos repositorioEstadosPedidos;
        private IRepositorioPedidos repositorioPedidos;
        private IRepositorioDetallesPedidos repositorioDetallesPedidos;
        private IRepositorioCarritos repositorioCarritos;
        private IRepositorioConfiguracionesClientes repositorioConfiguracionesClientes;
        private IRepositorioUsuarios repositorioUsuarios;
        private IRepositorioActualizaciones repositorioActualizaciones;
        private IRepositorioActivaciones repositorioActivaciones;
        private IRepositorioInstalaciones repositorioInstalaciones;
        private IRepositorioUsuariosWeb repositorioUsuariosWeb;


        public UoW()
        {
            ConexionRepositorio conexion = new ConexionRepositorio();
            this._connection = conexion.CrearConexion(true);
            this._transaction = _connection.BeginTransaction();
        }

        public IRepositorioClientes RepositorioClientes => repositorioClientes ?? (repositorioClientes = new RepositorioClientes(_transaction));

        public IRepositorioMarcas RepositorioMarcas => repositorioMarcas ?? (repositorioMarcas = new RepositorioMarcas(_transaction));

        public IRepositorioProductos RepositorioProductos => repositorioProductos ?? (repositorioProductos = new RepositorioProductosCache(_transaction));

        public IRepositorioRubros RepositorioRubros => repositorioRubros ?? (repositorioRubros = new RepositorioRubros(_transaction));

        public IRepositorioTiposAuto RepositorioTiposAuto => repositorioTiposAuto ?? (repositorioTiposAuto = new RepositorioTiposAuto(_transaction));

        public IRepositorioDescargas RepositorioDescargas => rpeositorioDescargas ?? (rpeositorioDescargas = new RepositorioDescargas(_transaction));

        public IRepositorioEstadosPedidos RepositorioEstadosPedidos => repositorioEstadosPedidos ?? (repositorioEstadosPedidos = new RepositorioEstadosPedidos(_transaction));

        public IRepositorioPedidos RepositorioPedidos => repositorioPedidos ?? (repositorioPedidos = new RepositorioPedidos(_transaction));

        public IRepositorioDetallesPedidos RepositorioDetallesPedidos => repositorioDetallesPedidos ?? (repositorioDetallesPedidos = new RepositorioDetallesPedidos(_transaction));

        public IRepositorioCarritos RepositorioCarritos => repositorioCarritos ?? (repositorioCarritos = new RepositorioCarritos(_transaction));

        public IRepositorioConfiguracionesClientes RepositorioConfiguracionesClientes  => repositorioConfiguracionesClientes ?? (repositorioConfiguracionesClientes = new RepositorioConfiguracionesClientes(_transaction));

        public IRepositorioUsuarios RepositorioUsuarios => repositorioUsuarios ?? (repositorioUsuarios = new RepositorioUsuarios(_transaction));

        public IRepositorioActualizaciones RepositorioActualizaciones => repositorioActualizaciones ?? (repositorioActualizaciones = new RepositorioActualizacionesCache(_transaction));

        public IRepositorioActivaciones RepositorioActivaciones => repositorioActivaciones ?? (repositorioActivaciones = new RepositorioActivaciones(_transaction));

        public IRepositorioInstalaciones RepositorioInstalaciones => repositorioInstalaciones ?? (repositorioInstalaciones = new RepositorioInstalaciones(_transaction));
        public IRepositorioUsuariosWeb RepositorioUsuariosWeb => repositorioUsuariosWeb ?? (repositorioUsuariosWeb = new RepositorioUsuariosWeb(_transaction));


        public void Commit()
        {
            try
            {
                this._transaction.Commit();
            }
            catch
            {
                this._transaction.Rollback();
                throw;
            }
            finally
            {
                this._transaction.Dispose();
                this._transaction = _connection.BeginTransaction();
                this.ResetarRepositorios();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Resetea todos los repositorios poniéndolos en null
        /// </summary>
        private void ResetarRepositorios()
        {
            this.repositorioClientes = null;
            this.repositorioMarcas = null;
            this.repositorioProductos = null;
            this.repositorioRubros = null;
            this.repositorioTiposAuto = null;
            this.rpeositorioDescargas = null;
            this.repositorioEstadosPedidos = null;
            this.repositorioPedidos = null;
            this.repositorioDetallesPedidos = null;
            this.repositorioCarritos = null;
            this.repositorioConfiguracionesClientes = null;
            this.repositorioActualizaciones = null;
            this.repositorioUsuarios = null;
            this.repositorioActivaciones = null;
            this.repositorioInstalaciones = null;
            this.repositorioUsuariosWeb = null;
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UoW()
        {
            Dispose(false);
        }
    }
}