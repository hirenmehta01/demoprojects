using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMongoDB.App_Data
{
    /// <summary>
    /// DBContext - which is resonsible for making connection with mongoDB
    /// </summary>
    public class DBContext
    {
        protected IMongoDatabase context { get; set; }
        /// <summary>
        /// define the mobgodb client
        /// </summary>
        public DBContext()
        {
            MongoClient client = new MongoClient("mongodb://testsample:O8rYqZQI0wUYSkUfUf6bys2tHsS73jM1pjcJq4DCva14ReNWVoCvMFn8Y1fw5WNSphQBXWanhGj1K1H7v79LOg==@testsample.documents.azure.com:10250/?ssl=true");
            this.context = client.GetDatabase("SampleDB");
        }
    }
}
