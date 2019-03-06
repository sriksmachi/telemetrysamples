using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Net.Http;
using GreenPipes;
using System.Diagnostics;
using System;

namespace AzureFunctions.GreenPipes
{
    public class TelemetryFilter<TContext> : IFilter<TContext>
        where TContext : HttpFunctionRequestContext
    {
        public void Probe(ProbeContext context)
        {
            context.CreateScope("Telemetry");
        }

        public async Task Send(TContext context, IPipe<TContext> next)
        {
            var correlationId = context.HttpRequestMessage.Headers.Contains("correlationId") ?
                context.HttpRequestMessage.Headers.GetValues("correlationId").FirstOrDefault() : null;

            if (!string.IsNullOrEmpty(correlationId))
            {
                // Use the correlation id to log to App Insights here
                Console.WriteLine("Correlation Id :" + correlationId);
            }

            await next.Send(context).ConfigureAwait(false);
        }
    }
}
