using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Base;
using Dapper;

namespace AutocorApi.Repositorios.Dapper
{
    public class RepositorioActualizaciones : RepositorioBase, IRepositorioActualizaciones
    {
        private static readonly string _QueryVersion;

        static RepositorioActualizaciones()
        {
            _QueryVersion = @"SELECT
                              -- {{top}} TOP (@Top)
                                 v.ID as Id
                                ,v.FECHA as Fecha
                                ,v.REGISTROS as Registros
                             FROM VERSION v ";
        }

        public RepositorioActualizaciones() : base()
        {
        }

        public RepositorioActualizaciones(IDbTransaction transaction) : base(transaction)
        {
        }

        public Actualizacion BuscarPorId(int idActualizacion)
        {
            string query = _QueryVersion + " WHERE v.ID = @idActualizacion";
            return Connection.QuerySingleOrDefault<Actualizacion>(query, new { idActualizacion }, transaction: Transaction);
        }

        public int Count(DateTime? desde, DateTime? hasta)
        {
            DynamicParameters param = new DynamicParameters();
            string query = "SELECT COUNT(*) FROM VERSION v WHERE 1 = 1 ";

            if (desde.HasValue)
            {
                query += " AND v.FECHA >= @Desde ";
                param.Add("Desde", desde);
            }

            if (hasta.HasValue)
            {
                query += " AND v.FECHA <= @Hasta ";
                param.Add("Hasta", hasta);
            }

            return Connection.ExecuteScalar<int>(query, param: param, transaction: Transaction);
        }

        public int Count()
        {
            return Count(null, null);
        }

        public virtual void Insertar(Actualizacion actualizacion)
        {
            string sql = @"INSERT INTO VERSION
                               (FECHA, REGISTROS)
                         VALUES
                               (@Fecha, @Registros);
                         SELECT CAST(SCOPE_IDENTITY() AS INT)";

            actualizacion.Id = Connection.ExecuteScalar<int>(sql, actualizacion);
        }

        public virtual Actualizacion ObtenerUltimaActualizacion()
        {
            return ObtenerUltimasActualizaciones(1).FirstOrDefault();
        }

        public IEnumerable<Actualizacion> ObtenerUltimasActualizaciones(int cantidad)
        {
            string query = _QueryVersion.Replace("-- {{top}}", string.Empty) +
                    "ORDER BY v.FECHA DESC";

            return Connection.Query<Actualizacion>(query, param: new { Top = cantidad }, transaction: Transaction);
        }
    }
}