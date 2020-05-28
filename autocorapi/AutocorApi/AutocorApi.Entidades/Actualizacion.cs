using System;

namespace AutocorApi.Entidades
{
    /// <summary>
    /// Corresponde con la tabla VERSION
    /// </summary>
    public class Actualizacion
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public int Registros { get; set; }
    }
}