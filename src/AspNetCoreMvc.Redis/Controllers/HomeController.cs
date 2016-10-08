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
                .Select(s => {
                    if (s.IsNullOrEmpty) return null;
                    var user = JsonConvert.DeserializeObject<User>(_db.StringGet(s.ToString()));
                    return new UserCreatedViewModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        CreatedOn = user.CreatedOn
                    };
                });

            var scores = _db.SortedSetRangeByScoreWithScores("highscores", take: 5, order: Order.Descending)
                        .Select(s =>
                        {
                            if (s.Element.IsNullOrEmpty) return null;
                            var user = JsonConvert.DeserializeObject<User>(_db.StringGet(s.Element.ToString()));
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
    }
}