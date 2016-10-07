namespace AspNetCoreMvc.Redis.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    public class UserController: Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(User user)
        {
            user.Id = Guid.NewGuid();
            user.CreatedOn = DateTime.UtcNow;

            // Persist to REDIS cache

            return RedirectToAction("Index", "Home");
        }
    }
}