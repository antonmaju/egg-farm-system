using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
using ServiceStack.OrmLite;

namespace EggFarmSystem.Core.Tests
{
    public static class DatabaseTestInitializer
    {
        public static IDbConnectionFactory GetConnectionFactory()
        {
            return new DummyDbConnectionFactory();
        }
    }

    public class DummyDbConnectionFactory : IDbConnectionFactory
    {
        private static readonly IDbConnectionFactory factory;

        static DummyDbConnectionFactory()
        {
            //factory = new OrmLiteConnectionFactory("Data Source=:memory:;Version=3;", false, SqliteDialect.Provider);
            factory = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["EggFarmDb"].ConnectionString, MySqlDialect.Provider);
        }

        public IDbConnection CreateDbConnection()
        {
            return factory.CreateDbConnection();
        }

        public IDbConnection OpenDbConnection()
        {
            var conn = factory.CreateDbConnection();
            //if(conn.State != ConnectionState.Open)
                conn.Open();
            conn.CreateTableIfNotExists<HenHouse>();
            conn.CreateTableIfNotExists<Hen>();
            conn.CreateTableIfNotExists<Employee>();
            conn.CreateTableIfNotExists<Consumable>();
            conn.CreateTableIfNotExists<ConsumableUsage>();
            
            return conn;
        }
    }
 
}
