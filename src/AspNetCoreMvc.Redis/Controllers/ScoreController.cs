namespace AspNetCoreMvc.Redis.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Newtonsoft.Json;
    using StackExchange.Redis;
    using System;
    using ViewModels;

    public class ScoreController : Controller
    {
        private bool disposed = false;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public ScoreController()
        {
            _redis = ConnectionMultiplexer.Connect("localhost");
            _db = _redis.GetDatabase(0);
        }

        // GET: /Score/
        public IActionResult Index()
        {
            var users = _db.SortedSetRangeByRankWithScores("users")
                .Select(s =>
                {
                    var user = JsonConvert.DeserializeObject<User>(s.Element.ToString());
                    return new HighscoreViewModel
                    {
                        Id = user.Id.ToString(),
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    };
                });
            return View(users);
        }

        // POST /Score/<id>
        [HttpPost]
        public IActionResult Index(Guid id)
        {
            var user = _db.SortedSetScan("highscores", $"*{id}*").FirstOrDefault();
            if (user.Element.IsNullOrEmpty) return BadRequest();

            _db.SortedSetAdd("highscores", user.Element, user.Score + 1);
            return Json(JsonConvert.DeserializeObject<User>(user.Element.ToString()));
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
            if (disposing)
            {
                _redis.Dispose();
            }

            disposed = true;
            base.Dispose(disposing);
        }
    }
}
