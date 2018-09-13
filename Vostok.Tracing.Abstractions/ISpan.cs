using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    /// <summary>
    /// <para>A span represents any significant event or interaction in a distrubted system, such as HTTP request or database query.</para>
    /// <para>A single span always describes a local event in a single process: an HTTP request usually produces two spans (client-side and server-side).</para>
    /// <para>Span is an atomic building block of traces: every span is marked with a <see cref="TraceId"/> of a trace it belongs to.</para>
    /// <para>Spans in a single trace form a tree through <see cref="ParentSpanId"/> links. Root span does not have a parent.</para>
    /// </summary>
    [PublicAPI]
    public interface ISpan
    {
        /// <summary>
        /// Unique identifier of the trace containing the span.
        /// </summary>
        Guid TraceId { get; }

        /// <summary>
        /// <para>Identifier of the span itself. </para>
        /// <para>It must be unique in the scope of current <see cref="TraceId"/>.</para>
        /// </summary>
        Guid SpanId { get; }

        /// <summary>
        /// <para><see cref="SpanId"/> of the parent span in the tree.</para>
        /// <para>Can be <c>null</c> for root span.</para>
        /// </summary>
        [CanBeNull] Guid? ParentSpanId { get; }

        /// <summary>
        /// Beginning timestamp of the event or interaction described by this span.
        /// </summary>
        DateTimeOffset BeginTimestamp { get; }

        /// <summary>
        /// <para>Ending timestamp of the event or interaction described by this span.</para>
        /// <para>It's always measured with the same clock as <see cref="BeginTimestamp"/>. This allows to derive span duration as a difference between <see cref="EndTimestamp"/> and <see cref="BeginTimestamp"/>.</para>
        /// <para>However, it may be absent for a special kind of 'endless' spans (see <see cref="WellKnownSpanKinds.Queue.TaskLifecycle"/>).</para>
        /// </summary>
        [CanBeNull] DateTimeOffset? EndTimestamp { get; }

        /// <summary>
        /// Span payload in key-value string format. See <see cref="WellKnownAnnotations"/> for examples of annotations content.
        /// </summary>
        [NotNull] IReadOnlyDictionary<string, string> Annotations { get; }
    }
}