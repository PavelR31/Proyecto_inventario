using System.ComponentModel.DataAnnotations;  


namespace Proyecto_inventario.Models
{
    public class ContratoAlquiler
    {
        public int Id { get; set; }

        public int PropiedadId { get; set; }
        public Propiedad Propiedad { get; set; }

        public int InquilinoId { get; set; }
        public Usuario Inquilino { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        [Required]
        public decimal Monto { get; set; }
    }
}
