using AutocorApi.Entidades;
using AutocorApi.Repositorios;
using AutocorApi.Servicios.Dto;
using AutoMapper;
using System.Security.Cryptography;
using System.Text;

namespace AutocorApi.Servicios.Core.Implementation
{
    public class ServicioToken : IServicioToken
    {
        private readonly IRepositorioRefreshTokens _repoTokens;

        public ServicioToken(IRepositorioRefreshTokens repositorioRefreshTokens)
        {
            _repoTokens = repositorioRefreshTokens;
        }

        public void EliminarToken(string idToken)
        {
            _repoTokens.Eliminar(idToken);
        }

        public void EliminarToken(string idToken, int idUsuario)
        {
            _repoTokens.Eliminar(idToken, idUsuario: idUsuario);
        }

        public string HashGuid(string token)
        {
            using (MD5 md5 = MD5.Create())
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                StringBuilder sb = new StringBuilder();
                byte[] stream = md5.ComputeHash(encoding.GetBytes(token));
                for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
                return sb.ToString();
            }
        }

        public RefreshTokenDto ObtenerTokenPorId(string id)
        {
            var token = _repoTokens.ObtenerPorId(id);
            return Mapper.Map<RefreshToken, RefreshTokenDto>(token);
        }

        public void RegistrarToken(RefreshTokenDto refreshToken)
        {
            var token = Mapper.Map<RefreshTokenDto, RefreshToken>(refreshToken);
            _repoTokens.Insertar(token);
        }
    }
}