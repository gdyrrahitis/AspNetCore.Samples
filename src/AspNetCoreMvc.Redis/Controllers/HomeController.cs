namespace AspNetCoreMvc.Redis.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController: Controller
    {
         // GET /home/
        public IActionResult Index()
        {
            ViewBag.Message = "Welcome!";
            return View();
        }
    }
}