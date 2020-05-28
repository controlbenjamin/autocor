using System.Data;
using AutocorApi.Repositorios.Base;

namespace AutocorApi.Repositorios.Dapper.Base
{
    public abstract class RepositorioBase : IRepositorio
    {
        private IDbTransaction _transaction;

        protected IDbConnection Connection
        {
            get;
            private set;
        }

        protected IDbTransaction Transaction
        {
            get
            {
                return _transaction;
            }
            private set
            {
                _transaction = value;

                if (_transaction != null) { Connection = _transaction.Connection; }
            }
        }

        public RepositorioBase(IDbTransaction transaction)
        {
            this.Transaction = transaction;
        }

        public RepositorioBase()
        {
            ConexionRepositorio conexionRepo = new ConexionRepositorio();
            this.Connection = conexionRepo.CrearConexion();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Transaction != null)
                {
                    Transaction.Dispose();
                }

                if (Connection != null)
                {
                    Connection.Close();
                    Connection.Dispose();
                }
            }
        }
    }
}