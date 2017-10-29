using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinySharpBlockChain;

namespace TinySharpBlockChain
{
    class Program
    {
        public static string NodeIdentifier;
        static void Main(string[] args)
        {
            NodeIdentifier = Guid.NewGuid().ToString().Replace("-", "").ToLower();
            SetupWebApiServer();
        }


        private static void SetupWebApiServer()
        {
            var url = "http://localhost:5000";
            using (var webApiServer = WebApp.Start<WebApiStartup>(url))
            {
                Console.WriteLine($"Node {NodeIdentifier} start at {url}");
                Console.WriteLine("Press Enter to quit!");
                Console.ReadLine();
            }
        }
    }
}
