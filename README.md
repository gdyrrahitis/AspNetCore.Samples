# AspNetCore.Samples
Sample apps with ASP.NET Core.

Solution contains sample applications build with ASP.NET Core.

## AspNetCoreMvc.Redis
An ASP.NET Core application using Redis cache to store users. `StackExchange.Redis.StrongName` is used.

Users are persisted as single entities, tracked by their key. Additionally, two sorted sets contain information regarding the date the user was created and points, respectively. These sorted sets will be used to create tow separate leaderboards.

Users persisted as JSON entities, identified by their Id, using a Redis `SET` command.

**Example**
```
_db.StringSet(userId, JsonConvert.SerializeObject(user));
// In redis
SET "00000000-0000-0000-0000-000000000000" "{\"Id\": \"00000000-0000-0000-0000-000000000000\",\"FirstName\":null,\"LastName\":null,\"CreatedOn\":\"0001-01-01T00:00:00\"}"

```

When new user is added, a new entry at a sorted set is created. There are two sorted sets used in the application, `users` and `highscores`.

In `users` sorted set, Id's of users are persisted, with score being the date they've been created in ticks.

For `scores` sorted set, Id's of users are also used to identify an entry, with the score being score points added by the application.

**Examples**
```
_db.SortedSetAdd("users", userId, user.CreatedOn.Ticks);
// In redis
ZADD users 123456789 "00000000-0000-0000-0000-000000000000"


_db.SortedSetAdd("highscores", user.Element, newScore);
// In redis
ZADD highscores 5 "00000000-0000-0000-0000-000000000000"
```

Leaderboards are shown in homepage `http://localhost:<port>/` using template components

Data is fetched by `ZRANGE` command of Redis, taking maximum up to five top entries in descenting order. Based on the need, data are fetched with or without associated score.

**Examples**
```
// Getting top 5 entries from `users` sorted set, in descenting order
_db.SortedSetRangeByScore("users", take: 5, order: Order.Descending);

// Getting top 5 entries from `highscores` sorted set, in descenting order
_db.SortedSetRangeByScoreWithScores("highscores", take: 5, order: Order.Descending)
```

**Component invocation**

```
<div class="row">
    <div class="col-md-6">
        @await Component.InvokeAsync("UsersByDateCreated")
    </div>
    <div class="col-md-6">
        @await Component.InvokeAsync("UsersByScore")
    </div>
</div>
```

# Download
Clone repository `https://github.com/gdyrrahitis/AspNetCore.Samples.git` on your desktop.

Make sure you have downloaded Redis for windows. If not, go [here](https://github.com/MSOpenTech/redis) and download redis. To run a redis server, go to `%programfiles%/Redis` and run the `redis-cli.exe`.

Then run the application.
