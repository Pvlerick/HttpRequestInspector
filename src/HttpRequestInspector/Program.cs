using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;

namespace HttpRequestInspector
{
    class Program
    {
        static Task<int> Main(string[] args)
        {
            var cmd = new RootCommand("HTTP Request Inspector")
            {
                new Option(new[] { "-p", "--port" })
                {
                    Description = "The port to listen on. Default is 5000.",
                    Argument = new Argument<int>(defaultValue: () => 5000)
                },
                new Option(new[] { "-u", "--unsecure" })
                {
                    Description = "Accept HTTP requests instead of HTTPS.",
                    Argument = new Argument<bool>(defaultValue: () => false)
                },
                new Option(new[] { "-s", "--response-status-code" })
                {
                    Description = "The HTTP status code to be returned when called. Default is 200.",
                    Argument = new Argument<int>(defaultValue: () => 200)
                }
            };

            cmd.Handler = CommandHandler.Create<int, bool, int>(Handler);

            return cmd.InvokeAsync(args);
        }

        private static void Handler(int port, bool unsecure, int responseStatusCode)
        {
            if (unsecure)
                StartListeningForHttpRequests(port, responseStatusCode);
            else
                StartListenginForHttpsRequests(port, responseStatusCode);
        }

        private static void StartListenginForHttpsRequests(int port, int responseStatusCode)
        {
            CreateWebHostBuilder(responseStatusCode)
                .UseUrls($"https://0.0.0.0:{port}")
                .Build()
                .Run();
        }

        private static void StartListeningForHttpRequests(int port, int responseStatusCode)
        {
            CreateWebHostBuilder(responseStatusCode)
                .UseUrls($"http://0.0.0.0:{port}")
                .Build()
                .Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(int responseStatusCode) =>
            WebHost
                .CreateDefaultBuilder<Startup>(Enumerable.Empty<string>().ToArray())
                .ConfigureAppConfiguration((_, b) => b.AddInMemoryCollection(
                    new Dictionary<string, string>
                    {
                        ["Logging:LogLevel:Default"] = "Warning",
                        [Startup.ResponseStatusCodeConfigurationItem] = responseStatusCode.ToString()
                    }));
    }
}
