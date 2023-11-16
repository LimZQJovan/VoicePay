using Microsoft.AspNetCore.Mvc;

namespace VoicePay.Controllers
{
    public class VoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
