using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CSVConversation
{
    class Program
    {
        static void Main(string[] args)
        {
            List<TestCSV> data = new List<TestCSV>();
            for (int i = 0; i < 1000000; i++)
            {
                TestCSV obj = new TestCSV() { id = i, Name = "Hiren" + i, PerDt = DateTime.Now, val1 = i, val2 = i + 1, val3 = i + 2 };
                data.Add(obj);
            }
            data.ToCSV<TestCSV>(@"D:\TestCSV.csv");
        }
    }
    class TestCSV
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int val1 { get; set; }
        public int val2 { get; set; }
        public int val3 { get; set; }

        public DateTime PerDt { get; set; }


    }
}
