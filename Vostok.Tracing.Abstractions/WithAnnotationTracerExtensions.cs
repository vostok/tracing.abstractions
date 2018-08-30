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
        public static ITracer WithAnnotation(this ITracer tracer, [NotNull] string key, [CanBeNull] string value, bool allowOverwrite = false)
        {
            return new WithAnnotationTracer(tracer, key, () => value, allowOverwrite, true);
        }

        [Pure]
        public static ITracer WithAnnotation(this ITracer tracer, [NotNull] string key, [NotNull] Func<string> value, bool allowOverwrite = false, bool allowNullValues = false)
        {
            return new WithAnnotationTracer(tracer, key, value, allowOverwrite, allowNullValues);
        }

        [Pure]
        public static ITracer WithAnnotations(this ITracer tracer, [NotNull] IReadOnlyDictionary<string, string> annotations, bool allowOverwrite = false, bool allowNullValues = false)
        {
            return new WithAnnotationsTracer(tracer, () => annotations?.Select(pair => (pair.Key, pair.Value)), allowOverwrite, allowNullValues);
        }

        [Pure]
        public static ITracer WithAnnotations(this ITracer tracer, [NotNull] Func<IEnumerable<(string, string)>> annotations, bool allowOverwrite = false, bool allowNullValues = false)
        {
            return new WithAnnotationsTracer(tracer, annotations, allowOverwrite, allowNullValues);
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
                this.value = value ?? throw new ArgumentNullException(nameof(value));
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
                var spanBuilder = baseTracer.BeginSpan();

                var valueString = value();
                if (valueString != null || allowNullValues)
                {
                    spanBuilder.SetAnnotation(key, valueString, allowOverwrite);
                }

                return spanBuilder;
            }
        }

        private class WithAnnotationsTracer : ITracer
        {
            private readonly ITracer baseTracer;
            private readonly Func<IEnumerable<(string, string)>> annotationsProvider;
            private readonly bool allowOverwrite;
            private readonly bool allowNullValues;

            public WithAnnotationsTracer(ITracer baseTracer, Func<IEnumerable<(string, string)>> annotationsProvider, bool allowOverwrite, bool allowNullValues)
            {
                this.baseTracer = baseTracer ?? throw new ArgumentNullException(nameof(baseTracer));
                this.annotationsProvider = annotationsProvider ?? throw new ArgumentNullException(nameof(annotationsProvider));
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
                var spanBuilder = baseTracer.BeginSpan();

                foreach (var (key, value) in annotationsProvider() ?? Enumerable.Empty<(string, string)>())
                {
                    if (!allowNullValues && value == null)
                        continue;

                    spanBuilder.SetAnnotation(key, value, allowOverwrite);
                }

                return spanBuilder;
            }
        }
    }
}
