using System.ComponentModel.DataAnnotations;

namespace Proyecto_inventario.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Clave { get; set; }

        public List<UsuarioRol> UsuarioRoles { get; set; }
    }
}
