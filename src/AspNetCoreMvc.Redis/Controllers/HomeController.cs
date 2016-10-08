namespace AspNetCoreMvc.Redis.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Newtonsoft.Json;
    using StackExchange.Redis;

    public class HomeController : Controller
    {
        private readonly IDatabase _db;

        public HomeController(IDatabase database)
        {
            _db = database;
        }

        // GET /home/
        public IActionResult Index()
        {
            var users = _db.SortedSetRangeByScore("users", take: 5, order: Order.Descending)
                .SelectUsersByCreationDateStrongTypeForUser((s) => DeserializeValue(_db.StringGet(s.ToString())));

            var scores = _db.SortedSetRangeByScoreWithScores("highscores", take: 5, order: Order.Descending)
                .SelectUsersByScoreStrongTypeForUser((s) => DeserializeValue(_db.StringGet(s.Element.ToString())));

            ViewBag.Users = users;
            ViewBag.Scores = scores;
            return View();
        }

        private User DeserializeValue(string value)
        {
            return JsonConvert.DeserializeObject<User>(value);
        }
    }
}