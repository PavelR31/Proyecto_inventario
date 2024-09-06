using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_inventario.Models
{
    public class FincaTerreno
    {
        
        public int Id { get; set; } // Sin auto-incremento

        [Required]
        [MaxLength(255)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(255)]
        public string Direccion { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Area { get; set; }

        [Required]
        [MaxLength(10)]
        public string Tipo { get; set; } // Debe corresponder a 'Finca' o 'Terreno'

        [Required]
        public int PropietarioId { get; set; }

        [ForeignKey("PropietarioId")]
        public Propietario Propietario { get; set; } // Relación con la entidad Propietario
    }
}
