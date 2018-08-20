using System;

namespace Vostok.Tracing.Abstractions
{
    public class TraceContext
    {
        public TraceContext(Guid traceId, Guid spanId)
        {
            TraceId = traceId;
            SpanId = spanId;
        }

        public Guid TraceId { get; }
        public Guid SpanId { get; }
    }
}