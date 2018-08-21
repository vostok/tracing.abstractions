using System;
using System.Collections.Generic;

namespace Vostok.Tracing.Abstractions
{
    public class TraceContext
    {
        public TraceContext(Guid traceId, Guid spanId, IDictionary<string,string> inheritAnnotations = null)
        {
            TraceId = traceId;
            SpanId = spanId;

            InheritAnnotations = inheritAnnotations?? new Dictionary<string, string>();
        }

        public Guid TraceId { get; }
        public Guid SpanId { get; }

        // CR(iloktionov): Remove this from public API.
        public IDictionary<string, string> InheritAnnotations { get;  }

        public void AddToInheritAnnotations(string key, string value)
        {
            InheritAnnotations.Add(key, value);
        }
    }
}