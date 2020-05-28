using System;

namespace AutocorApi.Servicios.Dto
{
    public class ActualizacionDto
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public int Registros { get; set; }
    }
}