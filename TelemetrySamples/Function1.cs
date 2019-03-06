using AzureFunctions.GreenPipes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TelemetrySamples
{
    [GreenPipesInjector]
    public static class Function1
    {
        /// <summary>
        /// Runs the specified req.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <param name="log">The log.</param>
        /// <returns></returns>
        [FunctionName("Function1")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, ILogger log)
        {
            try
            {
                string message = "Hello World";
                return new OkObjectResult(message);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.Azure.WebJobs.Host.FunctionInvocationFilterAttribute" />
    public class GreenPipesInjectorAttribute : FunctionInvocationFilterAttribute
    {
        /// <summary>
        /// Called when [executing asynchronous].
        /// </summary>
        /// <param name="executingContext">The executing context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">Http Request is not the first argument!</exception>
        public override async Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
        {
            var httpRequest = executingContext.Arguments.First().Value as HttpRequestMessage;

            if (httpRequest == null)
                throw new ValidationException("Http Request is not the first argument!");

            // all the pipelines must right into this context, so we need to find a way to circulate this within function as a single object per request
            var context = new HttpFunctionRequestContext(nameof(Function1), httpRequest);

            await Pipelines.Default.Send(context).ConfigureAwait(false);

            await base.OnExecutingAsync(executingContext, cancellationToken);
        }

    }
}
