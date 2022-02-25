using Flerp.Data.LocalDb.Properties;
using Flerp.DomainModel;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;

namespace Flerp.Data.LocalDb
{
    public class Repository<T> : IRepository<T> where T : class, IPersistable
    {
        MongoCollection<T> Collection { get; set; }

        public void GetCollection(string name)
        {
            Collection = DatabaseProvider.Current.GetDatabase().GetCollection<T>(name);
        }

        public void Save(T value)
        {
            Collection.Save(value);
        }

        public void Delete(T value)
        {
            Collection.Remove(Query.EQ(Resources.DbField_Id, value.Id));
        }

        public void Drop()
        {
            Collection.Drop();
        }

        public IList<T> GetAll()
        {
            var q = Query.Or(Query.NotExists(Resources.DbField_DisposedDate), Query.GT(Resources.DbField_DisposedDate, DateTime.UtcNow));

            var all = Collection.Find(q);

            return new List<T>(all);
        }

        public T GetById(string id)
        {
            return Collection.FindOneAs<T>(Query.EQ(Resources.DbField_Id, id));
        }

        public IList<T> GetByQuery(IMongoQuery query)
        {
            var result = Collection.Find(query);

            return new List<T>(result);
        }

        public override string ToString() { return Collection.Name; }
    }
}