using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;

namespace HttpRequestInspector
{
    internal class Startup
    {
        internal const string ResponseStatusCodeConfigurationItem = "ResponseStatusCode";

        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

#if NETCOREAPP3_0
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#else
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
#endif
        {
            app.UseMiddleware<PrintRequestDetailsMiddleware>();

            app.Run(async (context) =>
            {
                context.Response.StatusCode = this.configuration.GetValue<int>(ResponseStatusCodeConfigurationItem);
                
                if (context.Response.StatusCode != 204)
                    await context.Response.WriteAsync("");
            });
        }
    }
}