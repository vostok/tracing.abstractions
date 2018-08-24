using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    [PublicAPI]
    public static class SpanBuilder_Extension
    {
        public static void MakeEndless(this ISpanBuilder spanBuilder)
        {
            spanBuilder.IsEndless = true;
        }
    }
}