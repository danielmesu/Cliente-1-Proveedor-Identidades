using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text;
using AplicacionCliente_1.Models;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;

namespace AplicacionCliente_1.ModuloAutenticacion
{
    /// <summary>
    /// Módulo de autenticación por medio del proveedor de identidades interno.
    /// </summary>
    /// <param name="HttpClient">Instancia inyectada del cliente http.</param>
    /// <param name="Configuraciones">Instancia inyectada del acceso a las configuraciones de la aplicación.</param>
    public class ModuloAutenticacionProveedorCSharp(HttpClient HttpClient, IConfiguration Configuraciones) : IModuloAutenticacion
    {
        /// <summary>
        /// Cliente http para consumir el proveedor de identificaciones.
        /// </summary>
        private readonly HttpClient _httpClient = HttpClient;
        /// <summary>
        /// Instancia que permite acceder a las configuraciones del sistema
        /// </summary>
        private readonly IConfiguration _configuration = Configuraciones;

        /// <summary>
        /// Método que ejecuta el proceso de autenticación de la sesión del usuario.
        /// </summary>
        /// <param name="ContextoActual">Contexto actual del sistema de peticiones http.</param>
        /// <param name="UserName">Nombre de usuario del login.</param>
        /// <param name="UserPassword">Contraseña del usuario.</param>
        /// <returns>Proceso asíncrono con el resultado de la autenticación.</returns>
        public async Task<bool> AutenticarUsuario(HttpContext ContextoActual, string UserName, string UserPassword)
        {
            var RespuestaProveedorIdentidades = await ConsumirProveedorIdentidades(UserName, UserPassword);
            if (RespuestaProveedorIdentidades is not null)
            {
                var TokenAcceso = RespuestaProveedorIdentidades?.access_token;
                if (!string.IsNullOrEmpty(TokenAcceso))
                {
                    var ControladorJwt = new JwtSecurityTokenHandler();
                    var Jwt = ControladorJwt.ReadJwtToken(TokenAcceso);
                    var ClaimRol = Jwt.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Role)?.Value;
                    var ClaimEmail = Jwt.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Email)?.Value;
                    var ArregloClaims = new[]
                    {
                        new Claim(ClaimTypes.Name, UserName),
                        new Claim(ClaimTypes.Role, ClaimRol ?? "No identificado"),
                        new Claim(ClaimTypes.Email, ClaimEmail ?? "No identificado"),
                        new Claim("Token", TokenAcceso)
                    };
                    var IdentidadUsuario = new ClaimsIdentity(ArregloClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var ClaimPrincipal = new ClaimsPrincipal(IdentidadUsuario);
                    await ContextoActual.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, ClaimPrincipal);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Realiza el consumo del proveedor de identidades para validar la autenticidad del usuario.
        /// </summary>
        /// <param name="UserName">Nombre de usuario del login.</param>
        /// <param name="UserPassword">Contraseña del usuario.</param>
        /// <returns>Proceso asíncrono con el resultado del proveedor de identidades.</returns>
        private async Task<TokenResponse?> ConsumirProveedorIdentidades(string UserName, string UserPassword)
        {
            var UrlProveedor = _configuration.GetSection("ProveedorIdentidadesInterno:UrlConsumo").Value;
            var IdCliente = _configuration.GetSection("ProveedorIdentidadesInterno:IdCliente").Value;
            var Secreto = _configuration.GetSection("ProveedorIdentidadesInterno:Secreto").Value;

            var SolicitudToken = new HttpRequestMessage(HttpMethod.Post, UrlProveedor)
            {
                Content = new StringContent($"grant_type=password&username={UserName}&password={UserPassword}&client_id={IdCliente}&client_secret={Secreto}", Encoding.UTF8, "application/x-www-form-urlencoded")
            };
            var RespuestaProveedorIdentidades = await _httpClient.SendAsync(SolicitudToken);
            if (RespuestaProveedorIdentidades.IsSuccessStatusCode)
            {
                return await RespuestaProveedorIdentidades!.Content!.ReadFromJsonAsync<TokenResponse>()!;
            }
            return null;
        }
    }
}