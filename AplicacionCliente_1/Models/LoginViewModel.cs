namespace AplicacionCliente_1.Models
{
    /// <summary>
    /// Modelo con los datos de las credenciales de autenticación de un usuario.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        public string? Username { get; set; }
        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        public string? Password { get; set; }
    }
}