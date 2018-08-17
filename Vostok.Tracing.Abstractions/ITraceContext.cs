using System;

namespace Vostok.Tracing.Abstractions
{
    public interface ITraceContext
    {
        Guid TraceId { get; }
        Guid SpanId { get; }
    }
}