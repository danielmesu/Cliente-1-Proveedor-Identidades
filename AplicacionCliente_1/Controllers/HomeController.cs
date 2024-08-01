using AplicacionCliente_1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AplicacionCliente_1.Controllers
{
    /// <summary>
    /// Clase que requiere de autenticación y autorización del usuario por medio de roles.
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        /// Servicio que genera la vista Home.<br/>
        /// Este servicio requiere de autenticación mas no de autorización.
        /// </summary>
        /// <returns>Vista correspodiente</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Servicio que genera la vista de privacidad.<br/>
        /// Este servicio requiere de autenticación y autorización por rol "administrador".
        /// </summary>
        /// <returns>Vista correspondiente.</returns>
        [Authorize(Roles = "administrador")]
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// En caso de presentarse error este servicio genera la vista correspondiente para el mismo.
        /// </summary>
        /// <returns>Vista de error correspondiente.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}