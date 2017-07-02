using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using Microsoft.ServiceBus.Messaging;
using System.Configuration;


namespace SampleMongoDB
{
    
    class Program
    {
        /// <summary>
        /// repo - for use instance
        /// </summary>
        private static ShareClassRepository repo = new ShareClassRepository();
        private static List<int> items = Enumerable.Range(1, 1000000).ToList();
        static string eventHubName = ConfigurationManager.AppSettings["EventHubName"];
        static string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            //SendingRandomMessages();---- Azure Sample...
            //Console.ReadLine();
            TaskFactory fact = new TaskFactory();
            List<Task> lsttask = new List<Task>();
            #region "save records - Result Analysis - saving 1 million records - 30 sec. in parallel and Sequntial - 1.30 mins"
            int batchSize = 50000;
            var noOftaks = 1000000 / batchSize;
            //GenerateShare(repo, 0);// sequntial operation..
            for (int i = 1; i < noOftaks; i++)
            {
                var dataItemsRem = items.Skip(0 * batchSize).Take(batchSize).ToList();
                Console.WriteLine("task :" + i + " started...");
                var task = fact.StartNew(() => GenerateShare(i, dataItemsRem));
                lsttask.Add(task);
            }
            #endregion
            #region delete records -
            //var noOftaks = 1800000 / 100000;
            //for (int i = 0; i < noOftaks; i++)
            //{
            //    Console.WriteLine("task :" + i + " started...");
            //    var itemsOfData = items.Skip(i * 100000).Take(100000).ToList();
            //    var task = fact.StartNew(() => DeleteShare(itemsOfData));
            //    lsttask.Add(task);
            //}
            #endregion
            Task.WaitAll(lsttask.ToArray());
            watch.Stop();
            Console.WriteLine("process completed: " + watch.Elapsed.TotalSeconds);
            Console.ReadLine();
        }

        static void SendingRandomMessages()
        {
            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);
            while (true)
            {
                try
                {
                    var message = Guid.NewGuid().ToString();
                    Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, message);
                    eventHubClient.Send(new EventData(Encoding.UTF8.GetBytes(message)));
                }
                catch (Exception exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0} > Exception: {1}", DateTime.Now, exception.Message);
                    Console.ResetColor();
                }

                Thread.Sleep(200);
            }
        }

        /// <summary>
        /// GenerateShare = klklk
        /// </summary>
        private static void GenerateShare(int taskid, List<int> items)
        {

            List<ShareClass_Structure> lstStru = new List<ShareClass_Structure>();
            for (int i = items[0]; i <= items[items.Count - 1]; i++)
            {
                ShareClass_Structure structure = new ShareClass_Structure
                {
                    Country = "IN",
                    InstId = i,
                    Language = "HN",
                    Data = "lklklklklklklklklklklklklklkl",
                    PerDt = DateTime.Now
                };
                lstStru.Add(structure);

            }
            repo = new ShareClassRepository();
            repo.SaveCollection(lstStru.FirstOrDefault());
            Console.WriteLine("task :" + taskid + " completed"); ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="taskId"></param>
        private static void DeleteShare(List<int> data)
        {
            for (int i = data[0]; i < data[data.Count - 1]; i++)
            {
                var filter = Builders<ShareClass_Structure>.Filter.Eq("InstId", data[i]);
                repo.DeleteCollection(filter);
            }

        }

        private static IEnumerable<ShareClass_Structure> GetShares(int instid)
        {
            var filter = Builders<ShareClass_Structure>.Filter.Eq("InstId", instid);
            return repo.GetShares(filter);
        }
    }
}
