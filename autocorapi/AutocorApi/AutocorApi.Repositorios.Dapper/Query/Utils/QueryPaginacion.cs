using AutocorApi.Repositorios.Utils;
using Dapper;

namespace AutocorApi.Repositorios.Dapper.Query.Utils
{
    internal class QueryPaginacion
    {
        public static string CrearPaginacion(DynamicParameters parametros, PageConfig pagination)
        {
            string queryPaginacion = " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY ";
            parametros.AddDynamicParams(new { Offset = pagination.Offset, PageSize = pagination.PageSize });
            return queryPaginacion;
        }
    }
}