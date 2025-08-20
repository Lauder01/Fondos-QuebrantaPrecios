using System;

namespace ClassLibraryProject.Entities
{
    /// <summary>
    /// Representa un usuario de la aplicación, incluyendo sus datos de acceso y estado.
    /// </summary>
    public class FQP_User
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// Nombre de usuario para el acceso.
        /// </summary>
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Nombre real del usuario (opcional).
        /// </summary>
        public string? FirstName { get; set; } = string.Empty;
        /// <summary>
        /// Apellido real del usuario (opcional).
        /// </summary>
        public string? LastName { get; set; } = string.Empty;
        /// <summary>
        /// Hash de la contraseña del usuario.
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;
        /// <summary>
        /// Indica si el usuario está activo.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Constructor por defecto requerido por Entity Framework.
        /// </summary>
        public FQP_User() { }
    }
}
