using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    /// <summary>
    /// Represents a span.
    /// </summary>
    [PublicAPI]
    public interface ISpan
    {
        /// <summary>
        /// Trace identifier
        /// </summary>
        Guid TraceId { get; }

        /// <summary>
        /// Current span identifier
        /// </summary>
        Guid SpanId { get; }

        /// <summary>
        /// Pareny span identifier, if exist
        /// </summary>
        [CanBeNull] Guid? ParentSpanId { get; }

        /// <summary>
        /// The begin timestamp of trace. Represents the time when the span was begin. The local time zone is kept here for future use. 
        /// </summary>
        DateTimeOffset BeginTimestamp { get; }

        /// <summary>
        /// The end timestamp of trace. Represents the time when the span was end. The local time zone is kept here for future use. 
        /// </summary>
        [CanBeNull] DateTimeOffset? EndTimestamp { get; }

        /// <summary>
        /// Contains various user-defined annotations.
        /// </summary>
        [NotNull] IReadOnlyDictionary<string, string> Annotations { get; }
    }
}