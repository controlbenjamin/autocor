using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutocorApi.Entidades;

namespace AutocorApi.Repositorios
{
    public interface IRepositorioActivaciones : IRepositorio
    {
        Activacion BuscarActivacion(int idUsuario);
        void Insertar(int idUsuario);
    }
}
