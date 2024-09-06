using System.ComponentModel.DataAnnotations;  // Agrega esta línea


namespace Proyecto_inventario.Models

{
    public class Pago
    {
        public int Id { get; set; }

        public int ContratoAlquilerId { get; set; }
        public ContratoAlquiler ContratoAlquiler { get; set; }

        [Required]
        public DateTime FechaPago { get; set; }

        [Required]
        public decimal Monto { get; set; }

        [Required]
        public bool Pagado { get; set; }
    }
}
