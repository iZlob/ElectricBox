using ElectricBox.Models.Kontur;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;

namespace ElectricBox.Controllers
{
    public class GroundResistanceController : Controller
    {
        private readonly GroundResistance? _groundResistance;

        public GroundResistanceController()
        {
            _groundResistance = new GroundResistance();
        }

        public IActionResult Kontur()
        {
            return View("/Views/GroundResistance/Kontur.cshtml", _groundResistance);
        }

    }
}
