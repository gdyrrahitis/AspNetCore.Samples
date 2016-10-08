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

        public UserController(IDatabase database)
        {
            _db = database;
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
            var userId = user.Id.ToString();

            _db.StringSet(userId, JsonConvert.SerializeObject(user));
            _db.SortedSetAdd("users", userId, user.CreatedOn.Ticks);
            _db.SortedSetAdd("highscores", userId, 0);

            return RedirectToAction("Index", "Home");
        }
    }
}