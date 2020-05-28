using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AutocorApi.Models.Carrito
{
    public class CarritoClienteModel : IValidatableObject
    {
        public int CodigoCliente { get; set; }
        public IEnumerable<ItemCarritoModel> Items { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CodigoCliente <= 0)
                yield return new ValidationResult("Cliente no válido", new[] { "CodigoCliente" });

            if (Items == null || Items.Count() == 0)
                yield return new ValidationResult("No hay items", new[] { "Items" });
        }
    }

    public class ItemCarritoModel
    {
        public string CodigoPieza { get; set; }
        public int Cantidad { get; set; }
    }
}