namespace AspNetCoreMvc.Redis.Components
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Newtonsoft.Json;
    using StackExchange.Redis;
    using System.Linq;
    using System.Threading.Tasks;

    public class UsersByDateCreatedViewComponent : ViewComponent
    {
        public IDatabase _db { get; set; }

        public UsersByDateCreatedViewComponent(IDatabase db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var users = _db.SortedSetRangeByScore("users", take: 5, order: Order.Descending)
                .SelectUsersByCreationDateStrongTypeForUser((s) => DeserializeValue(_db.StringGet(s.ToString())))
                .ToList();
            return View(users);
        }

        private User DeserializeValue(string value)
        {
            return JsonConvert.DeserializeObject<User>(value);
        }
    }
}
