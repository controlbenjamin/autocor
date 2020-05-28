using System;

namespace AutocorApi.Entidades
{
    public class RefreshToken
    {
        public string Id { get; set; }
        public int IdUsuario { get; set; }
        public string ClientId { get; set; }
        public DateTime IssuedUTC { get; set; }
        public DateTime ExpiresUTC { get; set; }
        public string ProtectedTicket { get; set; }
    }
}