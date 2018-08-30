using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    [PublicAPI]
    public interface ITracer
    {
        [CanBeNull] TraceContext CurrentContext { get; set; }

        [NotNull] ISpanBuilder BeginSpan();
    }
}