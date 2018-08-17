using System;
using System.Collections.Generic;

namespace Vostok.Tracing.Abstractions
{
    public interface ISpan
    {
        Guid TraceId { get; set; }
        Guid SpanId { get; set; }
        Guid? ParentSpanId { get; set; }
        DateTimeOffset BeginTimestamp { get; set; }
        DateTimeOffset? EndTimestamp { get; set; }
        IDictionary<string, string> Annotations { get; set; }
    }
}