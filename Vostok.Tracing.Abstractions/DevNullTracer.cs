using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    /// <summary>
    /// A trivial <see cref="ITracer"/> implementation that does not manage context or record spans.
    /// </summary>
    [PublicAPI]
    public class DevNullTracer : ITracer
    {
        public TraceContext CurrentContext
        {
            get => null;
            set {}
        }

        public ISpanBuilder BeginSpan() => DevNullSpanBuilder.Instance;

        #region DevNullSpanBuilder

        private class DevNullSpanBuilder : ISpanBuilder
        {
            public static readonly DevNullSpanBuilder Instance = new DevNullSpanBuilder();

            public ISpan CurrentSpan => DevNullSpan.Instance;

            public void SetAnnotation(string key, object value, bool allowOverwrite = true)
            {
            }

            public void SetBeginTimestamp(DateTimeOffset timestamp)
            {
            }

            public void SetEndTimestamp(DateTimeOffset? timestamp)
            {
            }

            public void Dispose()
            {
            }
        }

        #endregion

        #region DevNullSpan

        private class DevNullSpan : ISpan
        {
            public static readonly DevNullSpan Instance = new DevNullSpan();

            public Guid TraceId => Guid.Empty;

            public Guid SpanId => Guid.Empty;

            public Guid? ParentSpanId => null;

            public DateTimeOffset BeginTimestamp => DateTimeOffset.MinValue;

            public DateTimeOffset? EndTimestamp => DateTimeOffset.MinValue;

            public IReadOnlyDictionary<string, object> Annotations { get; } = new Dictionary<string, object>();
        }

        #endregion
    }
}
