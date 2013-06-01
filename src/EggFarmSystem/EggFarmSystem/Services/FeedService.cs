using EggFarmSystem.Models;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Services
{
    public interface IFeedService
    {
        SearchResult<Feed> Search();

    }


    public class FeedService : DataService<Feed>, IFeedService
    {
        IDbConnectionFactory factory;

        public FeedService(IDbConnectionFactory factory)
            : base(factory)
        {
            this.factory = factory;
        }

        public SearchResult<Feed> Search()
        {
            var result = new SearchResult<Feed>();

            using (var db = factory.CreateDbConnection())
            {
                db.Open();
                result.Items = db.Select<Feed>().ToList();
                result.Total = (int)db.Count<Feed>();
            }

            return result;
        }
    }
}
