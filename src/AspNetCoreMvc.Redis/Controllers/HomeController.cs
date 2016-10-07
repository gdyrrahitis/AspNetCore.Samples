namespace AspNetCoreMvc.Redis.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using StackExchange.Redis;

    public class HomeController: Controller
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
            IEnumerable<RedisValue> users = _db.SortedSetRangeByScore("users", take: 5).AsEnumerable();
            return View(users);
        }
    }
}