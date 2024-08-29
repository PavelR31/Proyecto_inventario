namespace Proyecto_inventario.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Propiedad de navegación para la relación con usuarios
        public List<UsuarioRol> UsuarioRoles { get; set; }
    }
}
