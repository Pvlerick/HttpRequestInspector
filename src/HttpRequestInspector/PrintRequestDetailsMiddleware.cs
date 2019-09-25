using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpRequestInspector
{
    internal class PrintRequestDetailsMiddleware
    {
        private readonly RequestDelegate next;

        public PrintRequestDetailsMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await this.PrintRequestDetailsToConsole(context.Request);

            await next(context);
        }

        private async Task PrintRequestDetailsToConsole(HttpRequest request)
        {
            Console.WriteLine(new string('=', 40));
            Console.WriteLine($"Timestamp: {DateTime.Now}");
            Console.WriteLine($"Target: {request.Scheme}://{request.Host}{request.Path}{request.QueryString}");
            Console.WriteLine($"Protocol: {request.Protocol}");
            Console.WriteLine($"Method: {request.Method}");
            Console.WriteLine($"Content-Type: {request.ContentType}");

            Console.WriteLine("Headers:");
            foreach (var header in request.Headers.Select(i => string.Format("  {0}={1}", i.Key, i.Value)))
                Console.WriteLine(header);

            Console.WriteLine("Body:");
            Console.WriteLine(await ReadBody(request.Body, request.ContentType));
        }

        private async Task<string> ReadBody(Stream body, string contentType)
        {
            var encoding = contentType != null ? MediaType.GetEncoding(contentType) ?? Encoding.UTF8 : Encoding.UTF8;

            using (var bodyReader = new StreamReader(body, encoding))
            {
                return await bodyReader.ReadToEndAsync().ConfigureAwait(false);
            }
        }
    }
}