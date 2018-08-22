using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    [PublicAPI]
    public static class WithAnnotationTracerExtensions
    {
        [Pure]
        public static ITracer WithAnnotation(this ITracer tracer, string key, string value, bool allowOverwrite = false)
        {
            return new WithAnnotationTracer(tracer, key, () => value, allowOverwrite, true);
        }

        [Pure]
        public static ITracer WithAnnotation(this ITracer tracer, string key, Func<string> value, bool allowOverwrite = false, bool allowNullValues = false)
        {
            return new WithAnnotationTracer(tracer, key, value, allowOverwrite, allowNullValues);
        }

        [Pure]
        public static ITracer WithAnnotations(this ITracer tracer, IReadOnlyDictionary<string, string> properties, bool allowOverwrite = false, bool allowNullValues = false)
        {
            return new WithAnnotationsTracer(tracer, () => properties?.Select(pair => (pair.Key, pair.Value)), allowOverwrite, allowNullValues);
        }

        [Pure]
        public static ITracer WithAnnotations(this ITracer tracer, Func<IEnumerable<(string, string)>> properties, bool allowOverwrite = false, bool allowNullValues = false)
        {
            return new WithAnnotationsTracer(tracer, properties, allowOverwrite, allowNullValues);
        }

        private class WithAnnotationTracer : ITracer
        {
            private readonly ITracer baseTracer;
            private readonly string key;
            private readonly Func<string> value;
            private readonly bool allowOverwrite;
            private readonly bool allowNullValues;

            public WithAnnotationTracer(ITracer baseTracer, string key, Func<string> value, bool allowOverwrite, bool allowNullValues)
            {
                this.baseTracer = baseTracer ?? throw new ArgumentNullException(nameof(baseTracer));
                this.key = key ?? throw new ArgumentNullException(nameof(key));
                this.value = value;
                this.allowOverwrite = allowOverwrite;
                this.allowNullValues = allowNullValues;
            }

            public TraceContext CurrentContext
            {
                get => baseTracer.CurrentContext;
                set => baseTracer.CurrentContext = value;
            }

            public ISpanBuilder BeginSpan()
            {
                var span = baseTracer.BeginSpan();

                var valueObject = value();
                if (valueObject != null || allowNullValues)
                {
                    span.SetAnnotation(key, valueObject, allowOverwrite);
                }

                return span;
            }
        }

        private class WithAnnotationsTracer : ITracer
        {
            private readonly ITracer baseTracer;
            private readonly Func<IEnumerable<(string, string)>> propertiesProvider;
            private readonly bool allowOverwrite;
            private readonly bool allowNullValues;

            public WithAnnotationsTracer(ITracer baseTracer, Func<IEnumerable<(string, string)>> propertiesProvider, bool allowOverwrite, bool allowNullValues)
            {
                this.baseTracer = baseTracer ?? throw new ArgumentNullException(nameof(baseTracer));
                this.propertiesProvider = propertiesProvider ?? throw new ArgumentNullException(nameof(propertiesProvider));
                this.allowOverwrite = allowOverwrite;
                this.allowNullValues = allowNullValues;
            }

            public TraceContext CurrentContext
            {
                get => baseTracer.CurrentContext;
                set => baseTracer.CurrentContext = value;
            }

            public ISpanBuilder BeginSpan()
            {
                var span = baseTracer.BeginSpan();

                foreach (var (key, value) in propertiesProvider() ?? Enumerable.Empty<(string, string)>())
                {
                    if (!allowNullValues && value == null)
                        continue;

                    span.SetAnnotation(key, value, allowOverwrite);
                }

                return span;
            }
        }
    }
}