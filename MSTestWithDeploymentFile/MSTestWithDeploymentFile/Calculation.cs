using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace MSTestWithDeploymentFile
{
    public class Calculation
    {
        int a;
        int b;

        /// <summary>
        /// Calculation - required to set properties
        /// </summary>
        public Calculation()
        {
            LoadJSON();
        }
        /// <summary>
        /// load json for data load
        /// </summary>
        private void LoadJSON()
        {
            var fileName = ConfigurationManager.AppSettings["fileName"];
            var jsonFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName;
            using (StreamReader file = File.OpenText(jsonFilePath))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject o2 = (JObject)JToken.ReadFrom(reader);
                a = (int)o2["a"];
                b = (int)o2["b"];
            }
        }

        /// <summary>
        /// do the sum
        /// </summary>
        /// <returns></returns>
        public int Sum()
        {
            return a + b;
        }
    }
}
