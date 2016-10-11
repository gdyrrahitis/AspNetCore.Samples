namespace AspNetCoreMvc.Redis.Components
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Newtonsoft.Json;
    using StackExchange.Redis;
    using System.Linq;
    using System.Threading.Tasks;

    public class UsersByScoreViewComponent : ViewComponent
    {
        private readonly IDatabase _db;

        public UsersByScoreViewComponent(IDatabase db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var scores = _db.SortedSetRangeByScoreWithScores("highscores", take: 5, order: Order.Descending)
                .SelectUsersByScoreStrongTypeForUser((s) => DeserializeValue(_db.StringGet(s.Element.ToString())))
                .ToList();
            return View(scores);
        }

        private User DeserializeValue(string value)
        {
            return JsonConvert.DeserializeObject<User>(value);
        }
    }
}
