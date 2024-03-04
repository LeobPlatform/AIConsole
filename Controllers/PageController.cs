using Microsoft.AspNetCore.Mvc;

namespace AIConsole.Controllers
{

    [Route("[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PageController : Controller
    {
        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Default()
        {
            return Redirect("/welcome");
        }

        [HttpGet]
        [Route("/welcome")]
        public IActionResult Welcome()
        {
            return View("~/Pages/Welcome.cshtml");
        }

        [HttpGet]
        [Route("/favicon.ico")]
        public void favicon()
        {
            
        }

        [HttpGet, HttpPost]
        [Route("/{page}")]
        public async Task<IActionResult> PageView([FromRoute] string page)
        {
            return View($"~/Pages/{page}.cshtml");
        }
    }
}