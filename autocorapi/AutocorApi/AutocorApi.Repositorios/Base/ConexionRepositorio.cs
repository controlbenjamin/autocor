using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AutocorApi.Repositorios.Base
{
    public class ConexionRepositorio
    {
        private const string ConnectionStringKey = "db_autocor";

        static ConexionRepositorio()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
        }

        protected static string ConnectionString { get; }

        public virtual IDbConnection CrearConexion()
        {
            return this.CrearConexion(false);
        }

        public virtual IDbConnection CrearConexion(bool abierta)
        {
            IDbConnection connection = new SqlConnection(ConnectionString);

            if (abierta) connection.Open();

            return connection;
        }
    }
}