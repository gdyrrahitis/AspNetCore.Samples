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
        private ConnectionMultiplexer _redis;
        private bool disposed = false;

        public UserController()
        {
            _redis = ConnectionMultiplexer.Connect("localhost");
            _db = _redis.GetDatabase(0);
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

        public new void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                _redis.Dispose();
            }

            disposed = true;
            base.Dispose(disposing);
        }
    }
}