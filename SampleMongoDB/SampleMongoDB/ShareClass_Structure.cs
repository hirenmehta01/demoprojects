using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMongoDB
{
    public class ShareClass_Structure
    {
        public ObjectId _id { get; set; }
        public int InstId { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }

        public string Data { get; set; }

        public DateTime PerDt { get; set; }
    }
}
