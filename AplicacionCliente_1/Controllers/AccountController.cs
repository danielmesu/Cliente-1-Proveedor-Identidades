using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using AplicacionCliente_1.Models;
using AplicacionCliente_1.ModuloAutenticacion;

namespace AplicacionCliente_1.Controllers
{
    /// <summary>
    /// Controlador de la cuenta del usuario, contiene métodos autenticación y control de acceso.
    /// </summary>
    /// <param name="ModuloAutenticacion">Instancia inyectada del módulo de autenticación.</param>
    public class AccountController(IModuloAutenticacion ModuloAutenticacion) : Controller
    {
        /// <summary>
        /// Cliente http para consumir el proveedor de identificaciones.
        /// </summary>
        private readonly IModuloAutenticacion _ModuloAutenticacion = ModuloAutenticacion;

        /// <summary>
        /// Servicio que provee de la vista de login a usuarios.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Servicio que genera la autenticación del usuario con el sistema.<br/>
        /// Este método consume el proveedor de identidades para autenticar al usuario.
        /// </summary>
        /// <param name="model">Modelo con la inormación de las credenciales de usuario.</param>
        /// <returns>Vista correspondiente según el proceso de autenticación del usuario.</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var AutenticacionExitosa = await _ModuloAutenticacion.AutenticarUsuario(HttpContext, model.Username, model.Password);
                    if (AutenticacionExitosa)
                        return RedirectToAction("Index", "Home");
                    else
                        ModelState.AddModelError("", "Credenciales no válidas");
                }
            }
            catch
            {
                ModelState.AddModelError("", "El proveedor de identidades no se encuentra disponible.");
            }
            return View(model);
        }

        /// <summary>
        /// Servicio que provee de la vista de acceso denegado a un usuario, esto en caso de que no se encuentre autorizado para visualizar la interfaz solicitada.
        /// </summary>
        /// <returns>Vista del tipo acceso no autorizado.</returns>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// Servicio de cierre de sesión, este finaliza la sesión del usuario en el contexto actual.
        /// </summary>
        /// <returns>Vista para el login del usuario.</returns>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}