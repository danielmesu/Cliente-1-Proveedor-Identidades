namespace AplicacionCliente_1.Models
{
    /// <summary>
    /// Modelo para la vista de errores.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Id de la solicitud.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Indica si se cuenta con el id de la solicitud o no.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}