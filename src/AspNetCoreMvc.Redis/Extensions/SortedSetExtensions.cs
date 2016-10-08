namespace AspNetCoreMvc.Redis
{
    using Models;
    using StackExchange.Redis;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels;

    public static class SortedSetExtensions
    {
        public static IEnumerable<HighscoreViewModel> SelectUsersByScoreStrongTypeForUser(this SortedSetEntry[] set, Func<SortedSetEntry, User> predicate)
        {
            return set.Select(s =>
            {
                if (s.Element.IsNullOrEmpty) return null;

                var user = predicate(s);
                return new HighscoreViewModel
                {
                    Id = user.Id.ToString(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Score = Convert.ToInt32(s.Score)
                };
            });
        }

        public static IEnumerable<UserCreatedViewModel> SelectUsersByCreationDateStrongTypeForUser(this RedisValue[] set, Func<RedisValue, User> predicate)
        {
            return set.Select(s =>
            {
                if (s.IsNullOrEmpty) return null;
                var user = predicate(s);
                return new UserCreatedViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CreatedOn = user.CreatedOn
                };
            });
        }
    }
}
