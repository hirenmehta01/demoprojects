using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMongoDB
{
    /// <summary>
    /// ShareClassRepository = share class repository
    /// </summary>
    public class ShareClassRepository : IRepository<ShareClass_Structure>
    {
        /// <summary>
        /// return share class structures.
        /// </summary>
        public IMongoCollection<ShareClass_Structure> ShareStructures { get { return TCollection; } }

        private Func<int, IMongoCollection<ShareClass_Structure>, IEnumerable<ShareClass_Structure>> GetStructures = (id, collection) => collection.AsQueryable().Where(j => j.InstId.Equals(id));

        /// <summary>
        /// SaveCollection - save collection.
        /// </summary>
        /// <param name="obj"></param>
        public override void SaveCollection(ShareClass_Structure obj)
        {
            TCollection.InsertOne(obj);
        }

        public override void SaveCollections(ShareClass_Structure[] obj)
        {
            TCollection.InsertMany(obj);
        }

        public override void DeleteCollection(FilterDefinition<ShareClass_Structure> filter)
        {
            TCollection.DeleteMany(filter);
        }

        /// <summary>
        /// get share documents.
        /// </summary>
        /// <param name="instId"></param>
        /// <returns></returns>
        public IEnumerable<ShareClass_Structure>  GetShares(FilterDefinition<ShareClass_Structure> filter)
        {
            return TCollection.Find(filter).ToEnumerable<ShareClass_Structure>();
        }
        
    }
}
