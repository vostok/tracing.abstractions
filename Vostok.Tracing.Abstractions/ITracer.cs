using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    [PublicAPI]
    public interface ITracer
    {
        [NotNull]
        TraceContext CurrentContext { get; set; }

        [NotNull]
        ISpanBuilder BeginSpan();
    }
}