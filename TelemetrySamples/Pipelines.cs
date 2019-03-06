using System;
using System.Net;
using System.Net.Http;
using GreenPipes;

namespace AzureFunctions.GreenPipes
{
    public static class Pipelines
    {
        public static IPipe<AuthorizationFailureContext<HttpFunctionRequestContext>> AuthorizationFailurePipe =
            Pipe.New<AuthorizationFailureContext<HttpFunctionRequestContext>>(cfg => cfg.UseInlineFilter(async (cxt, next) =>
            {
                // this is still debatable as we are breaking the pipe here    
                throw new ArgumentException("Authorization Code Not found");
            }));


        public static IPipe<HttpFunctionRequestContext> Default =
            Pipe.New<HttpFunctionRequestContext>(cfg =>
            {
                cfg.AddPipeSpecification(new AuthorizationSpecification<HttpFunctionRequestContext>(AuthorizationFailurePipe, "abc-token"));
                cfg.AddPipeSpecification(new TelemetrySpecification<HttpFunctionRequestContext>());
            });
    }
}
