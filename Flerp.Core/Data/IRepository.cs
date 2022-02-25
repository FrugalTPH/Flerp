using Flerp.DomainModel;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Flerp.Data
{
    public interface IRepository<T> where T : class, IPersistable
    {
        void GetCollection(string name);

        void Save(T value);

        void Delete(T value);

        void Drop();

        IList<T> GetAll();

        T GetById(string id);

        IList<T> GetByQuery(IMongoQuery query);
    }
}