using ElectricBox.Models.Cable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectricBox.Controllers
{
    public class CableController : Controller
    {
        private readonly Cable? _cable;

        public CableController()
        {
            _cable = new Cable();
        }

        public IActionResult Cable()
        {
            return View("/Views/Cable/Cable.cshtml", _cable);
        }
    }
}
