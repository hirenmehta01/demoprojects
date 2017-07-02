using MongoDB.Driver;
using SampleMongoDB.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMongoDB
{
    public abstract class IRepository<T> : DBContext where T : class,new()
    {
        private string TName { get { return typeof(T).Name; } }

        private IMongoCollection<T> _TCollection;
        protected IMongoCollection<T> TCollection { get { if (_TCollection == null) { _TCollection = context.GetCollection<T>(TName); } return _TCollection; } }

        /// <summary>
        /// SaveCollection == for save collection
        /// </summary>
        /// <param name="obj"></param>
        public abstract void SaveCollection(T obj);

        /// <summary>
        /// SaveCollection == for save collection
        /// </summary>
        /// <param name="obj"></param>
        public abstract void SaveCollections(T[] obj);

        public abstract void DeleteCollection(FilterDefinition<ShareClass_Structure> obj);
        
    }
}
