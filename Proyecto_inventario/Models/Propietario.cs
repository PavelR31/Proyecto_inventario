namespace Proyecto_inventario.Models
{
    public class Propietario
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        // Propiedad de navegación
        public Usuario Usuario { get; set; }

        // Colección de propiedades
        public ICollection<Propiedad> Propiedades { get; set; }
        public ICollection<FincaTerreno> fincasTerrenos  { get; set; }

    }
}
