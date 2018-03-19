using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConcurrentHttpRequestsGenerator
{
    public class Program
    {
        private static HttpClient _httpClient = new HttpClient();
        private static List<Task> _concurrentRequests = new List<Task>();
        private static string _apiUrl;

        static void Main(string[] args)
        {
            Console.WriteLine("What url would you like to test?");
            _apiUrl = Console.ReadLine();

            Console.WriteLine("How many simultaneous requests shall we make?");
            Console.WriteLine("You will be able to increase/decrese this number by using the Up/Down arrow keys, once the program has started.");
            var requests = int.Parse(Console.ReadLine());

            for (int i = 0; i < requests; i++)
            {
                _concurrentRequests.Add(Task.Factory.StartNew(CreateRequest));
            }

            while (true)
            {
                var key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        _concurrentRequests.Add(Task.Factory.StartNew(CreateRequest));
                        LogRequests();
                        break;
                    case ConsoleKey.DownArrow:
                        if (_concurrentRequests.Any())
                        {
                            _concurrentRequests.RemoveAt(0);
                        }

                        LogRequests();
                        break;
                }
            }
        }

        private static Task CreateRequest()
        {
            return _httpClient.GetAsync(_apiUrl).ContinueWith(task => CreateRequest());
        }

        private static void LogRequests()
        {
            Console.WriteLine($"Number of parallel request: {_concurrentRequests.Count}");
        }
    }
}
