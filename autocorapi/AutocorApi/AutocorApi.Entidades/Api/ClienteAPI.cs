using System;
using System.Collections;
using System.Collections.Generic;

namespace AutocorApi.Entidades.Api
{
    public class ClienteAPI
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public string DescripcionRoles { get; set; }
        public IEnumerable<Rol> Roles
        {
            get
            {
                // si no tiene rol devuelve todos los roles
                if (string.IsNullOrEmpty(DescripcionRoles))
                    return Enum.GetValues(typeof(Rol)) as IEnumerable<Rol>;

                string[] split = DescripcionRoles.Split(',');
                List<Rol> roles = new List<Rol>();

                foreach (var splitItem in split)
                {
                    Rol rol;
                    if (Enum.TryParse(splitItem.Trim(), out rol))
                    {
                        roles.Add(rol);
                    }
                }

                return roles;
            }
        }

    }
}