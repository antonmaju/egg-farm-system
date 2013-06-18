using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
using ServiceStack.OrmLite;

namespace EggFarmSystem.Services
{
    public interface IAccountService : IDataService<Account>
    {
        bool Validate(string userName, string password);

        Account Get(string userName, string password);
    }

    public class AccountService : DataService<Account>, IAccountService
    {
        private readonly IDbConnectionFactory factory;

        public AccountService(IDbConnectionFactory factory) : base(factory)
        {
            this.factory = factory;
        }

        public bool Validate(string userName, string password)
        {
            using (var conn = factory.CreateDbConnection())
            {
                conn.Open();
                var result = conn.Where<Account>(new {Name = userName, Password = password});
                return result.Any();
            }
        }


        public Account Get(string userName, string password)
        {
            using (var conn = factory.CreateDbConnection())
            {
                conn.Open();
                var result = conn.Where<Account>(new { Name = userName, Password = password });
                return result.FirstOrDefault();
            }
        }
    }
}
