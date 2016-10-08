namespace AspNetCoreMvc.Redis.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Newtonsoft.Json;
    using StackExchange.Redis;
    using ViewModels;
    using System;

    public class HomeController : Controller
    {
        private bool disposed = false;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public HomeController()
        {
            var redis = ConnectionMultiplexer.Connect("localhost");
            _db = redis.GetDatabase(0);
        }

        // GET /home/
        public IActionResult Index()
        {
            var users = _db.SortedSetRangeByScore("users", take: 5, order: Order.Descending)
                .Select(s => JsonConvert.DeserializeObject<User>(s.ToString()))
                .Select(s => new UserCreatedViewModel
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    CreatedOn = s.CreatedOn
                });

            var scores = _db.SortedSetRangeByScoreWithScores("highscores", take: 5, order: Order.Descending)
                        .Select(s =>
                        {
                            var user = JsonConvert.DeserializeObject<User>(s.ToString());
                            return new HighscoreViewModel
                            {
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Score = Convert.ToInt32(s.Score)
                            };
                        });

            ViewBag.Users = users;
            ViewBag.Scores = scores;
            return View();
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