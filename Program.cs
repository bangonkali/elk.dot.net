using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json.Serialization;

namespace client.elk
{
    class Program
    {
        static void Main(string[] args)
        {
            ElkRandomizer _elk = new ElkRandomizer();

            _elk.Do();
        }
    }

    public class ElkRandomizer
    {
        public ElkRandomizer()
        {
        }

        private Uri _nodeUri = new Uri("http://phtemhm-autovm1:9200");
        private ConnectionSettings _settings;
        private ElasticClient _elasticClient;
        private Random _rand = new Random(DateTime.Now.Millisecond);

        private string[] _words =
        {
            "Uploading",
            "Creating",
            "Finished",
            "Warning",
            "Error",
            "Done",
            "Comparing",
            "Equal",
            "Not Equal",
            "Deploying",
            "Searching",
            "Control"
        };

        private void Connect()
        {
            _settings = new ConnectionSettings(
                _nodeUri,
                defaultIndex: "my-application"
            );

            _elasticClient = new ElasticClient(_settings);

            Console.WriteLine("Connection created.");
        }

        private void GenerateRandomData()
        {
            for (int i = 0; i < long.MaxValue - 1; i++)
            {
                var log = new Log
                {
                    Id = i,
                    Message = _words[_rand.Next(0, _words.Length - 1)],
                    DateCreated = RandomDay(),
                    DateModified = RandomDay(),
                    LogInfo = _rand.Next(0, 9)
                };

                Console.WriteLine("Log created: [" + i + "] " + log.Message);
                var index = _elasticClient.Index(log);
                Console.WriteLine("Log Sent: [" + i + "] " + log.Message);
            }
        }

        DateTime RandomDay()
        {
            DateTime start = new DateTime(1995, 1, 1);
            Random gen = new Random();

            int range = (DateTime.Today - start).Days;
            start = start.AddDays(gen.Next(range));
            return start.AddMilliseconds(gen.Next(0, int.MaxValue - 1));
        }

        public void Do()
        {
            Connect();
            GenerateRandomData();
        }
    }

    public class Log
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int LogInfo { get; set; }
    }
}
