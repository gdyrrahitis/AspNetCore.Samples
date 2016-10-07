namespace AspNetCoreMvc.Redis.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Newtonsoft.Json;
    using StackExchange.Redis;

    public class UserController: Controller
    {
        private readonly IDatabase _db;

        public UserController()
        {
            var redis = ConnectionMultiplexer.Connect("localhost");
            _db = redis.GetDatabase(0);
        }

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
            _db.SortedSetAdd("users", JsonConvert.SerializeObject(user), user.CreatedOn.Ticks);

            return RedirectToAction("Index", "Home");
        }
    }
}