
using System.ComponentModel.DataAnnotations;

namespace Proyecto_inventario.Models
{
    public class Propiedad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int PropietarioId { get; set; }

        // Propiedad de navegación
        public Propietario Propietario { get; set; }
    }

}
