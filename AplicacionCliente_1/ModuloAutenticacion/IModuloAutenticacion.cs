namespace AplicacionCliente_1.ModuloAutenticacion
{
    /// <summary>
    /// Interfaz para un módulo de autenticación por medio de un proveedor de identidades.
    /// </summary>
    public interface IModuloAutenticacion
    {
        /// <summary>
        /// Método que ejecuta el proceso de autenticación de la sesión del usuario.
        /// </summary>
        /// <param name="ContextoActual">Contexto actual del sistema de peticiones http.</param>
        /// <param name="UserName">Nombre de usuario del login.</param>
        /// <param name="UserPassword">Contraseña del usuario.</param>
        /// <returns>Proceso asíncrono con el resultado de la autenticación.</returns>
        Task<bool> AutenticarUsuario(HttpContext ContextoActual, string UserName, string UserPassword);
    }
}