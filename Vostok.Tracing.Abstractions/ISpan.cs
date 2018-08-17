using System;
using System.Collections.Generic;

namespace Vostok.Tracing.Abstractions
{
    public interface ISpan
    {
        Guid TraceId { get; }
        Guid SpanId { get; }
        Guid? ParentSpanId { get; }
        DateTimeOffset BeginTimestamp { get; }
        DateTimeOffset? EndTimestamp { get; }
        IReadOnlyDictionary<string, string> Annotations { get; }
    }
}