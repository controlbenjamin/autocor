using System;
using System.Collections.Generic;
using System.Data;
using AutocorApi.Entidades;
using AutocorApi.Repositorios.Dapper.Base;
using AutocorApi.Repositorios.Utils;
using Dapper;

namespace AutocorApi.Repositorios.Dapper
{
    public class RepositorioIniciosSesion : RepositorioBase, IRepositorioIniciosSesion
    {
        private static string _QueryInicioSesion;

        static RepositorioIniciosSesion()
        {
            _QueryInicioSesion = @"SELECT ses.CODCLI as CodigoCliente
                                      ,ses.FECHA as Fecha
                                      ,ses.TIPO_CATALOGO as TipoCatalogo
                                      ,ses.PC as NombrePC
                                      ,ses.USUARIO as UsuarioPC
                                      ,ses.CODCLI_ANTERIOR as CodigoClienteAnterior
                                  FROM INICIOS_SESION ses ";
        }

        public RepositorioIniciosSesion()
            : base()
        {
        }

        public RepositorioIniciosSesion(IDbTransaction transaction)
            : base(transaction)
        {
        }

        public IEnumerable<InicioSesion> ObtenerIniciosSesion(int? codigoCliente, string tipoCatalogo,
            DateTime? fechaDesde, DateTime? fechaHasta, int? codigoClienteAnterior, bool soloCambiosUsuario,
            PageConfig page = null)
        {
            var parameters = new DynamicParameters();

            string query = _QueryInicioSesion
                + CrearWhereObtenerIniciosSesion(parameters, codigoCliente, tipoCatalogo,
                    fechaDesde, fechaHasta, codigoClienteAnterior, soloCambiosUsuario);

            // agregar orden
            query += " ORDER BY ses.FECHA DESC ";

            // agregar paginación
            if (page != null)
            {
                query += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY ";
                parameters.AddDynamicParams(new { Offset = page.Offset, PageSize = page.PageSize });
            }

            return Connection.Query<InicioSesion>(query, param: parameters, transaction: Transaction);
        }

        public void Insertar(InicioSesion inicioSesion)
        {
            string sql = @"INSERT INTO INICIOS_SESION
                                   (CODCLI ,FECHA ,TIPO_CATALOGO ,PC ,USUARIO ,CODCLI_ANTERIOR)
                             VALUES
                                   (@CodigoCliente ,@Fecha ,@TipoCatalogo ,@NombrePC ,@UsuarioPC ,@CodigoClienteAnterior)";

            Connection.Execute(sql, inicioSesion, transaction: Transaction);
        }

        private string CrearWhereObtenerIniciosSesion(DynamicParameters parameters, int? codigoCliente, string tipoCatalogo,
            DateTime? fechaDesde, DateTime? fechaHasta, int? codigoClienteAnterior, bool soloCambiosUsuario)
        {
            string where = "WHERE 1 = 1 ";

            if (codigoCliente.HasValue)
            {
                parameters.Add("Cliente", codigoCliente.Value);
                where += " AND ses.CODCLI = @Cliente ";
            }

            if (!string.IsNullOrEmpty(tipoCatalogo))
            {
                where += " AND ses.TIPO_CATALOGO = @TipoCatalogo";
                parameters.Add("TipoCatalogo", tipoCatalogo);
            }

            if (fechaDesde.HasValue)
            {
                where += " AND ses.FECHA >= @Desde ";
                parameters.Add("Desde", fechaDesde.Value);
            }

            if (fechaHasta.HasValue)
            {
                where += " AND ses.FECHA <= @Hasta ";
                parameters.Add("Hasta", fechaHasta.Value);
            }

            if (codigoClienteAnterior.HasValue)
            {
                where += " AND ses.CODCLI_ANTERIOR = @ClienteAnterior ";
                parameters.Add("ClienteAnterior", codigoClienteAnterior.Value);
            }

            if (soloCambiosUsuario)
            {
                where += " AND ses.CODCLI_ANTERIOR IS NOT NULL ";
            }

            return where;
        }

        public int CountObtenerIniciosSesion(int? codigoCliente, string tipoCatalogo,
            DateTime? fechaDesde, DateTime? fechaHasta, int? codigoClienteAnterior, bool soloCambiosUsuario)
        {
            var parameters = new DynamicParameters();
            string query = @"SELECT COUNT(*) FROM INICIOS_SESION ses ";

            query += CrearWhereObtenerIniciosSesion(parameters, codigoCliente, tipoCatalogo,
                    fechaDesde, fechaHasta, codigoClienteAnterior, soloCambiosUsuario);

            return Connection.ExecuteScalar<int>(query, param: parameters, transaction: Transaction);
        }
    }
}