namespace AplicacionCliente_1.Models
{
    /// <summary>
    /// Modelo que define la estructura de respuesta del servicio del proveedor de identidades.
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// Token de acceso generado por el proveedor.
        /// </summary>
        public string? access_token { get; set; }
        /// <summary>
        /// Tipo de token entregado por el proveedor.
        /// </summary>
        public string? token_type { get; set; }
        /// <summary>
        /// Tiempo de vigencia del token entregado.
        /// </summary>
        public int? expires_in { get; set; }
    }
}