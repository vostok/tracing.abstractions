using JetBrains.Annotations;

namespace Vostok.Tracing.Abstractions
{
    [PublicAPI]
    public static class ISpanBuilderExtensions
    {
        public static void MakeEndless(this ISpanBuilder spanBuilder)
        {
            spanBuilder.IsEndless = true;
        }
    }
}