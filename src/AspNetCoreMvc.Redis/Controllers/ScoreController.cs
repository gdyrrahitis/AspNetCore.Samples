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
        private readonly IDatabase _db;

        public ScoreController(IDatabase database)
        {
            _db = database;
        }

        // GET: /Score/
        public IActionResult Index()
        {
            var users = _db.SortedSetRangeByScoreWithScores("highscores", order: Order.Descending)
                .SelectUsersByScoreStrongTypeForUser((s) => JsonConvert.DeserializeObject<User>(_db.StringGet(s.Element.ToString())));
            return View(users);
        }

        // POST /Score/<id>
        [HttpPost]
        public IActionResult Index(Guid id)
        {
            var user = _db.SortedSetScan("highscores", $"*{id}*").FirstOrDefault();
            if (user.Element.IsNullOrEmpty) return BadRequest();

            var newScore = user.Score + 1;
            _db.SortedSetAdd("highscores", user.Element, newScore);

            var entity = JsonConvert.DeserializeObject<HighscoreViewModel>(_db.StringGet(user.Element.ToString()));
            entity.Score = Convert.ToInt32(newScore);
            return Json(entity);
        }
    }
}
