using EggFarmSystem.Models;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Services
{
    public interface IDataService<T> where T:Entity
    {
        IList<T> GetAll();

        T Get(Guid id);

        void Save(T model);

        void Delete(Guid id);
    }

    public class DataService<T> : IDataService<T> where T:Entity, new()
    {
        IDbConnectionFactory factory;

        public DataService(IDbConnectionFactory factory)
        {
            this.factory = factory;
        }


        public virtual IList<T> GetAll()
        {
            using (var db = factory.CreateDbConnection())
            {
                db.Open();
                return db.Select<T>();
            }
        }

        public T Get(Guid id)
        {
            using (var db = factory.CreateDbConnection())
            {
                db.Open();
                return db.GetById<T>(id.ToString());
            }
        }

        public void Save(T model)
        {
            using (var db = factory.CreateDbConnection())
            {
                db.Open();
                if (model.Id == Guid.Empty)
                {
                    model.Id = Guid.NewGuid();
                    db.InsertParam(model);
                }
                else
                    db.UpdateParam(model);
            }
        }

        public void Delete(Guid id)
        {
            using (var db = factory.CreateDbConnection())
            {
                db.Open();
                db.DeleteByIdParam<T>(id.ToString());
            }
        }
    }
}
