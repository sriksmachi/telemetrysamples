﻿using System.Collections.Generic;
using System.Linq;
using GreenPipes;

namespace AzureFunctions.GreenPipes
{
    public class TelemetrySpecification<TContext> : IPipeSpecification<TContext>
        where TContext : HttpFunctionRequestContext
    {
        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public void Apply(IPipeBuilder<TContext> builder)
        {
            builder.AddFilter(new TelemetryFilter<TContext>());
        }
    }
}
